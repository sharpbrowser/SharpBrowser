using SharpBrowser.Handlers;
using System.Windows.Forms;

namespace SharpBrowser.Managers {
	internal static class HotkeyManager {

		/// <summary>
		/// these hotkeys work when the user is focussed on the .NET form and its controls,
		/// AND when the user is focussed on the browser (CefSharp portion)
		/// </summary>
		public static void Init(MainForm form) {

			// browser hotkeys
			KeyboardHandler.AddHotKey(form, form.CloseActiveTab, Keys.W, true);
			KeyboardHandler.AddHotKey(form, form.CloseActiveTab, Keys.Escape, true);
			KeyboardHandler.AddHotKey(form, form.AddBlankTab, Keys.T, true);
			KeyboardHandler.AddHotKey(form, form.RefreshActiveTab, Keys.F5);
			KeyboardHandler.AddHotKey(form, form.OpenDeveloperTools, Keys.F12);
			KeyboardHandler.AddHotKey(form, form.NextTab, Keys.Tab, true);
			KeyboardHandler.AddHotKey(form, form.PrevTab, Keys.Tab, true, true);
			KeyboardHandler.AddHotKey(form, form.Print, Keys.P, true);
			KeyboardHandler.AddHotKey(form, form.PrintToPDF, Keys.P, true, true);

			// search hotkeys
			KeyboardHandler.AddHotKey(form, form.OpenSearch, Keys.F, true);
			KeyboardHandler.AddHotKey(form, form.CloseSearch, Keys.Escape);
			KeyboardHandler.AddHotKey(form, form.StopActiveTab, Keys.Escape);
			KeyboardHandler.AddHotKey(form, form.ToggleFullscreen, Keys.F11);


		}

	}
}
