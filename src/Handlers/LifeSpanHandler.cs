using CefSharp;

namespace SharpBrowser {
    internal class LifeSpanHandler : ILifeSpanHandler
    {
        MainForm myForm;

        public LifeSpanHandler(MainForm form)
        {
            myForm = form;
        }


		// Summary:
		//     Called when a browser has recieved a request to close. This may result directly
		//     from a call to CefBrowserHost::CloseBrowser() or indirectly if the browser
		//     is a top-level OS window created by CEF and the user attempts to close the
		//     window. This method will be called after the JavaScript 'onunload' event
		//     has been fired. It will not be called for browsers after the associated OS
		//     window has been destroyed (for those browsers it is no longer possible to
		//     cancel the close).  If CEF created an OS window for the browser returning
		//     false will send an OS close notification to the browser window's top-level
		//     owner (e.g. WM_CLOSE on Windows, performClose: on OS-X and "delete_event"
		//     on Linux). If no OS window exists (window rendering disabled) returning false
		//     will cause the browser object to be destroyed immediately. Return true if
		//     the browser is parented to another window and that other window needs to
		//     receive close notification via some non-standard technique.  If an application
		//     provides its own top-level window it should handle OS close notifications
		//     by calling CefBrowserHost::CloseBrowser(false) instead of immediately closing
		//     (see the example below). This gives CEF an opportunity to process the 'onbeforeunload'
		//     event and optionally cancel the close before DoClose() is called.  The CefLifeSpanHandler::OnBeforeClose()
		//     method will be called immediately before the browser object is destroyed.
		//     The application should only exit after OnBeforeClose() has been called for
		//     all existing browsers.  If the browser represents a modal window and a custom
		//     modal loop implementation was provided in CefLifeSpanHandler::RunModal()
		//     this callback should be used to restore the opener window to a usable state.
		//      By way of example consider what should happen during window close when the
		//     browser is parented to an application-provided top-level OS window.  1. User
		//     clicks the window close button which sends an OS close notification (e.g.
		//     WM_CLOSE on Windows, performClose: on OS-X and "delete_event" on Linux).
		//      2. Application's top-level window receives the close notification and: A.
		//     Calls CefBrowserHost::CloseBrowser(false).  B. Cancels the window close.
		//      3. JavaScript 'onbeforeunload' handler executes and shows the close confirmation
		//     dialog (which can be overridden via CefJSDialogHandler::OnBeforeUnloadDialog()).
		//      4. User approves the close.  5. JavaScript 'onunload' handler executes.
		//      6. Application's DoClose() handler is called. Application will: A. Set a
		//     flag to indicate that the next close attempt will be allowed.  B. Return
		//     false.  7. CEF sends an OS close notification.  8. Application's top-level
		//     window receives the OS close notification and allows the window to close
		//     based on the flag from #6B.  9. Browser OS window is destroyed.  10. Application's
		//     CefLifeSpanHandler::OnBeforeClose() handler is called and the browser object
		//     is destroyed.  11. Application exits by calling CefQuitMessageLoop() if no
		//     other browsers exist.
		//
		// Parameters:
		//   browserControl:
		//     The CefSharp.IWebBrowser control that is realted to the window is closing.
		//
		//   browser:
		//     The browser instance
		//
		// Returns:
		//     For default behaviour return false
		public bool DoClose(IWebBrowser browserControl, IBrowser browser) {
			return false;
		}
		//
		// Summary:
		//     Called after a new browser is created.
		//
		// Parameters:
		//   browserControl:
		//     The CefSharp.IWebBrowser control that is realted to the window is closing.
		//
		//   browser:
		//     The browser instance
		public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser) {
		}
		//
		// Summary:
		//     Called before a CefBrowser window (either the main browser for CefSharp.IWebBrowser,
		//     or one of its children)
		//
		// Parameters:
		//   browserControl:
		//     The CefSharp.IWebBrowser control that is realted to the window is closing.
		//
		//   browser:
		//     The browser instance
		public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser) {
		}
		//
		// Summary:
		//     Called before a popup window is created.
		//
		// Parameters:
		//   browserControl:
		//     The CefSharp.IWebBrowser control this request is for.
		//
		//   browser:
		//     The browser instance that launched this popup.
		//
		//   frame:
		//     The HTML frame that launched this popup.
		//
		//   targetUrl:
		//     The URL of the popup content. (This may be empty/null)
		//
		//   targetFrameName:
		//     The name of the popup. (This may be empty/null)
		//
		//   targetDisposition:
		//     The value indicates where the user intended to open the popup (e.g. current
		//     tab, new tab, etc)
		//
		//   userGesture:
		//     The value will be true if the popup was opened via explicit user gesture
		//     (e.g. clicking a link) or false if the popup opened automatically (e.g. via
		//     the DomContentLoaded event).
		//
		//   popupFeatures:
		//     structure contains additional information about the requested popup window
		//
		//   windowInfo:
		//     window information
		//
		//   browserSettings:
		//     browser settings, defaults to source browsers
		//
		//   noJavascriptAccess:
		//     value indicates whether the new browser window should be scriptable and in
		//     the same process as the source browser.
		//
		//   newBrowser:
		//     EXPERIMENTAL - A newly created browser that will host the popup
		//
		// Returns:
		//     To cancel creation of the popup window return true otherwise return false.
		//
		// Remarks:
		//     CEF documentation: Called on the IO thread before a new popup window is created.
		//     The |browser| and |frame| parameters represent the source of the popup request.
		//     The |target_url| and |target_frame_name| values may be empty if none were
		//     specified with the request. The |popupFeatures| structure contains information
		//     about the requested popup window. To allow creation of the popup window optionally
		//     modify |windowInfo|, |client|, |settings| and |no_javascript_access| and
		//     return false. To cancel creation of the popup window return true. The |client|
		//     and |settings| values will default to the source browser's values. The |no_javascript_access|
		//     value indicates whether the new browser window should be scriptable and in
		//     the same process as the source browser.
		public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser) {

			// open popup in new tab!
			newBrowser = myForm.AddNewBrowserTab(targetUrl);

			return true;

		}
    }
}
