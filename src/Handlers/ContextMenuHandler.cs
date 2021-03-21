using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CefSharp;
using System.Windows.Forms;
using CefSharp.WinForms;

namespace SharpBrowser {
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
			if (id == SaveAsPdf)
			{

				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = "PDF Files | *.pdf";
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					//string path = Path.GetFileName(sfd.FileName);
					browser.PrintToPdfAsync(sfd.FileName, new PdfPrintSettings()
					{
						SelectionOnly = false,
						BackgroundsEnabled = true
					});
				}
			}
			if (id == Print)
			{
				browser.Print();
			}

			return false;
		}

		public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame) {

		}

		public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback) {

			// show default menu
			return false;
		}
	}
}