using System;

namespace SharpBrowser.Controls.BrowserTabStrip {
	public class TabStripItemClosingEventArgs : EventArgs {
		private bool _cancel;

		private BrowserTabItem _item;

		public BrowserTabItem Item {
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

		public TabStripItemClosingEventArgs(BrowserTabItem item) {
			_item = item;
		}
	}
}