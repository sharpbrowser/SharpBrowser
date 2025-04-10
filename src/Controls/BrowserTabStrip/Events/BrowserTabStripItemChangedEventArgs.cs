using System;

namespace SharpBrowser.Controls.BrowserTabStrip {
	public class TabStripItemChangedEventArgs : EventArgs {
		private BrowserTabItem itm;

		private BrowserTabStripItemChangeTypes changeType;

		public BrowserTabStripItemChangeTypes ChangeType => changeType;

		public BrowserTabItem Item => itm;

		public TabStripItemChangedEventArgs(BrowserTabItem item, BrowserTabStripItemChangeTypes type) {
			changeType = type;
			itm = item;
		}
	}
}