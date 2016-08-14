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
using FarsiLibrary.Win;

namespace SharpBrowser {
	public partial class MainForm : Form {
		private const int PROGRESS_DISABLED = -1;
		private const int PROGRESS_INDETERMINATE = -2;
		private volatile bool closing;
		private DownloadHandler dHandler;
		private MenuHandler mHandler;
		private LifeSpanHandler lHandler;
		private KeyboardHandler kHandler;
		private FATabStripItem newStrip;
		private FATabStripItem downloadsStrip;
		private string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
		public Dictionary<int, DownloadItem> downloads;
		public Dictionary<int, string> downloadNames;
		public List<int> downloadCancelRequests;
		public HostHandler host;

		private static string ChromeAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.79 Safari/537.1";
		private string startURL = "about:blank";
		private string downloadsURL = "chrome://storage/downloads.htm";


		private void MainForm_Load(object sender, EventArgs e) {

			this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

			SetStatusText("Ready");
			SetStatusProgress(PROGRESS_DISABLED);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			try {
				foreach (TabPage tab in tabPages.Items) {
					ChromiumWebBrowser browser = (ChromiumWebBrowser)tab.Controls[0];
					browser.Dispose();
				}
			}
			catch (System.Exception ex)
			{
				
			}
		}

		public MainForm() {
			InitializeComponent();

			CefSettings settings = new CefSettings();

			settings.RegisterScheme(new CefCustomScheme {
				SchemeName = SchemeHandlerFactory.SchemeName,
				SchemeHandlerFactory = new SchemeHandlerFactory()
			});

			settings.RegisterScheme(new CefCustomScheme {
				SchemeName = SchemeHandlerFactory.SchemeNameTest,
				SchemeHandlerFactory = new SchemeHandlerFactory()
			});

			settings.UserAgent = ChromeAgent;

			Cef.Initialize(settings);

			dHandler = new DownloadHandler(this);
			lHandler = new LifeSpanHandler(this);
			mHandler = new MenuHandler(this);
			kHandler = new KeyboardHandler(this);

			downloads = new Dictionary<int, DownloadItem>();
			downloadNames = new Dictionary<int, string>();
			downloadCancelRequests = new List<int>();

			host = new HostHandler(this);

			txtUrl.Text = startURL;
			AddNewBrowser(tabStrip1, startURL);

			SetFormTitle(null);

		}

		public ChromiumWebBrowser AddNewBrowserTab(String url, bool focusNewTab = true) {
			return (ChromiumWebBrowser)this.Invoke((Func<ChromiumWebBrowser>)delegate {
				FATabStripItem tabStrip = new FATabStripItem();
				tabStrip.Title = "about:blank";
				tabPages.Items.Insert(tabPages.Items.Count - 1, tabStrip);
				newStrip = tabStrip;
				ChromiumWebBrowser browser = AddNewBrowser(newStrip, url);
				if (focusNewTab) timer1.Enabled = true;
				return browser;
			});
		}

		private ChromiumWebBrowser AddNewBrowser(FATabStripItem tabStrip, String url) {
			if (url == "") url = startURL;
			ChromiumWebBrowser browser = new ChromiumWebBrowser(url);

			browser.Dock = DockStyle.Fill;
			tabStrip.Controls.Add(browser);
			browser.BringToFront();

			browser.StatusMessage += Browser_StatusMessage;
			browser.LoadingStateChanged += Browser_LoadingStateChanged;
			browser.TitleChanged += Browser_TitleChanged;
			browser.LoadError += Browser_LoadError;
			browser.AddressChanged += Browser_URLChanged;
			browser.DownloadHandler = dHandler;
			browser.MenuHandler = mHandler;
			browser.LifeSpanHandler = lHandler;
			browser.KeyboardHandler = kHandler;

			if (url.StartsWith("chrome:")) {
				browser.RegisterAsyncJsObject("host", host, true);
			}
			return browser;
		}

		public ChromiumWebBrowser Browser {
			get {
				if (tabPages.SelectedItem.Controls.Count > 0)
					return (ChromiumWebBrowser)tabPages.SelectedItem.Controls[0];
				else
					return null;
			}
		}

		private void SetFormTitle(string tabName) {
			if (tabName == null || tabName.Length == 0) {
				this.Text = "SharpBrowser";
			} else {
				this.Text = tabName + " - SharpBrowser";
			}
		}

		private void Browser_URLChanged(object sender, AddressChangedEventArgs e) {
			InvokeIfNeeded(() => {

				// if current tab
				if (sender == Browser) {

					txtUrl.Text = e.Address;

					SetCanGoBack(Browser.CanGoBack);
					SetCanGoForward(Browser.CanGoForward);

					SetTabTitle((ChromiumWebBrowser)sender, "Loading...");

				}

			});
		}

		private void Browser_LoadError(object sender, LoadErrorEventArgs e) {
			SetErrorText("Load Error:" + e.ErrorCode + ";" + e.ErrorText);
		}

		private void Browser_TitleChanged(object sender, TitleChangedEventArgs e) {
			InvokeIfNeeded(() => {

				SetTabTitle((ChromiumWebBrowser)sender, e.Title);

			});
		}

		private void SetTabTitle(ChromiumWebBrowser sender, string text) {

			text = text.Trim();
			if (text == "" || text == "about:blank") {
				text = "New Tab";
			}

			// get tab of given browser
			FATabStripItem tabStrip = (FATabStripItem)sender.Parent;
			tabStrip.Title = text;


			// if current tab
			if (sender == Browser) {

				SetFormTitle(text);

			}
		}

		private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e) {
			if (sender == Browser) {

				SetCanGoBack(e.CanGoBack);
				SetCanGoForward(e.CanGoForward);

				if (e.IsLoading) {

					// set title
					//SetTabTitle();

					// during loading
					SetStatusProgress(PROGRESS_INDETERMINATE);
					InvokeIfNeeded(() => {
						bRefresh.Visible = false;
						bStop.Visible = true;
					});
				} else {

					// after loaded / stopped
					SetStatusProgress(PROGRESS_DISABLED);
					InvokeIfNeeded(() => {
						bRefresh.Visible = true;
						bStop.Visible = false;
					});
				}
				SetErrorText("");
			}
		}

		public void InvokeIfNeeded(Action action) {
			if (this.InvokeRequired) {
				this.BeginInvoke(action);
			} else {
				action.Invoke();
			}
		}

		private void SetStatusText(string txt) {
			//InvokeIfNeeded(() => lblStatus.Text = txt);
		}

		private void SetErrorText(string txt) {
			// InvokeIfNeeded(() => lblError.Text = txt);
		}

		private void SetStatusProgress(int value) {
			/*if (closing)
				return;

			Invoke((Action)delegate
			{
				if (value == PROGRESS_DISABLED)
				{
					statusProgress.Visible = false;
					btnLoading.Image = global::SharpBrowser.Properties.Resources.loader2;
				}
				else if (value == PROGRESS_INDETERMINATE)
				{
					statusProgress.Visible = true;
					statusProgress.Style = ProgressBarStyle.Marquee;
					btnLoading.Image = global::SharpBrowser.Properties.Resources.loader;
				}
				else
				{
					statusProgress.Visible = true;
					statusProgress.Value = Math.Min(100, Math.Max(0, value));
				}
			});*/
		}

		private void Browser_StatusMessage(object sender, StatusMessageEventArgs e) {
			SetStatusText(e.Value);
		}

		public void WaitForBrowserToInitialize(ChromiumWebBrowser browser) {
			while (!browser.IsBrowserInitialized) {
				Thread.Sleep(100);
			}
		}

		private void LoadUrl(string url) {
			Uri outUri;
			string newUrl = url;
			string urlToLower = url.Trim().ToLower();

			// UI
			SetTabTitle(Browser, "Loading...");

			// load page
			if (urlToLower == "localhost") {
				Browser.Load("http://localhost/");
			} else {

				Uri.TryCreate(url, UriKind.Absolute, out outUri);

				if (!(urlToLower.StartsWith("http") || urlToLower.StartsWith(SchemeHandlerFactory.SchemeName) || urlToLower.StartsWith(SchemeHandlerFactory.SchemeNameTest))) {
					if (outUri == null || outUri.Scheme != Uri.UriSchemeFile) newUrl = "http://" + url;
				}

				if (urlToLower.StartsWith(SchemeHandlerFactory.SchemeName + ":") || urlToLower.StartsWith(SchemeHandlerFactory.SchemeNameTest + ":") ||

					// load URL if it seems valid
					(Uri.TryCreate(newUrl, UriKind.Absolute, out outUri)
					 && ((outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps) && newUrl.Contains(".") || outUri.Scheme == Uri.UriSchemeFile))) {
					Browser.Load(newUrl);

				} else {

					// search google if unknown URL
					string searchURL = "https://www.google.com/?q=";
					Browser.Load(searchURL + HttpUtility.UrlEncode(url));
				}

			}
		}

		private void SetCanGoBack(bool canGoBack) {
			InvokeIfNeeded(() => bBack.Enabled = canGoBack);
		}

		private void SetCanGoForward(bool canGoForward) {
			InvokeIfNeeded(() => bForward.Enabled = canGoForward);
		}

		private void txtUrl_Enter(object sender, EventArgs e) {
			BeginInvoke((Action)delegate {
				txtUrl.SelectAll();
			});
		}

		private void tabPages_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e) {
			if (e.ChangeType == FATabStripItemChangeTypes.SelectionChanged) {
				if (tabPages.SelectedItem == tabStripAdd) {
					AddNewBrowserTab("");
				} else {
					txtUrl.Text = Browser.Address;
					if (Browser.IsLoading) SetStatusProgress(PROGRESS_INDETERMINATE);
					else SetStatusProgress(PROGRESS_DISABLED);
					SetCanGoBack(Browser.CanGoBack);
					SetCanGoForward(Browser.CanGoForward);
				}
			}
			if (e.ChangeType == FATabStripItemChangeTypes.Removed) {
				if (e.Item == downloadsStrip) downloadsStrip = null;
				if (e.Item.Controls.Count > 0) {
					((ChromiumWebBrowser)e.Item.Controls[0]).Dispose();
				}
			}
			if (e.ChangeType == FATabStripItemChangeTypes.Changed) {
				if (e.Item.Controls.Count > 0) {
					((ChromiumWebBrowser)e.Item.Controls[0]).Focus();
				}
			}
		}

		private void timer1_Tick(object sender, EventArgs e) {
			tabPages.SelectedItem = newStrip;
			timer1.Enabled = false;
		}

		public void CloseActiveTab() {
			FATabStripItem activeStrip = tabPages.SelectedItem;
			if (activeStrip.Controls.Count > 0) {
				if (activeStrip.Controls[0] is ChromiumWebBrowser) {
					InvokeIfNeeded(() => {
						tabPages.RemoveTab(activeStrip);
					});
				}
			}
		}

		public void UpdateDownloadItem(DownloadItem item) {
			lock (downloads) {
				//SuggestedFileName comes full only in the first attempt so keep it somewhere
				if (item.SuggestedFileName != "") downloadNames[item.Id] = item.SuggestedFileName;

				//Set it back if it is empty
				if (item.SuggestedFileName == "" && downloadNames.ContainsKey(item.Id)) item.SuggestedFileName = downloadNames[item.Id];

				downloads[item.Id] = item;
			}
		}

		private void btnDownloads_Click(object sender, EventArgs e) {
			if (downloadsStrip != null && ((ChromiumWebBrowser)downloadsStrip.Controls[0]).Address == downloadsURL) {
				tabPages.SelectedItem = downloadsStrip;
			} else {
				ChromiumWebBrowser brw = AddNewBrowserTab(downloadsURL);
				downloadsStrip = (FATabStripItem)brw.Parent;
			}
		}

		private void menuCloseTab_Click(object sender, EventArgs e) {
			CloseActiveTab();
		}

		private void menuCloseOtherTabs_Click(object sender, EventArgs e) {
			List<FATabStripItem> listToClose = new List<FATabStripItem>();
			foreach (FATabStripItem tab in tabPages.Items) {
				if (tab != tabStripAdd && tab != tabPages.SelectedItem) listToClose.Add(tab);
			}
			foreach (FATabStripItem tab in listToClose) {
				tabPages.RemoveTab(tab);
			}

		}

		public Dictionary<int, DownloadItem> Downloads {
			get {
				return downloads;
			}
		}

		public List<int> CancelRequests {
			get {
				return downloadCancelRequests;
			}
		}

		private void bBack_Click(object sender, EventArgs e) {
			Browser.Back();
		}

		private void bForward_Click(object sender, EventArgs e) {
			Browser.Forward();
		}

		private void txtUrl_TextChanged(object sender, EventArgs e) {

		}

		private void bDownloads_Click(object sender, EventArgs e) {
			AddNewBrowserTab(downloadsURL);
		}

		private void bRefresh_Click(object sender, EventArgs e) {
			Browser.Load(Browser.Address);
		}

		private void bStop_Click(object sender, EventArgs e) {
			Browser.Stop();
		}

		private void txtUrl_KeyDown_1(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				LoadUrl(txtUrl.Text);
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void txtUrl_Click(object sender, EventArgs e) {
			txtUrl.SelectAll();
		}

		//  Browser.ShowDevTools();

	}
}