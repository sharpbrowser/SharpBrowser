using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CefSharp;
using CefSharp.WinForms;
using SharpBrowser.Controls.BrowserTabStrip;
using Timer = System.Windows.Forms.Timer;
using System.Drawing;
using System.Reflection;
using SharpBrowser.Browser;
using SharpBrowser.Browser.Model;
using SharpBrowser.Controls;
using SharpBrowser.Properties;
using System.Resources;
using System.Threading.Tasks;
using CefSharp.WinForms.Experimental;

namespace SharpBrowser {

	/// <summary>
	/// The main SharpBrowser form, supporting multiple tabs.
	/// We used the x86 version of CefSharp, so the app works on 32-bit and 64-bit machines.
	/// If you would only like to support 64-bit machines, simply change the DLL references.
	/// </summary>
	internal partial class MainForm : Form {
		private string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
		public static MainForm Instance;

		public MainForm() {
			Instance = this;

			InitializeComponent();

			InitBrowser();
			SetFormTitle(null);


		}

		Panel pnlToolbarOverlay;
		private void MainForm_Load(object sender, EventArgs e) {

			InitAppIcon();
			InitTooltips(this.Controls);
			InitHotkeys();

			InitFAButton_DisabledColors();
			TxtURL.MakeTextbox_CustomBorderColor();

			//cant  do this on gui. paneltoolbar gets deleted. buggy designer 
			//PanelToolbar location Fix -2025
			PanelToolbar.Dock = DockStyle.None;
			PanelToolbar.BringToFront();
			PanelToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			PanelToolbar.Location = new Point(0, TabPages.TabButton_Height + 1); //for different dpi, need stable way to get it.				 
			PanelToolbar.Width = this.Width;
			PanelToolbar.Width = pnlToolbarOverlay.Width;


			if (Debugger.IsAttached)
				pnlToolbarOverlay.BackColor = Color.Cyan;

		}

        private void InitFAButton_DisabledColors()
        {
            BtnBack.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
            BtnForward.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
            BtnStop.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
            BtnRefresh.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
            BtnDownloads.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
            BtnHome.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
            BtnMenu.EnabledChanged += (s1, e1) => handleFAButton_DisabledColors(s1);
        }
        private async void handleFAButton_DisabledColors(object senderBtn)
        {
            var faBtn = senderBtn as FontAwesome.Sharp.IconButton;
			//faBtn.IconColor = faBtn.Enabled ? Color.Black : PanelToolbar.BackColor.ChangeColorBrightness(0);
			faBtn.IconColor = faBtn.Enabled ? Color.Black : Color.Black.ChangeColorBrightness(0.8);


        }


        #region App Icon

        /// <summary>
        /// embedding the resource using the Visual Studio designer results in a blurry icon.
        /// the best way to get a non-blurry icon for Windows 7 apps.
        /// </summary>
        private void InitAppIcon() {
			assembly = Assembly.GetAssembly(typeof(MainForm));
			Icon = new Icon(GetResourceStream("sharpbrowser.ico"), new Size(64, 64));
		}

		public static Assembly assembly = null;
		public Stream GetResourceStream(string filename, bool withNamespace = true) {
			try {
				return assembly.GetManifestResourceStream("SharpBrowser.Resources." + filename);
			}
			catch (System.Exception ex) { }
			return null;
		}

		#endregion

		#region Tooltips & Hotkeys

		/// <summary>
		/// these hotkeys work when the user is focussed on the .NET form and its controls,
		/// AND when the user is focussed on the browser (CefSharp portion)
		/// </summary>
		private void InitHotkeys() {

			// browser hotkeys
			KeyboardHandler.AddHotKey(this, CloseActiveTab, Keys.W, true);
			KeyboardHandler.AddHotKey(this, CloseActiveTab, Keys.Escape, true);
			KeyboardHandler.AddHotKey(this, AddBlankWindow, Keys.N, true);
			KeyboardHandler.AddHotKey(this, AddBlankTab, Keys.T, true);
			KeyboardHandler.AddHotKey(this, RefreshActiveTab, Keys.F5);
			KeyboardHandler.AddHotKey(this, OpenDeveloperTools, Keys.F12);
			KeyboardHandler.AddHotKey(this, NextTab, Keys.Tab, true);
			KeyboardHandler.AddHotKey(this, PrevTab, Keys.Tab, true, true);

			// search hotkeys
			KeyboardHandler.AddHotKey(this, OpenSearch, Keys.F, true);
			KeyboardHandler.AddHotKey(this, CloseSearch, Keys.Escape);
			KeyboardHandler.AddHotKey(this, StopActiveTab, Keys.Escape);
			KeyboardHandler.AddHotKey(this, ToggleFullscreen, Keys.F11);


		}

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

		#region Web Browser & Tabs

		private BrowserTabStripItem newStrip;
		private BrowserTabStripItem downloadsStrip;

		private string currentFullURL;
		private string currentCleanURL;
		private string currentTitle;

		public HostHandler host;
		private DownloadHandler dHandler;
		private ContextMenuHandler mHandler;
		private LifeSpanHandler lHandler;
		private KeyboardHandler kHandler;
		private RequestHandler rHandler;

		/// <summary>
		/// this is done just once, to globally initialize CefSharp/CEF
		/// </summary>
		private void InitBrowser() {

			//CefSharpSettings.LegacyJavascriptBindingEnabled = true;

			//Cef.EnableHighDPISupport();
			CefSettings settings = new CefSettings();

			settings.RegisterScheme(new CefCustomScheme {
				SchemeName = BrowserConfig.InternalURL,
				SchemeHandlerFactory = new SchemeHandlerFactory()
			});

			settings.UserAgent = BrowserConfig.UserAgent;
			settings.AcceptLanguageList = BrowserConfig.AcceptLanguage;

			settings.IgnoreCertificateErrors = true;

			settings.CachePath = GetAppDir("Cache");

			if (BrowserConfig.Proxy) {
				CefSharpSettings.Proxy = new ProxyOptions(BrowserConfig.ProxyIP,
					BrowserConfig.ProxyPort.ToString(), BrowserConfig.ProxyUsername,
					BrowserConfig.ProxyPassword, BrowserConfig.ProxyBypassList);
			}

			Cef.Initialize(settings);

			dHandler = new DownloadHandler(this);
			lHandler = new LifeSpanHandler(this);
			mHandler = new ContextMenuHandler(this);
			kHandler = new KeyboardHandler(this);
			rHandler = new RequestHandler(this);

			InitDownloads();

			host = new HostHandler(this);

			AddNewBrowser(tabStrip1, BrowserConfig.HomepageURL);

		}

		/// <summary>
		/// this is done every time a new tab is openede
		/// </summary>
		private void ConfigureBrowser(ChromiumWebBrowser browser) {

			BrowserSettings config = new BrowserSettings();

			//config.FileAccessFromFileUrls = (!CrossDomainSecurity).ToCefState();
			//config.UniversalAccessFromFileUrls = (!CrossDomainSecurity).ToCefState();
			//config.WebSecurity = WebSecurity.ToCefState();
			config.WebGl = BrowserConfig.WebGL.ToCefState();
			//config.ApplicationCache = ApplicationCache.ToCefState();

			browser.BrowserSettings = config;

		}


		private static string GetAppDir(string name) {
			string winXPDir = @"C:\Documents and Settings\All Users\Application Data\";
			if (Directory.Exists(winXPDir)) {
				return winXPDir + BrowserConfig.Branding + @"\" + name + @"\";
			}
			return @"C:\ProgramData\" + BrowserConfig.Branding + @"\" + name + @"\";

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

				if (!(urlLower.StartsWith("http") || urlLower.StartsWith(BrowserConfig.InternalURL))) {
					if (outUri == null || outUri.Scheme != Uri.UriSchemeFile) newUrl = "http://" + url;
				}

				if (urlLower.StartsWith(BrowserConfig.InternalURL + ":") ||

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
			currentCleanURL = CleanURL(URL);

			TxtURL.Text = currentCleanURL;

			CurTab.CurURL = currentFullURL;

			CloseSearch();

		}

		private string CleanURL(string url) {
			if (url.BeginsWith("about:")) {
				return "";
			}
			url = url.RemovePrefix("http://");
			url = url.RemovePrefix("https://");
			url = url.RemovePrefix("file://");
			url = url.RemovePrefix("/");
			return url.DecodeURL();
		}
		private bool IsBlank(string url) {
			return (url == "" || url == "about:blank");
		}
		private bool IsBlankOrSystem(string url) {
			return (url == "" || url.BeginsWith("about:") || url.BeginsWith("chrome:") || url.BeginsWith(BrowserConfig.InternalURL + ":"));
		}

		public void AddBlankWindow() {

			// open a new instance of the browser

			ProcessStartInfo info = new ProcessStartInfo(Application.ExecutablePath, "");
			//info.WorkingDirectory = workingDir ?? exePath.GetPathDir(true);
			info.LoadUserProfile = true;

			info.UseShellExecute = false;
			info.RedirectStandardError = true;
			info.RedirectStandardOutput = true;
			info.RedirectStandardInput = true;

			Process.Start(info);
		}
		public void AddBlankTab() {
			AddNewBrowserTab("");
			this.InvokeOnParent(delegate () {
				TxtURL.Focus();
			});
		}

		public ChromiumWebBrowser AddNewBrowserTab(string url, bool focusNewTab = true, string refererUrl = null) {
			return (ChromiumWebBrowser)this.Invoke((Func<ChromiumWebBrowser>)delegate {

				// check if already exists
				foreach (BrowserTabStripItem tab in TabPages.Items) {
					BrowserTab tab2 = (BrowserTab)tab.Tag;
					if (tab2 != null && (tab2.CurURL == url)) {
						TabPages.SelectedItem = tab;
						return tab2.Browser;
					}
				}

				BrowserTabStripItem tabStrip = new BrowserTabStripItem();
				tabStrip.Title = "New Tab";
				TabPages.Items.Insert(TabPages.Items.Count - 1, tabStrip);
				newStrip = tabStrip;

				BrowserTab newTab = AddNewBrowser(newStrip, url);
				newTab.RefererURL = refererUrl;
				//if (focusNewTab) timer1.Enabled = true;  //50ms
				if (focusNewTab)
					Task.Run(() => {
						//ui is not frozen as i expecte. but AddNewTab button gets white?? for 1 seconds.
						Thread.Sleep(100);
						this.Invoke(() => TabPages.SelectedItem = newStrip);
					});


				return newTab.Browser;
			});
		}
		private BrowserTab AddNewBrowser(BrowserTabStripItem tabStrip, String url) {
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
			browser.StatusMessage += Browser_StatusMessage;
			browser.LoadingStateChanged += Browser_LoadingStateChanged;
			browser.TitleChanged += Browser_TitleChanged;
			browser.LoadError += Browser_LoadError;
			browser.AddressChanged += Browser_URLChanged;
			browser.DownloadHandler = dHandler;
			browser.MenuHandler = mHandler;
			browser.LifeSpanHandler = lHandler;
			browser.KeyboardHandler = kHandler;
			browser.RequestHandler = rHandler;

			//listen zoomLevel Changed
			browser.JavascriptMessageReceived += Browser_JavascriptMessageReceived_mswheel;
			browser.FrameLoadEnd += Browser_FrameLoadEnd_mswheel;

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

			if (url.StartsWith(BrowserConfig.InternalURL + ":")) {
				browser.JavascriptObjectRepository.Register("host", host, BindingOptions.DefaultBinder);
			}
			return tab;
		}




		public BrowserTab GetTabByBrowser(IWebBrowser browser) {
			foreach (BrowserTabStripItem tab2 in TabPages.Items) {
				BrowserTab tab = (BrowserTab)(tab2.Tag);
				if (tab != null && tab.Browser == browser) {
					return tab;
				}
			}
			return null;
		}

		public void RefreshActiveTab() => CurBrowser.Load(CurBrowser.Address);

		public void CloseActiveTab() {
			if (CurTab != null/* && TabPages.Items.Count > 2*/) {

				// remove tab and save its index
				int index = TabPages.Items.IndexOf(TabPages.SelectedItem);
				TabPages.RemoveTab(TabPages.SelectedItem);

				// keep tab at same index focussed
				if ((TabPages.Items.Count - 1) > index) {
					TabPages.SelectedItem = TabPages.Items[index];
				}
			}
		}
		private FormWindowState oldWindowState;
		private FormBorderStyle oldBorderStyle;
		private bool isFullScreen = false;

		private void ToggleFullscreen() {

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

		private void OnTabClosed(object sender, EventArgs e) { }

		private void OnTabClosing(SharpBrowser.Controls.BrowserTabStrip.TabStripItemClosingEventArgs e) {

			// exit if invalid tab
			if (CurTab == null) {
				e.Cancel = true;
				return;
			}

			// add a blank tab if the very last tab is closed!
			if (TabPages.Items.Count <= 2) {
				AddBlankTab();
				//e.Cancel = true;
			}

		}

		private void StopActiveTab() => CurBrowser.Stop();

		private bool IsOnFirstTab() {
			return TabPages.SelectedItem == TabPages.Items[0];
		}
		private bool IsOnLastTab() {
			return TabPages.SelectedItem == TabPages.Items[TabPages.Items.Count - 2];
		}

		private int CurIndex {
			get {
				return TabPages.Items.IndexOf(TabPages.SelectedItem);
			}
			set {
				TabPages.SelectedItem = TabPages.Items[value];
			}
		}
		private int LastIndex {
			get {
				return TabPages.Items.Count - 2;
			}
		}

		private void NextTab() {
			if (IsOnLastTab()) {
				CurIndex = 0;
			}
			else {
				CurIndex++;
			}
		}
		private void PrevTab() {
			if (IsOnFirstTab()) {
				CurIndex = LastIndex;
			}
			else {
				CurIndex--;
			}
		}

		public ChromiumWebBrowser CurBrowser {
			get {
				if (TabPages.SelectedItem != null && TabPages.SelectedItem.Tag != null) {
					return ((BrowserTab)TabPages.SelectedItem.Tag).Browser;
				}
				else {
					return null;
				}
			}
		}

		public BrowserTab CurTab {
			get {
				if (TabPages.SelectedItem != null && TabPages.SelectedItem.Tag != null) {
					return ((BrowserTab)TabPages.SelectedItem.Tag);
				}
				else {
					return null;
				}
			}
		}
		public List<BrowserTab> GetAllTabs() {
			List<BrowserTab> tabs = new List<BrowserTab>();
			foreach (BrowserTabStripItem tabPage in TabPages.Items) {
				if (tabPage.Tag != null) {
					tabs.Add((BrowserTab)tabPage.Tag);
				}
			}
			return tabs;
		}

		public int CurTabLoadingDur {
			get {
				if (TabPages.SelectedItem != null && TabPages.SelectedItem.Tag != null) {
					int loadTime = (int)(DateTime.Now - CurTab.DateCreated).TotalMilliseconds;
					return loadTime;
				}
				else {
					return 0;
				}
			}
		}

		private void Browser_URLChanged(object sender, AddressChangedEventArgs e) {
			InvokeIfNeeded(() => {
				// if current tab
				if (sender == CurBrowser) {

					if (!Utils.IsFocussed(TxtURL)) {
						SetFormURL(e.Address);
					}

					EnableBackButton(CurBrowser.CanGoBack);
					EnableForwardButton(CurBrowser.CanGoForward);

					SetTabTitle((ChromiumWebBrowser)sender, "Loading...");
					Task.Run(() => Get_ZoomLevel_intoTbx(CurBrowser).Wait());

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

		private void SetTabTitle(ChromiumWebBrowser browser, string text) {

			text = text.Trim();
			if (IsBlank(text)) {
				text = "New Tab";
			}

			// save text
			browser.Tag = text;

			// get tab of given browser
			BrowserTabStripItem tabStrip = (BrowserTabStripItem)browser.Parent;
			if (tabStrip != null) //fix error, when fast hit on close button 
				tabStrip.Title = text;

			// if current tab
			if (browser == CurBrowser) {

				SetFormTitle(text);

			}
		}

		private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e) {
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
		}

		public void InvokeIfNeeded(Action action) {
			if (this.InvokeRequired) {
				this.BeginInvoke(action);
			}
			else {
				action.Invoke();
			}
		}

		private void Browser_StatusMessage(object sender, StatusMessageEventArgs e) { }

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

		private async void OnTabsChanged(TabStripItemChangedEventArgs e) {
			ChromiumWebBrowser browser = null;
			try {
				//reduce exception - freqent exception is perf Killer
				if (e.Item.Controls.Count >= 1 && e.Item.Controls[0] as ChromiumWebBrowser != null)
					browser = ((ChromiumWebBrowser)e.Item.Controls[0]);
			}
			catch (System.Exception ex) {
				//"- when closing Tab, this executes 10 times, if you have 10 tabs. --- aka perf killer.";
				var Msg = DateTime.Now.ToLongTimeString() + $" OnTabsChanged -ex: {ex.Message} ";
			}


			if (e.ChangeType == BrowserTabStripItemChangeTypes.SelectionChanged) {
				if (TabPages.SelectedItem == tabStripAdd) {
					AddBlankTab();
				}
				else {
					browser = CurBrowser;

					SetFormURL(browser.Address);
					SetFormTitle(browser.Tag.ConvertToString() ?? "New Tab");
					await Get_ZoomLevel_intoTbx(browser);

					EnableBackButton(browser.CanGoBack);
					EnableForwardButton(browser.CanGoForward);
				}
			}

			if (e.ChangeType == BrowserTabStripItemChangeTypes.Removed) {
				if (e.Item == downloadsStrip) downloadsStrip = null;
				if (browser != null) {
					browser.Dispose();
				}
			}

			if (e.ChangeType == BrowserTabStripItemChangeTypes.Changed) {
				if (browser != null) {
					if (currentFullURL != "about:blank") {
						browser.Focus();
					}
				}
			}

		}


		#region Ctrl+Mousewheel Zoom

		private void Browser_FrameLoadEnd_mswheel(object sender, FrameLoadEndEventArgs e) {
			if (e.Frame.IsMain) {
				e.Browser.ExecuteScriptAsync(
"""     
 document.addEventListener("wheel", (e) => {
        if (e.ctrlKey) 
        {  
            /*ctrl is down*/
            var msg ="ctrl+wheel";
            console.log(msg); 
            CefSharp.PostMessage(msg);
        }
    });
 
"""
				);
			}
		}
		private void Browser_JavascriptMessageReceived_mswheel(object sender, JavascriptMessageReceivedEventArgs e) {
			if (e.Message == null)
				return;

			// to reference the main UI thread for updating the necessary controls.
			if (e.Message + "" == "ctrl+wheel") {
				InvokeIfNeeded(async () => {

					await Task.Delay(55);
					await Get_ZoomLevel_intoTbx(CurBrowser);
				});
			}
		}
		int convertZoomlevel_toZoomPct(double zoomLevel) {
			
			// expo fit - y= 100.011 e^(0.182307 x)
			var zoomPct = 100.011 * Math.Exp(0.182307 * zoomLevel);
			return (int)zoomPct;

		}
		private async Task Get_ZoomLevel_intoTbx(ChromiumWebBrowser browser) {
			//InvokeIfNeeded(() => lbl_ZoomLevel.Text = $"...%");
			await Task.Delay(100);

			if (browser.IsDisposed)
				return;
			var zoom = await browser.GetZoomLevelAsync();
			int zoomPct = convertZoomlevel_toZoomPct(zoom);
			//var zoom2 = await browser.BrowserCore.GetZoomLevelAsync();
			//InvokeIfNeeded(() => lbl_ZoomLevel.Text = $"{Math.Round(zoom, 4)} ; {zoomPct}?%");
			InvokeIfNeeded(() => lbl_ZoomLevel.Text = $"{zoomPct}%");
			InvokeIfNeeded(() => lbl_ZoomLevel.Visible = zoom != 0);

			//await Task.Delay(500);
			//lbl_ZoomLevel.Text = $"{(int)(zoom2 * 100)}%";

		}
		private async void lbl_ZoomLevel_Click(object sender, EventArgs e) {
			CurBrowser.SetZoomLevel(0);//0 -> 100%   [-10 , 0 , +10]
			await Get_ZoomLevel_intoTbx(CurBrowser);
		}
		private async void lbl_ZoomLevel_MouseEnter(object sender, EventArgs e) {
			await Get_ZoomLevel_intoTbx(CurBrowser);
		}

		#endregion

		private void timer1_Tick(object sender, EventArgs e) {
			TabPages.SelectedItem = newStrip;
			timer1.Enabled = false;
		}

		private void menuCloseTab_Click(object sender, EventArgs e) => CloseActiveTab();

		private void menuCloseOtherTabs_Click(object sender, EventArgs e) {
			List<BrowserTabStripItem> listToClose = new List<BrowserTabStripItem>();
			foreach (BrowserTabStripItem tab in TabPages.Items) {
				if (tab != tabStripAdd && tab != TabPages.SelectedItem) listToClose.Add(tab);
			}
			foreach (BrowserTabStripItem tab in listToClose) {
				TabPages.RemoveTab(tab);
			}

		}

		public List<int> CancelRequests => downloadCancelRequests;

		private void bBack_Click(object sender, EventArgs e) => CurBrowser.Back();

		private void bForward_Click(object sender, EventArgs e) => CurBrowser.Forward();



		private void bDownloads_Click(object sender, EventArgs e) => AddNewBrowserTab(BrowserConfig.DownloadsURL);

		private void bRefresh_Click(object sender, EventArgs e) => RefreshActiveTab();

		private void bStop_Click(object sender, EventArgs e) => StopActiveTab();

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
			if (e.IsHotkey(Keys.C, true) && Utils.IsFullySelected(TxtURL)) {

				// copy the real URL, not the pretty one
				Clipboard.SetText(CurBrowser.Address, TextDataFormat.UnicodeText);

				// im handling this
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		//-----urlbar text selection behavior---
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


		private void OpenDeveloperTools() => CurBrowser.ShowDevTools();

		private void tabPages_MouseClick(object sender, MouseEventArgs e) {
			/*if (e.Button == System.Windows.Forms.MouseButtons.Right) {
				tabPages.GetTabItemByPoint(this.mouse
			}*/
		}

		#endregion

		#region Download Queue

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {

			// ask user if they are sure
			if (DownloadsInProgress()) {
				if (MessageBox.Show("Downloads are in progress. Cancel those and exit?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
					e.Cancel = true;
					return;
				}
			}

			// dispose all browsers
			try {
				foreach (TabPage tab in TabPages.Items) {
					ChromiumWebBrowser browser = (ChromiumWebBrowser)tab.Controls[0];
					browser.Dispose();
				}
			}
			catch (System.Exception ex) { }

		}

		public Dictionary<int, DownloadItem> downloads;
		public Dictionary<int, string> downloadNames;
		public List<int> downloadCancelRequests;

		/// <summary>
		/// we must store download metadata in a list, since CefSharp does not
		/// </summary>
		private void InitDownloads() {

			downloads = new Dictionary<int, DownloadItem>();
			downloadNames = new Dictionary<int, string>();
			downloadCancelRequests = new List<int>();

		}

		public Dictionary<int, DownloadItem> Downloads => downloads;

		public void UpdateDownloadItem(DownloadItem item) {
			lock (downloads) {

				// SuggestedFileName comes full only in the first attempt so keep it somewhere
				if (item.SuggestedFileName != "") {
					downloadNames[item.Id] = item.SuggestedFileName;
				}

				// Set it back if it is empty
				if (item.SuggestedFileName == "" && downloadNames.ContainsKey(item.Id)) {
					item.SuggestedFileName = downloadNames[item.Id];
				}

				downloads[item.Id] = item;

				//UpdateSnipProgress();
			}
		}

		public string CalcDownloadPath(DownloadItem item) => item.SuggestedFileName;

		public bool DownloadsInProgress() {
			foreach (DownloadItem item in downloads.Values) {
				if (item.IsInProgress) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// open a new tab with the downloads URL
		/// </summary>
		private void btnDownloads_Click(object sender, EventArgs e) => OpenDownloadsTab();

		public void OpenDownloadsTab() {
			if (downloadsStrip != null && ((ChromiumWebBrowser)downloadsStrip.Controls[0]).Address == BrowserConfig.DownloadsURL) {
				TabPages.SelectedItem = downloadsStrip;
			}
			else {
				ChromiumWebBrowser brw = AddNewBrowserTab(BrowserConfig.DownloadsURL);
				downloadsStrip = (BrowserTabStripItem)brw.Parent;
			}
		}

		#endregion

		#region Search Bar

		bool searchOpen = false;
		string lastSearch = "";

		private void OpenSearch() {
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
		private void CloseSearch() {
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

		private void FindTextOnPage(bool next = true) {
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



		private void BtnMenu_Click(object sender, EventArgs e) {

		}
	}
}