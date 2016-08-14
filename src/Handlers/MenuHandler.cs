using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CefSharp;
using System.Windows.Forms;
using CefSharp.WinForms;

namespace SharpBrowser
{
    internal class MenuHandler : IContextMenuHandler
    {
        private const int ShowDevTools = 26501;
        private const int CloseDevTools = 26502;
        private const int SaveImageAs = 26503;
        private const int SaveAsPdf = 26504;
        private const int SaveLinkAs = 26505;
        private const int CopyLinkAddress = 26506;
        private const int OpenLinkInNewTab = 26507;
        MainForm myForm;

        public MenuHandler(MainForm form)
        {
            myForm = form;
        }

        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            //To disable the menu then call clear
            model.Clear();

            //Removing existing menu item
            //bool removed = model.Remove(CefMenuCommand.ViewSource); // Remove "View Source" option
            if (parameters.LinkUrl != "")
            {
                model.AddItem((CefMenuCommand)OpenLinkInNewTab, "Open link in new tab");
                model.AddItem((CefMenuCommand)SaveLinkAs, "Save link as");
                model.AddItem((CefMenuCommand)CopyLinkAddress, "Copy link address");
                model.AddSeparator();
            }

            if (parameters.HasImageContents && parameters.SourceUrl != "")
            {
				model.AddItem((CefMenuCommand)SaveImageAs, "Save image as");
				model.AddSeparator();
            }

           // model.AddItem((CefMenuCommand)SaveAsPdf, "Save as Pdf");

            //Add new custom menu items
            model.AddItem((CefMenuCommand)ShowDevTools, "Developer tools");
            model.AddItem(CefMenuCommand.ViewSource, "View source");
        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if ((int)commandId == ShowDevTools)
            {
                browser.ShowDevTools();
            }
            if ((int)commandId == CloseDevTools)
            {
                browser.CloseDevTools();
            }
            if ((int)commandId == SaveImageAs)
            {
                browser.GetHost().StartDownload(parameters.SourceUrl);
            }
            if ((int)commandId == SaveLinkAs)
            {
                browser.GetHost().StartDownload(parameters.LinkUrl);
            }
            if ((int)commandId == OpenLinkInNewTab)
            {
                ChromiumWebBrowser newBrowser = myForm.AddNewBrowserTab(parameters.LinkUrl, false);
            }
            if ((int)commandId == CopyLinkAddress)
            {
                Clipboard.SetText(parameters.LinkUrl);
            }
            
           /* if ((int)commandId == SaveAsPdf)
            {
                PdfPrintSettings settings = new PdfPrintSettings();
                settings.Landscape = true;
                settings.BackgroundsEnabled = false;
                browser.PrintToPdfAsync(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SharpBrowser.pdf", settings);
            }*/

            return false;
        }

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        /*bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            return false;
        }*/
    }
}
