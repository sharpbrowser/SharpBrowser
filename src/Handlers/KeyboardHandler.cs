	using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CefSharp;


namespace SharpBrowser {
    internal class KeyboardHandler : IKeyboardHandler
    {
        MainForm myForm;

		public static List<SharpHotKey> Hotkeys = new List<SharpHotKey>();
		public static void AddHotKey(Form form, Action function, Keys key, bool ctrl = false, bool shift = false, bool alt = false) {
			Utils.AddHotKey(form, function, key, ctrl, shift, alt);
			Hotkeys.Add(new SharpHotKey(function, key, ctrl, shift, alt));
		}

        public KeyboardHandler(MainForm form)
        {
            myForm = form;
        }
        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }

        /// <inheritdoc/>
		public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {
			
			if (type == KeyType.RawKeyDown) {


				// check if my hotkey
				int mod = ((int)modifiers);
				bool ctrlDown = mod.IsBitmaskOn((int)CefEventFlags.ControlDown);
				bool shiftDown = mod.IsBitmaskOn((int)CefEventFlags.ShiftDown);
				bool altDown = mod.IsBitmaskOn((int)CefEventFlags.AltDown);

				// per registered hotkey
				foreach (SharpHotKey key in Hotkeys) {
					if (key.KeyCode == windowsKeyCode){
						if (key.Ctrl == ctrlDown && key.Shift == shiftDown && key.Alt == altDown) {
							myForm.InvokeOnParent(delegate() {
								key.Callback();
							});
						}
					}
				}

				//Debug.WriteLine(String.Format("OnKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
				
			}

			return false;
		}
    }
}
