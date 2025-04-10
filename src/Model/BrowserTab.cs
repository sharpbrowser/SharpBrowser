using CefSharp.WinForms;
using SharpBrowser.Controls.BrowserTabStrip;
using System;

namespace SharpBrowser.Model {
	/// <summary>
	/// POCO created for holding data per tab
	/// </summary>
	internal class BrowserTab {

		public bool IsOpen;

		public string OrigURL;
		public string CurURL;
		public string Title;

		public string RefererURL;

		public DateTime DateCreated;

		public BrowserTabItem Tab;
		public ChromiumWebBrowser Browser;

	}
}
