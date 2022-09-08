using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser.Browser {
	internal static class BrowserConfig {


		public static string Branding = "SharpBrowser";
		public static string AcceptLanguage = "en-US,en;q=0.9";
		public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.5195.102 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion; // UserAgent to fix issue with Google account authentication		
		public static string HomepageURL = "https://www.google.com";
		public static string NewTabURL = "about:blank";
		public static string InternalURL = "sharpbrowser";
		public static string DownloadsURL = "sharpbrowser://storage/downloads.html";
		public static string FileNotFoundURL = "sharpbrowser://storage/errors/notFound.html";
		public static string CannotConnectURL = "sharpbrowser://storage/errors/cannotConnect.html";
		public static string SearchURL = "https://www.google.com/search?q=";

		public static bool WebSecurity = true;
		public static bool CrossDomainSecurity = true;
		public static bool WebGL = true;
		public static bool ApplicationCache = true;



	}
}
