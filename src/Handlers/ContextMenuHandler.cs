using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CefSharp;
using System.Windows.Forms;
using CefSharp.WinForms;

namespace SharpBrowser.Handlers {
	internal class ContextMenuHandler : IContextMenuHandler {

		private const int ShowDevTools = 26501;
		private const int CloseDevTools = 26502;
		private const int SaveImageAs = 26503;
		private const int SaveAsPdf = 26504;
		private const int SaveLinkAs = 26505;
		private const int CopyLinkAddress = 26506;
		private const int OpenLinkInNewTab = 26507;
		private const int CloseTab = 40007;
		private const int RefreshTab = 40008;
		private const int Print = 26508;
		readonly MainForm myForm;

		private string lastSelText = "";

		public ContextMenuHandler(MainForm form) {
			myForm = form;
		}

		//
		// Summary:
		//     Called before a context menu is displayed. The model can be cleared to show no
		//     context menu or modified to show a custom menu.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   frame:
		//     The frame the request is coming from
		//
		//   parameters:
		//     provides information about the context menu state
		//
		//   model:
		//     initially contains the default context menu
		public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model) {

			// clear the menu
			model.Clear();

			// save text
			lastSelText = parameters.SelectionText;

			// to copy text
			if (parameters.SelectionText.CheckIfValid()) {
				model.AddItem(CefMenuCommand.Copy, "Copy");
				model.AddSeparator();
			}

			//Removing existing menu item
			//bool removed = model.Remove(CefMenuCommand.ViewSource); // Remove "View Source" option
			if (parameters.LinkUrl != "") {
				model.AddItem((CefMenuCommand)OpenLinkInNewTab, "Open link in new tab");
				model.AddItem((CefMenuCommand)CopyLinkAddress, "Copy link");
				model.AddSeparator();
			}

			if (parameters.HasImageContents && parameters.SourceUrl.CheckIfValid()) {

				// RIGHT CLICKED ON IMAGE

			}

			if (parameters.SelectionText != null) {

				// TEXT IS SELECTED

			}

			//Add new custom menu items
			//#if DEBUG
			model.AddItem((CefMenuCommand)ShowDevTools, "Developer tools");
			model.AddItem(CefMenuCommand.ViewSource, "View source");
			model.AddSeparator();
			//#endif

			model.AddItem((CefMenuCommand)RefreshTab, "Refresh tab");
			model.AddItem((CefMenuCommand)CloseTab, "Close tab");
			model.AddSeparator();

			model.AddItem((CefMenuCommand)SaveAsPdf, "Save as PDF");
			model.AddItem((CefMenuCommand)Print, "Print Page");

		}

		//
		// Summary:
		//     Called to execute a command selected from the context menu. See cef_menu_id_t
		//     for the command ids that have default implementations. All user-defined command
		//     ids should be between MENU_ID_USER_FIRST and MENU_ID_USER_LAST.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   frame:
		//     The frame the request is coming from
		//
		//   parameters:
		//     will have the same values as what was passed to
		//
		//   commandId:
		//     menu command id
		//
		//   eventFlags:
		//     event flags
		//
		// Returns:
		//     Return true if the command was handled or false for the default implementation.
		public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags) {

			int id = (int)commandId;

			if (id == ShowDevTools) {
				browser.ShowDevTools();
			}
			if (id == CloseDevTools) {
				browser.CloseDevTools();
			}
			if (id == SaveImageAs) {
				browser.GetHost().StartDownload(parameters.SourceUrl);
			}
			if (id == SaveLinkAs) {
				browser.GetHost().StartDownload(parameters.LinkUrl);
			}
			if (id == OpenLinkInNewTab) {
				ChromiumWebBrowser newBrowser = myForm.AddNewBrowserTab(parameters.LinkUrl, false, browser.MainFrame.Url);
			}
			if (id == CopyLinkAddress) {
				Clipboard.SetText(parameters.LinkUrl);
			}
			if (id == CloseTab) {
				myForm.InvokeOnParent(delegate () {
					myForm.CloseActiveTab();
				});
			}
			if (id == RefreshTab) {
				myForm.InvokeOnParent(delegate () {
					myForm.RefreshActiveTab();
				});
			}
			if (id == SaveAsPdf) {
				SaveAsPDF(browser);
			}
			if (id == Print)
			{
				browser.Print();
			}

			return false;
		}

		public static void SaveAsPDF(IBrowser browser) {
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "PDF Files|*.pdf";
			//sfd.DefaultExt = (browser.MainFrame.Name ?? "Document") + ".pdf";
			if (sfd.ShowDialog() == DialogResult.OK) {
				browser.PrintToPdfAsync(sfd.FileName, new PdfPrintSettings() {
					PrintBackground = true,
				});
			}
		}

		//
		// Summary:
		//     Called when the context menu is dismissed irregardless of whether the menu was
		//     empty or a command was selected.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   frame:
		//     The frame the request is coming from
		public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame) {

		}

		//
		// Summary:
		//     Called to allow custom display of the context menu. For custom display return
		//     true and execute callback either synchronously or asynchronously with the selected
		//     command Id. For default display return false. Do not keep references to parameters
		//     or model outside of this callback.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   frame:
		//     The frame the request is coming from
		//
		//   parameters:
		//     provides information about the context menu state
		//
		//   model:
		//     contains the context menu model resulting from OnBeforeContextMenu
		//
		//   callback:
		//     the callback to execute for custom display
		//
		// Returns:
		//     For custom display return true and execute callback either synchronously or asynchronously
		//     with the selected command ID.
		public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback) {

			// show default menu
			return false;
		}
	}
}