﻿using CefSharp;
using CefSharp.WinForms;
using System.Net;

namespace SharpBrowser.Config {
	internal static class BrowserConfig {

		/// <summary>
		/// The title of the window and application in Windows.
		/// </summary>
		public static string Branding = "SharpBrowser";
		/// <summary>
		/// The folder name in AppData.
		/// </summary>
		public static string AppID = "SharpBrowser";
		/// <summary>
		/// The language you distribute it in
		/// </summary>
		public static string AcceptLanguage = "en-US,en;q=0.9";
		/// <summary>
		/// The browser's user agent string, which identifies itself to websites
		/// </summary>
		public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 CefSharp/" + Cef.CefSharpVersion; // UserAgent to fix issue with Google account authentication		
		/// <summary>
		/// The home page of your browser which shows up when you press the home button
		/// </summary>
		public static string HomepageURL = "https://www.google.com";
		/// <summary>
		/// The page you will see when you press "New Tab".
		/// </summary>
		public static string NewTabURL = "about:blank";
		/// <summary>
		/// The main URL prefix for internal pages.
		/// </summary>
		public static string InternalScheme = "sharpbrowser";
		/// <summary>
		/// The URL for the downloads page
		/// </summary>
		public static string DownloadsURL = InternalScheme + "://storage/downloads.html";
		/// <summary>
		/// URL to display when local file is not fonud
		/// </summary>
		public static string FileNotFoundURL = InternalScheme + "://storage/errors/notFound.html";
		/// <summary>
		/// URL to display when internet connection is not available.
		/// </summary>
		public static string CannotConnectURL = InternalScheme + "://storage/errors/cannotConnect.html";
		/// <summary>
		/// The search string; it must be the string before the search result.
		/// Examples:
		/// https://www.google.com/search?q=
		/// https://www.bing.com/search?q=
		/// https://duckduckgo.com/?q=
		/// </summary>
		public static string SearchURL = "https://www.google.com/search?q=";



		/// <summary>
		/// Should we save the tabs that are open and re-open them on the next startup?
		/// </summary>
		public static bool SaveOpenTabs = true;
		/// <summary>
		/// Is WebGL enabled for webpages?
		/// </summary>
		public static bool WebGL = true;
		/// <summary>
		/// Can JS on webpages access WebRTC streams?
		/// </summary>
		public static bool WebRTC = true;
		/// <summary>
		/// Can JS on webpages access MIDI devices using WebMidi API?
		/// </summary>
		public static bool WebMidi = true;
		/// <summary>
		/// Can JS on webpages access Web Cameras?
		/// </summary>
		public static bool Camera = true;
		/// <summary>
		/// Can JS on webpages access Microphone?
		/// </summary>
		public static bool Microphone = true;
		/// <summary>
		/// Is JS enabled for webpages?
		/// </summary>
		public static bool Javascript = true;
		/// <summary>
		/// Can JS on webpages access the clipboard?
		/// </summary>
		public static bool JavascriptClipboard = true;
		/// <summary>
		/// Can webpages access local files?
		/// </summary>
		public static bool LocalFiles = false;
		/// <summary>
		/// Can webpages access local storage API?
		/// </summary>
		public static bool LocalStorage = true;
		/// <summary>
		/// Can users resize text areas on webpages?
		/// </summary>
		public static bool TextAreaResize = true;


		//3way to set proxy, needs enum to reduce complication.
		public static ProxyMode currentProxyMode = ProxyMode.AutoDetect;
		//public static bool useSystemProxy = true;

		/// <summary>
		/// If true then the following proxy is used for all browsing and downloads.
		/// </summary>
		//public static bool customProxy = false;
		public static string ProxyIP = "123.123.123.123";
		public static int ProxyPort = 123;
		public static string ProxyUsername = "username";
		public static string ProxyPassword = "pass";
		public static string ProxyBypassList = "";

		public enum ProxyMode 
		{
			/// <summary>
			/// aka use system proxy 
			/// </summary>
			AutoDetect,
			CustomProxy,
			NoProxy,
		}

		/// <summary>
		/// Load the above config into the CEF `BrowserSettings` object.
		/// </summary>
		public static BrowserSettings GetCefConfig() {
			BrowserSettings config = new BrowserSettings();

			config.TextAreaResize = TextAreaResize.ToCefState();
			config.LocalStorage = LocalStorage.ToCefState();
			config.WebGl = WebGL.ToCefState();
			config.Javascript = Javascript.ToCefState();
			config.JavascriptAccessClipboard = JavascriptClipboard.ToCefState();
			config.JavascriptCloseWindows = CefState.Disabled;
			config.JavascriptDomPaste = JavascriptClipboard.ToCefState();
			config.RemoteFonts = CefState.Enabled;

			return config;
		}
		public static CefState ToCefState(this bool value) {
			return value ? CefState.Enabled : CefState.Disabled;
		}

		/// <summary>
		/// Load the above config into the CEF `CefSettings` object.
		/// </summary>
		public static void GetCefSettings(CefSettings settings) {

			// add user agent settings
			settings.UserAgent = UserAgent;
			settings.AcceptLanguageList = AcceptLanguage;
			settings.IgnoreCertificateErrors = true;

			// needed for loading local images
			if (LocalFiles) {
				settings.CefCommandLineArgs.Add("disable-web-security", "1");
				settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1");
			}

			// enable webRTC streams
			if (WebRTC) {
				settings.CefCommandLineArgs.Add("enable-media-stream", "1");
			}

			if (currentProxyMode == ProxyMode.AutoDetect) 
			{
				//settings.CefCommandLineArgs.Add("proxy-auto-detect"); // or maybe do nothing.
			}
			else if (currentProxyMode == ProxyMode.CustomProxy)
			{
				// enable proxy if wanted
				CefSharpSettings.Proxy = new ProxyOptions(
					ProxyIP, 
					ProxyPort.ToString(), 
					ProxyUsername,ProxyPassword, 
					ProxyBypassList);
			}
			else if (currentProxyMode == ProxyMode.NoProxy)
			{

				// disable proxy if not wanted
				settings.CefCommandLineArgs.Add("no-proxy-server");
			}
		}

	}
}
