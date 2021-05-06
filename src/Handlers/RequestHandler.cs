using System;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using CefSharp;

namespace SharpBrowser {
	internal class RequestHandler : IRequestHandler {
		MainForm myForm;

		public RequestHandler(MainForm form) {
			myForm = form;
		}

		//
		// Summary:
		//     Called on the UI thread before OnBeforeBrowse in certain limited cases where
		//     navigating a new or different browser might be desirable. This includes user-initiated
		//     navigation that might open in a special way (e.g. links clicked via middle-click
		//     or ctrl + left-click) and certain types of cross-origin navigation initiated
		//     from the renderer process (e.g. navigating the top-level frame to/from a file
		//     URL).
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   frame:
		//     The frame object
		//
		//   targetUrl:
		//     target url
		//
		//   targetDisposition:
		//     The value indicates where the user intended to navigate the browser based on
		//     standard Chromium behaviors (e.g. current tab, new tab, etc).
		//
		//   userGesture:
		//     The value will be true if the browser navigated via explicit user gesture (e.g.
		//     clicking a link) or false if it navigated automatically (e.g. via the DomContentLoaded
		//     event).
		//
		// Returns:
		//     Return true to cancel the navigation or false to allow the navigation to proceed
		//     in the source browser's top-level frame.


		//
		// Summary:
		//     Called when the browser needs credentials from the user.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   originUrl:
		//     is the origin making this authentication request
		//
		//   isProxy:
		//     indicates whether the host is a proxy server
		//
		//   host:
		//     hostname
		//
		//   port:
		//     port number
		//
		//   realm:
		//     realm
		//
		//   scheme:
		//     scheme
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of authentication requests.
		//
		// Returns:
		//     Return true to continue the request and call CefSharp.IAuthCallback.Continue(System.String,System.String)
		//     when the authentication information is available. Return false to cancel the
		//     request.
		public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) {
			// Return false to cancel the request.
			return false;
		}

		//
		// Summary:
		//     Called before browser navigation.  If the navigation is allowed CefSharp.IWebBrowser.FrameLoadStart
		//     and CefSharp.IWebBrowser.FrameLoadEnd will be called. If the navigation is
		//     canceled CefSharp.IWebBrowser.LoadError will be called with an ErrorCode
		//     value of CefSharp.CefErrorCode.Aborted.
		//
		// Parameters:
		//   frame:
		//     The frame the request is coming from
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		//   isRedirect:
		//     has the request been redirected
		//
		// Returns:
		//     Return true to cancel the navigation or false to allow the navigation to
		//     proceed.
		public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect) {
			return false;
		}
		

		//
		// Summary:
		//     Called to handle requests for URLs with an invalid SSL certificate.  Return
		//     true and call CefSharp.IRequestCallback.Continue(System.Boolean) either in
		//     this method or at a later time to continue or cancel the request. If CefSettings.IgnoreCertificateErrors
		//     is set all invalid certificates will be accepted without calling this method.
		//
		// Parameters:
		//   errorCode:
		//     the error code for this invalid certificate
		//
		//   requestUrl:
		//     the url of the request for the invalid certificate
		//
		//   sslInfo:
		//     ssl certificate information
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of url requests.  If
		//     empty the error cannot be recovered from and the request will be canceled
		//     automatically.
		//
		// Returns:
		//     Return false to cancel the request immediately. Return true and use CefSharp.IRequestCallback
		//     to execute in an async fashion.
		public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback) {
			return true;
		}
		//
		// Summary:
		//     Called on the UI thread before OnBeforeBrowse in certain limited cases where
		//     navigating a new or different browser might be desirable. This includes user-initiated
		//     navigation that might open in a special way (e.g.  links clicked via middle-click
		//     or ctrl + left-click) and certain types of cross-origin navigation initiated
		//     from the renderer process (e.g.  navigating the top-level frame to/from a
		//     file URL).
		//
		// Parameters:
		//   frame:
		//     The frame object
		//
		//   targetDisposition:
		//     The value indicates where the user intended to navigate the browser based
		//     on standard Chromium behaviors (e.g. current tab, new tab, etc).
		//
		//   userGesture:
		//     The value will be true if the browser navigated via explicit user gesture
		//     (e.g. clicking a link) or false if it navigated automatically (e.g. via the
		//     DomContentLoaded event).
		//
		// Returns:
		//     Return true to cancel the navigation or false to allow the navigation to
		//     proceed in the source browser's top-level frame.
		public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture) {
			return false;
		}
		//
		// Summary:
		//     Called when a plugin has crashed
		//
		// Parameters:
		//   pluginPath:
		//     path of the plugin that crashed
		public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath) {
		}

		//
		// Summary:
		//     Called when JavaScript requests a specific storage quota size via the webkitStorageInfo.requestQuota
		//     function.  For async processing return true and execute CefSharp.IRequestCallback.Continue(System.Boolean)
		//     at a later time to grant or deny the request or CefSharp.IRequestCallback.Cancel()
		//     to cancel.
		//
		// Parameters:
		//   originUrl:
		//     the origin of the page making the request
		//
		//   newSize:
		//     is the requested quota size in bytes
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of url requests.
		//
		// Returns:
		//     Return false to cancel the request immediately. Return true to continue the
		//     request and call CefSharp.IRequestCallback.Continue(System.Boolean) either
		//     in this method or at a later time to grant or deny the request.
		public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback) {
			callback.Continue(true);
			return true;
		}
		//
		// Summary:
		//     Called when the render process terminates unexpectedly.
		//
		// Parameters:
		//   status:
		//     indicates how the process terminated.
		public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status) {
		}
		//
		// Summary:
		//     Called on the CEF UI thread when the render view associated with browser
		//     is ready to receive/handle IPC messages in the render process.
		public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser) {
		}

		//
		// Summary:
		//     Called on the CEF IO thread before a resource request is initiated.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     represent the source browser of the request
		//
		//   frame:
		//     represent the source frame of the request
		//
		//   request:
		//     represents the request contents and cannot be modified in this callback
		//
		//   isNavigation:
		//     will be true if the resource request is a navigation
		//
		//   isDownload:
		//     will be true if the resource request is a download
		//
		//   requestInitiator:
		//     is the origin (scheme + domain) of the page that initiated the request
		//
		//   disableDefaultHandling:
		//     to true to disable default handling of the request, in which case it will need
		//     to be handled via CefSharp.IResourceRequestHandler.GetResourceHandler(CefSharp.IWebBrowser,CefSharp.IBrowser,CefSharp.IFrame,CefSharp.IRequest)
		//     or it will be canceled
		//
		// Returns:
		//     To allow the resource load to proceed with default handling return null. To specify
		//     a handler for the resource return a CefSharp.IResourceRequestHandler object.
		//     If this callback returns null the same method will be called on the associated
		//     CefSharp.IRequestContextHandler, if any
		public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling) {
			var rh = new ResourceRequestHandler(myForm);
			return rh;
		}
		

		//
		// Summary:
		//     Called when the browser needs user to select Client Certificate for authentication
		//     requests (eg. PKI authentication).
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		//
		//   isProxy:
		//     indicates whether the host is a proxy server
		//
		//   host:
		//     hostname
		//
		//   port:
		//     port number
		//
		//   certificates:
		//     List of Client certificates for selection
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of client certificate selection
		//     for authentication requests.
		//
		// Returns:
		//     Return true to continue the request and call ISelectClientCertificateCallback.Select()
		//     with the selected certificate for authentication. Return false to use the default
		//     behavior where the browser selects the first certificate from the list.
		public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback) {
			return false;
		}


		//
		// Summary:
		//     Called on the CEF UI thread when the window.document object of the main frame
		//     has been created.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     the ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object
		public void OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser) {

		}



	}
}