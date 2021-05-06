using System;

namespace SharpBrowser.BrowserTabStrip {
	public class TabStripItemChangedEventArgs : EventArgs {
		private BrowserTabStripItem itm;

		private BrowserTabStripItemChangeTypes changeType;

		public BrowserTabStripItemChangeTypes ChangeType => changeType;

		public BrowserTabStripItem Item => itm;

		public TabStripItemChangedEventArgs(BrowserTabStripItem item, BrowserTabStripItemChangeTypes type) {
			changeType = type;
			itm = item;
		}
	}
}