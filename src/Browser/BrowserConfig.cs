﻿using CefSharp;

namespace SharpBrowser.Browser {
	internal static class BrowserConfig {

		/// <summary>
		/// The title of the window and application in Windows
		/// </summary>
		public static string Branding = "SharpBrowser";
		/// <summary>
		/// The language you distribute it in
		/// </summary>
		public static string AcceptLanguage = "en-US,en;q=0.9";
		/// <summary>
		/// The browser's user agent string, which identifies itself to websites
		/// </summary>
		public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion; // UserAgent to fix issue with Google account authentication		
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
		/// Is LocalStorage enabled for webpages?
		/// </summary>
		public static bool LocalStorage = false;
		/// <summary>
		/// Is WebGL enabled for webpages?
		/// </summary>
		public static bool WebGL = true;
		/// <summary>
		/// Is JS enabled for webpages?
		/// </summary>
		public static bool Javascript = true;
		/// <summary>
		/// Can JS on webpages access the clipboard?
		/// </summary>
		public static bool JavascriptClipboard = true;

		/// <summary>
		/// If true then the following proxy is used for all browsing and downloads.
		/// </summary>
		public static bool Proxy = false;
		public static string ProxyIP = "123.123.123.123";
		public static int ProxyPort = 123;
		public static string ProxyUsername = "username";
		public static string ProxyPassword = "pass";
		public static string ProxyBypassList = "";

	}
}
