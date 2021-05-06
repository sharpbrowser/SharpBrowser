using System;

namespace SharpBrowser.BrowserTabStrip {
	public class TabStripItemClosingEventArgs : EventArgs {
		private bool _cancel;

		private BrowserTabStripItem _item;

		public BrowserTabStripItem Item {
			get {
				return _item;
			}
			set {
				_item = value;
			}
		}

		public bool Cancel {
			get {
				return _cancel;
			}
			set {
				_cancel = value;
			}
		}

		public TabStripItemClosingEventArgs(BrowserTabStripItem item) {
			_item = item;
		}
	}
}