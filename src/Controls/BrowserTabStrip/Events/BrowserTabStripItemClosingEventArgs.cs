using System;

namespace SharpBrowser.Controls.BrowserTabStrip {
	public class TabStripItemClosingEventArgs : EventArgs {
		private bool _cancel;

		private BrowserTabPage _item;

		public BrowserTabPage Item {
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

		public TabStripItemClosingEventArgs(BrowserTabPage item) {
			_item = item;
		}
	}
}