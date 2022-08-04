using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBrowser.Browser.Model {
	/// <summary>
	/// POCO for holding hotkey data
	/// </summary>
	internal class BrowserHotKey {

		public Keys Key;
		public int KeyCode;
		public bool Ctrl;
		public bool Shift;
		public bool Alt;

		public Action Callback;

		public BrowserHotKey(Action callback, Keys key, bool ctrl = false, bool shift = false, bool alt = false) {
			Callback = callback;
			Key = key;
			KeyCode = (int)key;
			Ctrl = ctrl;
			Shift = shift;
			Alt = alt;
		}

	}
}
