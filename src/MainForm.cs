using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using CefSharp;
using CefSharp.WinForms;
using SharpBrowser.Controls.BrowserTabStrip;
using System.Drawing;
using SharpBrowser.Managers;
using SharpBrowser.Controls;
using SharpBrowser.Handlers;
using SharpBrowser.Config;
using SharpBrowser.Model;
using SharpBrowser.Utils;

namespace SharpBrowser {

	/// <summary>
	/// The main SharpBrowser form, supporting multiple tabs.
	/// We used the x86 version of CefSharp, so the app works on 32-bit and 64-bit machines.
	/// If you would only like to support 64-bit machines, simply change the DLL references.
	/// </summary>
	internal partial class MainForm : Form {

		public static MainForm Instance;

		public MainForm() {
			Instance = this;

			InitializeComponent();

		}

		Panel pnlToolbarOverlay;
		private void MainForm_Load(object sender, EventArgs e) {


			// init managers
			ConfigManager.Init(BrowserConfig.AppID);
			DownloadManager.Init();
			BrowserManager.Init(this);
			InitFavIcons();

			// init the browser UI
			InitBrowser();
			SetFormTitle(null);
			IconManager.Init(this);
			InitTooltips(this.Controls);
			HotkeyManager.Init(this);
			InitDisabledIcons();
			InitToolbar();

			// start with the last tabs
			LoadLastTabs();

		}



		#region Tooltips & Hotkeys

		/// <summary>
		/// we activate all the tooltips stored in the Tag property of the buttons
		/// </summary>
		public void InitTooltips(System.Windows.Forms.Control.ControlCollection parent) {
			foreach (Control ui in parent) {
				Button btn = ui as Button;
				if (btn != null) {
					if (btn.Tag != null) {
						ToolTip tip = new ToolTip();
						tip.ReshowDelay = tip.InitialDelay = 200;
						tip.ShowAlways = true;
						tip.SetToolTip(btn, btn.Tag.ToString());
					}
				}
				Panel panel = ui as Panel;
				if (panel != null) {
					InitTooltips(panel.Controls);
				}
			}
		}

		#endregion

		#region Browser UI


		private void SetFormTitle(string tabName) {

			if (tabName.CheckIfValid()) {

				this.Text = tabName + " - " + BrowserConfig.Branding;
				currentTitle = tabName;

			}
			else {

				this.Text = BrowserConfig.Branding;
				currentTitle = "New Tab";
			}

		}

		private void SetFormURL(string URL) {

			currentFullURL = URL;
			currentCleanURL = URLUtils.CleanURL(URL);

			TxtURL.Text = currentCleanURL;

			if (CurTab != null) {
				CurTab.CurURL = currentFullURL;
			}

			CloseSearch();

		}


		#endregion

		#region Web Browser & Tabs

		private string currentFullURL;
		private string currentCleanURL;
		private string currentTitle;

		/// <summary>
		/// this is done just once, to globally initialize CefSharp/CEF
		/// </summary>
		private void InitBrowser() {

			AddNewBrowser(tabStrip1, BrowserConfig.HomepageURL);

		}

		/// <summary>
		/// this is done every time a new tab is openede
		/// </summary>
		private void ConfigureBrowser(ChromiumWebBrowser browser) {
			browser.BrowserSettings = BrowserConfig.GetCefConfig();

		}

		private void InitFavIcons() {
			FavIconManager.OnLoaded = Manager_OnFavIconLoaded;
			FavIconManager.Init();
		}

		private void LoadURL(string url) {
			Uri outUri;
			string newUrl = url;
			string urlLower = url.Trim().ToLower();

			// UI
			SetTabTitle(CurBrowser, "Loading...");

			// load page
			if (urlLower == "localhost") {

				newUrl = "http://localhost/";

			}
			else if (url.CheckIfFilePath() || url.CheckIfFilePath2()) {

				newUrl = url.PathToURL();

			}
			else {

				Uri.TryCreate(url, UriKind.Absolute, out outUri);

				if (!(urlLower.StartsWith("http") || urlLower.StartsWith(BrowserConfig.InternalScheme))) {
					if (outUri == null || outUri.Scheme != Uri.UriSchemeFile) newUrl = "http://" + url;
				}

				if (urlLower.Contains("://") ||

					// load URL if it seems valid
					(Uri.TryCreate(newUrl, UriKind.Absolute, out outUri)
					 && ((outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps) && newUrl.Contains(".") || outUri.Scheme == Uri.UriSchemeFile))) {

				}
				else {

					// run search if unknown URL
					newUrl = BrowserConfig.SearchURL + HttpUtility.UrlEncode(url);

				}

			}

			// load URL
			CurBrowser.Load(newUrl);

			// set URL in UI
			SetFormURL(newUrl);

			// always enable back btn
			EnableBackButton(true);
			EnableForwardButton(false);

		}

		public ChromiumWebBrowser AddNewBrowserTab(string url, bool focusNewTab = true, string refererUrl = null, bool skipIfUrlAlreadyOpen = false, bool focusOnAddressBar = false) {
			return Invoke((Func<ChromiumWebBrowser>)delegate {

				// check if already exists
				if (skipIfUrlAlreadyOpen) {
					foreach (BrowserTabPage tab in TabPages.Items) {
						BrowserTab tab2 = (BrowserTab)tab.Tag;
						if (tab2 != null && (tab2.CurURL == url)) {
							TabPages.SelectedTab = tab;
							return tab2.Browser;
						}
					}
				}

				// new tab button and select it
				BrowserTabPage tabStrip = new BrowserTabPage();
				tabStrip.Title = "New Tab";
				TabPages.AddTab(tabStrip, focusNewTab);

				// new browser
				BrowserTab newTab = AddNewBrowser(tabStrip, url);
				newTab.RefererURL = refererUrl;

				// focus on address bar
				if (focusOnAddressBar) {
					Thread.Sleep(1000);
					TxtURL.Focus();
				}

				return newTab.Browser;
			});
		}

		private BrowserTab AddNewBrowser(BrowserTabPage tabStrip, String url) {
			if (url == "") url = BrowserConfig.NewTabURL;
			ChromiumWebBrowser browser = new ChromiumWebBrowser(url);

			// set config
			ConfigureBrowser(browser);

			// set layout
			browser.Dock = DockStyle.Fill;
			tabStrip.Controls.Add(browser);
			pnlToolbarOverlay = new Panel() {
				Width = PanelToolbar.Width,
				Height = PanelToolbar.Height,
				Dock = DockStyle.Top,
			};
			tabStrip.Controls.Add(pnlToolbarOverlay);
			browser.BringToFront();

			// add events
			//browser.StatusMessage += Browser_StatusMessage;
			browser.LoadingStateChanged += Browser_LoadingStateChanged;
			browser.FrameLoadEnd += Browser_FrameLoadEnd;
			browser.TitleChanged += Browser_TitleChanged;
			browser.LoadError += Browser_LoadError;
			browser.AddressChanged += Browser_URLChanged;
			BrowserManager.SetupHandlers(browser);

			// new tab obj
			BrowserTab tab = new BrowserTab {
				IsOpen = true,
				Browser = browser,
				Tab = tabStrip,
				OrigURL = url,
				CurURL = url,
				Title = "New Tab",
				DateCreated = DateTime.Now
			};

			// save tab obj in tabstrip
			tabStrip.Tag = tab;

			// handle downloads page
			if (url.StartsWith(BrowserConfig.InternalScheme + ":")) {
				browser.JavascriptObjectRepository.Register("host", BrowserManager._HostHandler, BindingOptions.DefaultBinder);
			}

			return tab;
		}

		public BrowserTab GetTabByBrowser(IWebBrowser browser) {
			foreach (BrowserTabPage tab2 in TabPages.Items) {
				BrowserTab tab = (BrowserTab)(tab2.Tag);
				if (tab != null && tab.Browser == browser) {
					return tab;
				}
			}
			return null;
		}


		private void OnTabClosed(object sender, EventArgs e) {

			// if the very last tab is closed
			if (TabPages.Items.Count < 1) {
				OnLastTabClosed();
			}

		}

		private void OnLastTabClosed() {

			// close the window
			this.Close();

			// the new tab is bugged so this is disabled
			//AddNewTabInternal(BrowserConfig.NewTabURL, null);
		}

		public void StopActiveTab() => CurBrowser.Stop();

		private bool IsOnFirstTab() {
			return TabPages.SelectedTab == TabPages.Items[0];
		}
		private bool IsOnLastTab() {
			return TabPages.SelectedTab == TabPages.Items[TabPages.Items.Count - 1];
		}

		private int LastIndex {
			get {
				return TabPages.Items.Count - 1;
			}
		}

		public ChromiumWebBrowser CurBrowser {
			get {
				if (TabPages.SelectedTab != null && TabPages.SelectedTab.Tag != null) {
					return ((BrowserTab)TabPages.SelectedTab.Tag).Browser;
				}
				else {
					return null;
				}
			}
		}

		public BrowserTab CurTab {
			get {
				if (TabPages.SelectedTab != null && TabPages.SelectedTab.Tag != null) {
					return ((BrowserTab)TabPages.SelectedTab.Tag);
				}
				else {
					return null;
				}
			}
		}
		public List<BrowserTab> GetAllTabs() {
			List<BrowserTab> tabs = new List<BrowserTab>();
			foreach (BrowserTabPage tabPage in TabPages.Items) {
				if (tabPage.Tag != null) {
					tabs.Add((BrowserTab)tabPage.Tag);
				}
			}
			return tabs;
		}

		public bool IsFavIconLoaded(ChromiumWebBrowser browser) {
			var tab = GetTabByBrowser(browser);
			return (tab != null && tab.FavIcon != null);
		}

		private void SetTabTitle(ChromiumWebBrowser browser, string text) {

			text = text.Trim();
			if (URLUtils.IsBlank(text)) {
				text = "New Tab";
			}

			// save text
			browser.Tag = text;

			// get tab of given browser
			BrowserTabPage tabStrip = (BrowserTabPage)browser.Parent;
			if (tabStrip != null) //fix error, when fast hit on close button 
				tabStrip.Title = text;

			// if current tab
			if (browser == CurBrowser) {

				SetFormTitle(text);

			}
		}

		public void InvokeIfNeeded(Action action) {
			if (this.InvokeRequired) {
				this.BeginInvoke(action);
			}
			else {
				action.Invoke();
			}
		}

		public void WaitForBrowserToInitialize(ChromiumWebBrowser browser) {
			while (!browser.IsBrowserInitialized) {
				Thread.Sleep(100);
			}
		}

		private void EnableBackButton(bool canGoBack) {
			InvokeIfNeeded(() => BtnBack.Enabled = canGoBack);
		}
		private void EnableForwardButton(bool canGoForward) {
			InvokeIfNeeded(() => BtnForward.Enabled = canGoForward);
		}

		private async void OnNewTab(object o, EventArgs e) {
			AddBlankTab();
		}
		private async void OnTabsChanged(TabStripItemChangedEventArgs e) {
			ChromiumWebBrowser browser = null;

			try {
				// apparently this 'fix' reduces frequent exceptions
				if (e.Item.Controls.Count >= 1 && e.Item.Controls[0] as ChromiumWebBrowser != null)
					browser = ((ChromiumWebBrowser)e.Item.Controls[0]);
			}
			catch (System.Exception ex) {
			}


			if (e.ChangeType == BrowserTabStripItemChangeTypes.SelectionChanged) {
				Tabs_OnSwitchedTab();

			}

			if (e.ChangeType == BrowserTabStripItemChangeTypes.Removed) {
				if (browser != null) {
					browser.Dispose();
				}
			}

		}


		private void TMReload_Click(object sender, EventArgs e) {
			RefreshActiveTab();
		}
		private void TMCloseTab_Click(object sender, EventArgs e) {
			CloseActiveTab();
		}

		private void TMCloseOtherTabs_Click(object sender, EventArgs e) {
			CloseOtherTabs();
		}


		private void bBack_Click(object sender, EventArgs e) { Back(); }


		private void bForward_Click(object sender, EventArgs e) { Forward(); }


		private void bDownloads_Click(object sender, EventArgs e) {
			OpenDownloads();
		}

		private void bRefresh_Click(object sender, EventArgs e) => RefreshActiveTab();

		private void bStop_Click(object sender, EventArgs e) => StopActiveTab();


		#endregion

		#region Web Browser Events

		private ChromiumWebBrowser Tabs_OnSwitchedTab() {
			ChromiumWebBrowser browser = CurBrowser;
			if (browser != null) {

				// load the text/URL from this tab into the window
				SetFormURL(browser.Address);
				SetFormTitle(browser.Tag.ConvertToString() ?? "New Tab");

				EnableBackButton(browser.CanGoBack);
				EnableForwardButton(browser.CanGoForward);

				// focus on the browser
				// FIX: important for hotkeys to work (for example a chain of Ctrl+W, Ctrl+W)
				browser.Focus();
			}
			else {

				// when a new tab is just created, the browser does not exist, so show default text/URL
				SetFormURL("");
				SetFormTitle("Loading...");

				EnableBackButton(false);
				EnableForwardButton(false);

			}

			return browser;
		}

		private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {

			// for any tab, even background tabs, download the favicon
			if (e.Frame.IsMain) {

				if (!URLUtils.IsBlankOrSystem(e.Url)) {

					FavIconManager.LoadFavicon(sender as ChromiumWebBrowser, false);
				}
			}
		}

		private void Manager_OnFavIconLoaded(ChromiumWebBrowser browser, byte[] iconData) {
			InvokeIfNeeded(() => {

				var tab = GetTabByBrowser(browser);
				if (tab != null && tab.Tab != null) {
					var bitmap = new Bitmap(new MemoryStream(iconData));
					tab.FavIcon = bitmap;
					tab.Tab.Image = bitmap;
				}

			});
		}

		private void Browser_URLChanged(object sender, AddressChangedEventArgs e) {
			InvokeIfNeeded(() => {
				// if current tab
				if (sender == CurBrowser) {

					if (!WinFormsUtils.IsFocussed(TxtURL)) {
						SetFormURL(e.Address);
					}

					EnableBackButton(CurBrowser.CanGoBack);
					EnableForwardButton(CurBrowser.CanGoForward);

					SetTabTitle((ChromiumWebBrowser)sender, "Loading...");

					BtnRefresh.Visible = false;
					BtnStop.Visible = true;

					CurTab.DateCreated = DateTime.Now;

				}

			});
		}

		private void Browser_LoadError(object sender, LoadErrorEventArgs e) {
			// ("Load Error:" + e.ErrorCode + ";" + e.ErrorText);
		}

		private void Browser_TitleChanged(object sender, TitleChangedEventArgs e) {
			InvokeIfNeeded(() => {

				ChromiumWebBrowser browser = (ChromiumWebBrowser)sender;

				SetTabTitle(browser, e.Title);

			});
		}

		private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e) {

			var browser = sender as ChromiumWebBrowser;

			// if its the current tab, update browser UI based on its state
			if (sender == CurBrowser) {

				EnableBackButton(e.CanGoBack);
				EnableForwardButton(e.CanGoForward);

				if (e.IsLoading) {

					// set title
					//SetTabTitle();

				}
				else {

					// after loaded / stopped
					InvokeIfNeeded(() => {
						BtnRefresh.Visible = true;
						BtnStop.Visible = false;
					});
				}
			}

			// for any tab, check favicons
			if (!URLUtils.IsBlankOrSystem(browser.Address)) {
				if (!IsFavIconLoaded(browser)) {
					FavIconManager.LoadFavicon(sender as ChromiumWebBrowser, true);
				}
			}

		}

		private void Browser_StatusMessage(object sender, StatusMessageEventArgs e) { }


		#endregion

		#region Web Browser Commands

		public void AddBlankWindow() {

			// DISABLED BECAUSE 2 CEFSHARP INSTANCES CAUSES A CRASH

			/*// open a new instance of the browser

			ProcessStartInfo info = new ProcessStartInfo(Application.ExecutablePath, "");
			//info.WorkingDirectory = workingDir ?? exePath.GetPathDir(true);
			info.LoadUserProfile = true;

			info.UseShellExecute = false;
			info.RedirectStandardError = true;
			info.RedirectStandardOutput = true;
			info.RedirectStandardInput = true;

			Process.Start(info);*/
		}
		public void AddBlankTab() {
			AddNewBrowserTab(BrowserConfig.NewTabURL, true, null, false, true);
		}

		public void Back() {
			CurBrowser.Back();
		}
		public void Forward() {
			CurBrowser.Forward();
		}
		public void OpenDownloads() {
			AddNewBrowserTab(BrowserConfig.DownloadsURL, true, null, true);
		}
		public void OpenDeveloperTools() {
			CurBrowser.ShowDevTools();
		}
		public void CloseOtherTabs() {
			List<BrowserTabPage> listToClose = new List<BrowserTabPage>();
			foreach (BrowserTabPage tab in TabPages.Items) {
				if (tab != TabPages.SelectedTab) listToClose.Add(tab);
			}
			foreach (BrowserTabPage tab in listToClose) {
				TabPages.RemoveTab(tab);
			}
		}

		public void RefreshActiveTab() => CurBrowser.Load(CurBrowser.Address);

		public void CloseActiveTab() {

			// if any tab is open
			var curTab = TabPages.SelectedTab;
			if (CurTab != null) {

				// if this is the last tab, open a new tab also
				if (TabPages.Items.Count <= 1) {
					OnLastTabClosed();
				}

				// remove tab and save its index
				int index = TabPages.Items.IndexOf(curTab);
				TabPages.RemoveTab(curTab);

				// keep tab at same index focussed
				if (TabPages.Items.Count > 1) {
					if ((TabPages.Items.Count - 1) > index) {
						TabPages.SelectedTab = TabPages.Items[index];
					}
				}
			}
		}
		public void NextTab() {
			if (IsOnLastTab()) {
				TabPages.SelectedIndex = 0;
			}
			else {
				TabPages.SelectedIndex++;
			}
		}
		public void PrevTab() {
			if (IsOnFirstTab()) {
				TabPages.SelectedIndex = LastIndex;
			}
			else {
				TabPages.SelectedIndex--;
			}
		}
		public void Print() {
			CurBrowser.Print();
		}
		public void PrintToPDF() {
			ContextMenuHandler.SaveAsPDF(CurBrowser.GetBrowser());
		}


		#endregion

		#region Fullscreen

		private FormWindowState oldWindowState;
		private FormBorderStyle oldBorderStyle;
		private bool isFullScreen = false;
		public void ToggleFullscreen() {

			if (!isFullScreen) {
				oldWindowState = this.WindowState;
				oldBorderStyle = this.FormBorderStyle;
				this.FormBorderStyle = FormBorderStyle.None;
				this.WindowState = FormWindowState.Maximized;
				isFullScreen = true;
			}
			else {
				this.FormBorderStyle = oldBorderStyle;
				this.WindowState = oldWindowState;
				isFullScreen = false;
			}
		}

		#endregion

		#region Download Queue

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {

			// ask user if they are sure
			if (DownloadManager.DownloadsInProgress()) {
				if (MessageBox.Show("Downloads are in progress. Cancel those and exit?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
					e.Cancel = true;
					return;
				}
			}

			// save tabs
			SaveTabsBeforeClosing();

			// dispose all browsers
			try {
				foreach (TabPage tab in TabPages.Items) {
					ChromiumWebBrowser browser = (ChromiumWebBrowser)tab.Controls[0];
					browser.Dispose();
				}
			}
			catch (System.Exception ex) { }

		}

		/// <summary>
		/// open a new tab with the downloads URL
		/// </summary>
		private void btnDownloads_Click(object sender, EventArgs e) => OpenDownloads();

		#endregion

		#region Toolbar

		private void InitToolbar() {
			TxtURL.MakeTextbox_CustomBorderColor();
			PanelToolbar.Dock = DockStyle.None;
			PanelToolbar.BringToFront();
			PanelToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			PanelToolbar.Location = new Point(0, TabPages.TabButton_Height + 1); //for different dpi, need stable way to get it.				 
			PanelToolbar.Width = this.Width;
			PanelToolbar.Width = pnlToolbarOverlay.Width;

			if (Debugger.IsAttached)
				pnlToolbarOverlay.BackColor = Color.Cyan;
		}

		private void InitDisabledIcons() {
			BtnBack.EnabledChanged += (s1, e1) => SetIconByState(s1);
			BtnForward.EnabledChanged += (s1, e1) => SetIconByState(s1);
			BtnStop.EnabledChanged += (s1, e1) => SetIconByState(s1);
			BtnRefresh.EnabledChanged += (s1, e1) => SetIconByState(s1);
			BtnDownloads.EnabledChanged += (s1, e1) => SetIconByState(s1);
			BtnHome.EnabledChanged += (s1, e1) => SetIconByState(s1);
			BtnMenu.EnabledChanged += (s1, e1) => SetIconByState(s1);
		}
		private async void SetIconByState(object senderBtn) {
			var faBtn = senderBtn as FontAwesome.Sharp.IconButton;
			//faBtn.IconColor = faBtn.Enabled ? Color.Black : PanelToolbar.BackColor.ChangeColorBrightness(0);
			faBtn.IconColor = faBtn.Enabled ? Color.Black : Color.Black.ChangeColorBrightness(0.8);
		}

		#endregion

		#region Address Bar

		private void TxtURL_KeyDown(object sender, KeyEventArgs e) {

			// if ENTER or CTRL+ENTER pressed
			if (e.IsHotkey(Keys.Enter) || e.IsHotkey(Keys.Enter, true)) {
				LoadURL(TxtURL.Text);

				// im handling this
				e.Handled = true;
				e.SuppressKeyPress = true;

				// defocus from url textbox
				this.Focus();
			}

			// if full URL copied
			if (e.IsHotkey(Keys.C, true) && WinFormsUtils.IsFullySelected(TxtURL)) {

				// copy the real URL, not the pretty one
				Clipboard.SetText(CurBrowser.Address, TextDataFormat.UnicodeText);

				// im handling this
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private bool TxtURL_JustEntered = false;
		private void TxtURL_Enter(object sender, EventArgs e) {
			TxtURL.SelectAll();
			TxtURL_JustEntered = true;
		}
		private void TxtURL_Click(object sender, EventArgs e) {
			if (TxtURL_JustEntered) {
				TxtURL.SelectAll();
			}
			TxtURL_JustEntered = false;
		}


		#endregion

		#region Search Bar

		bool searchOpen = false;
		string lastSearch = "";

		public void OpenSearch() {
			if (!searchOpen) {
				searchOpen = true;
				InvokeIfNeeded(delegate () {
					PanelSearch.Visible = true;
					TxtSearch.Text = lastSearch;
					TxtSearch.Focus();
					TxtSearch.SelectAll();
				});
			}
			else {
				InvokeIfNeeded(delegate () {
					TxtSearch.Focus();
					TxtSearch.SelectAll();
				});
			}
		}
		public void CloseSearch() {
			if (searchOpen) {
				searchOpen = false;
				InvokeIfNeeded(delegate () {
					PanelSearch.Visible = false;
					CurBrowser.GetBrowser().StopFinding(true);
				});
			}
		}

		private void BtnClearSearch_Click(object sender, EventArgs e) => CloseSearch();

		private void BtnPrevSearch_Click(object sender, EventArgs e) => FindTextOnPage(false);
		private void BtnNextSearch_Click(object sender, EventArgs e) => FindTextOnPage(true);

		public void FindTextOnPage(bool next = true) {
			bool first = lastSearch != TxtSearch.Text;
			lastSearch = TxtSearch.Text;
			if (lastSearch.CheckIfValid()) {
				CurBrowser.GetBrowser().Find(lastSearch, true, false, !first);
			}
			else {
				CurBrowser.GetBrowser().StopFinding(true);
			}
			TxtSearch.Focus();
		}

		private void TxtSearch_TextChanged(object sender, EventArgs e) => FindTextOnPage(true);

		private void TxtSearch_KeyDown(object sender, KeyEventArgs e) {
			if (e.IsHotkey(Keys.Enter)) {
				FindTextOnPage(true);
			}
			if (e.IsHotkey(Keys.Enter, true) || e.IsHotkey(Keys.Enter, false, true)) {
				FindTextOnPage(false);
			}
		}



		#endregion

		#region Home Button
		private void BtnHome_Click(object sender, EventArgs e) => CurBrowser.Load(BrowserConfig.HomepageURL);
		#endregion

		#region Main Menu Button

		private void BtnMenu_Click(object sender, EventArgs e) {
			var buttonScreenPoint = BtnMenu.PointToScreen(Point.Empty);
			int x = buttonScreenPoint.X + BtnMenu.Width - MainMenu.Width;
			int y = buttonScreenPoint.Y + BtnMenu.Height;

			MainMenu.Show(x, y);
		}

		private void MMNewTab_Click(object sender, EventArgs e) {
			AddBlankTab();
		}

		private void MMNewWindow_Click(object sender, EventArgs e) {
			AddBlankWindow();
		}

		private void MMNextTab_Click(object sender, EventArgs e) {
			NextTab();
		}

		private void MMPrevTab_Click(object sender, EventArgs e) {
			PrevTab();
		}

		private void MMDownloads_Click(object sender, EventArgs e) {
			OpenDownloads();
		}

		private void MMPrint_Click(object sender, EventArgs e) {
			Print();
		}

		private void MMPrintPDF_Click(object sender, EventArgs e) {
			PrintToPDF();
		}

		private void MMClose_Click(object sender, EventArgs e) {
			CloseActiveTab();
		}

		private void MMCloseOther_Click(object sender, EventArgs e) {
			CloseOtherTabs();
		}

		private void MMDevTools_Click(object sender, EventArgs e) {
			OpenDeveloperTools();
		}
		private void MMFullscreen_Click(object sender, EventArgs e) {
			ToggleFullscreen();
		}
		#endregion

		#region Saved Settings

		private void LoadLastTabs() {
			if (BrowserConfig.SaveOpenTabs) {

				// load last tabs
				var tabs = ConfigManager.GetString("browser.lastTabs", "").Split("||");

				// open them all
				var added = false;
				foreach (var url in tabs) {
					if (url.Length > 0) {
						AddNewBrowserTab(url, false);
						added = true;
					}
				}

				// close the default tab if tabs were restored
				if (added) {
					CloseActiveTab();
				}

				// switch to the last active tab
				TabPages.SelectedIndex = ConfigManager.GetInt("browser.lastTab", 0);
			}
		}
		private void SaveTabsBeforeClosing() {
			if (BrowserConfig.SaveOpenTabs) {

				// get current URLs
				var urls = TabPages.Tabs.Select(t => (t.Tag as BrowserTab).CurURL).ToList().Join("||");

				// save in settings
				ConfigManager.Set("browser.lastTabs", urls);
				ConfigManager.Set("browser.lastTab", TabPages.SelectedIndex);
				ConfigManager.SaveSettings();
			}
		}

		#endregion


	}
}
