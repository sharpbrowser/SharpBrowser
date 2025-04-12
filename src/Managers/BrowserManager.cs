using CefSharp.WinForms;
using CefSharp;
using SharpBrowser.Handlers;
using System.IO;
using SharpBrowser.Config;
using SharpBrowser.Utils;

namespace SharpBrowser.Managers {
	internal static class BrowserManager {

		public static HostHandler _HostHandler;

		private static DownloadHandler dHandler;
		private static ContextMenuHandler mHandler;
		private static LifeSpanHandler lHandler;
		private static KeyboardHandler kHandler;
		private static RequestHandler rHandler;
		private static PermissionHandler pHandler;

		public static void Init(MainForm form) {

			if (Cef.IsInitialized != true) {
				CefSettings settings = new CefSettings();

				settings.RegisterScheme(new CefCustomScheme {
					SchemeName = BrowserConfig.InternalScheme,
					SchemeHandlerFactory = new SchemeHandlerFactory()
				});

				//------------------------------------------------------------
				// FIX: this prevents a crash if 2 CefSharp apps are opened at once

				// init cache dirs in AppData Roaming
				var rcPath = Path.Combine(ConfigManager.AppDataPath, "CefCache");
				// fix: CachePath MUST be a child of the RootCachePath as of CEF 128+
				var cPath = Path.Combine(rcPath, "_TempCache");

				// create cache dirs
				rcPath.EnsureFolderExists();
				cPath.EnsureFolderExists();

				settings.RootCachePath = rcPath;
				settings.CachePath = cPath;
				//------------------------------------------------------------

				BrowserConfig.GetCefSettings(settings);

				Cef.Initialize(settings);

			}

			if (dHandler == null) {

				dHandler = new DownloadHandler(form);
				lHandler = new LifeSpanHandler(form);
				mHandler = new ContextMenuHandler(form);
				kHandler = new KeyboardHandler(form);
				rHandler = new RequestHandler(form);
				pHandler = new PermissionHandler();
				_HostHandler = new HostHandler(form);

			}
		}

		/// <summary>
		/// Register our handlers with the given CefSharp browser instance.
		/// </summary>
		public static void SetupHandlers(ChromiumWebBrowser browser) {
			browser.DownloadHandler = dHandler;
			browser.MenuHandler = mHandler;
			browser.LifeSpanHandler = lHandler;
			browser.KeyboardHandler = kHandler;
			browser.RequestHandler = rHandler;
			browser.PermissionHandler = pHandler;
		}
	}
}