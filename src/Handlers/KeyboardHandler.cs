	using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CefSharp;
using SharpBrowser.Browser.Model;

namespace SharpBrowser {
	internal class KeyboardHandler : IKeyboardHandler {
		MainForm myForm;

		public static List<BrowserHotKey> Hotkeys = new List<BrowserHotKey>();
		public static void AddHotKey(Form form, Action function, Keys key, bool ctrl = false, bool shift = false, bool alt = false) {
			Utils.AddHotKey(form, function, key, ctrl, shift, alt);
			Hotkeys.Add(new BrowserHotKey(function, key, ctrl, shift, alt));
		}

		public KeyboardHandler(MainForm form) {
			myForm = form;
		}

		// Summary:
		//     Called before a keyboard event is sent to the renderer. Return true if the event
		//     was handled or false otherwise. If the event will be handled in CefSharp.IKeyboardHandler.OnKeyEvent(CefSharp.IWebBrowser,CefSharp.IBrowser,CefSharp.KeyType,System.Int32,System.Int32,CefSharp.CefEventFlags,System.Boolean)
		//     as a keyboard shortcut set isKeyboardShortcut to true and return false.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     The browser instance.
		//
		//   type:
		//     Whether this was a key up/down/raw/etc...
		//
		//   windowsKeyCode:
		//     The Windows key code for the key event. This value is used by the DOM specification.
		//     Sometimes it comes directly from the event (i.e. on Windows) and sometimes it's
		//     determined using a mapping function. See WebCore/platform/chromium/KeyboardCodes.h
		//     for the list of values.
		//
		//   nativeKeyCode:
		//     The native key code. On Windows this appears to be in the format of WM_KEYDOWN/WM_KEYUP/etc...
		//     lParam data.
		//
		//   modifiers:
		//     What other modifier keys are currently down: Shift/Control/Alt/OS X Command/etc...
		//
		//   isSystemKey:
		//     Indicates whether the event is considered a "system key" event (see http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx
		//     for details).
		//
		//   isKeyboardShortcut:
		//     See the summary for an explanation of when to set this to true.
		//
		// Returns:
		//     Returns true if the event was handled or false otherwise.
		public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut) {
			return false;
		}

		//
		// Summary:
		//     Called after the renderer and JavaScript in the page has had a chance to handle
		//     the event. Return true if the keyboard event was handled or false otherwise.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     The browser instance.
		//
		//   type:
		//     Whether this was a key up/down/raw/etc...
		//
		//   windowsKeyCode:
		//     The Windows key code for the key event. This value is used by the DOM specification.
		//     Sometimes it comes directly from the event (i.e. on Windows) and sometimes it's
		//     determined using a mapping function. See WebCore/platform/chromium/KeyboardCodes.h
		//     for the list of values.
		//
		//   nativeKeyCode:
		//     The native key code. On Windows this appears to be in the format of WM_KEYDOWN/WM_KEYUP/etc...
		//     lParam data.
		//
		//   modifiers:
		//     What other modifier keys are currently down: Shift/Control/Alt/OS X Command/etc...
		//
		//   isSystemKey:
		//     Indicates whether the event is considered a "system key" event (see http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx
		//     for details).
		//
		// Returns:
		//     Return true if the keyboard event was handled or false otherwise.
		public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {

			if (type == KeyType.RawKeyDown) {


				// check if my hotkey
				int mod = ((int)modifiers);
				bool ctrlDown = mod.IsBitmaskOn((int)CefEventFlags.ControlDown);
				bool shiftDown = mod.IsBitmaskOn((int)CefEventFlags.ShiftDown);
				bool altDown = mod.IsBitmaskOn((int)CefEventFlags.AltDown);

				// per registered hotkey
				foreach (BrowserHotKey key in Hotkeys) {
					if (key.KeyCode == windowsKeyCode) {
						if (key.Ctrl == ctrlDown && key.Shift == shiftDown && key.Alt == altDown) {
							myForm.InvokeOnParent(delegate () {
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