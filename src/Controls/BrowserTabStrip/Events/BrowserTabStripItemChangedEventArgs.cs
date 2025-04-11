using System;

namespace SharpBrowser.Controls.BrowserTabStrip {
	public class TabStripItemChangedEventArgs : EventArgs {
		private BrowserTabPage itm;

		private BrowserTabStripItemChangeTypes changeType;

		public BrowserTabStripItemChangeTypes ChangeType => changeType;

		public BrowserTabPage Item => itm;

		public TabStripItemChangedEventArgs(BrowserTabPage item, BrowserTabStripItemChangeTypes type) {
			changeType = type;
			itm = item;
		}
	}
}