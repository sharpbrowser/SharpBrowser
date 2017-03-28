using System.Collections.Specialized;
using CefSharp;

namespace SharpBrowser {
	internal class RequestHandler : IRequestHandler {
		MainForm myForm;

		public RequestHandler(MainForm form) {
			myForm = form;
		}

		// Summary:
		//     Called when the browser needs credentials from the user.
		//
		// Parameters:
		//   frame:
		//     The frame object that needs credentials (This will contain the URL that is
		//     being requested.)
		//
		//   isProxy:
		//     indicates whether the host is a proxy server
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of authentication requests.
		//
		// Returns:
		//     Return true to continue the request and call CefAuthCallback::Continue()
		//     when the authentication information is available. Return false to cancel
		//     the request.
		public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) {
			
			return false;
		}
		//
		// Summary:
		//     Called on the CEF IO thread to optionally filter resource response content.
		//
		// Parameters:
		//   frame:
		//     The frame that is being redirected.
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		//   response:
		//     the response object - cannot be modified in this callback
		//
		// Returns:
		//     Return an IResponseFilter to intercept this response, otherwise return null
		public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response) {
			return null;
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
		public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect) {
			return false;
		}
		//
		// Summary:
		//     Called before a resource request is loaded. For async processing return CefSharp.CefReturnValue.ContinueAsync
		//     and execute CefSharp.IRequestCallback.Continue(System.Boolean) or CefSharp.IRequestCallback.Cancel()
		//
		// Parameters:
		//   frame:
		//     The frame object
		//
		//   request:
		//     the request object - can be modified in this callback.
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of url requests.
		//
		// Returns:
		//     To cancel loading of the resource return CefSharp.CefReturnValue.Cancel or
		//     CefSharp.CefReturnValue.Continue to allow the resource to load normally.
		//     For async return CefSharp.CefReturnValue.ContinueAsync
		public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback) {

			// if referer given
			var tab = myForm.GetTabByBrowser(browserControl);
			if (tab != null && tab.RefererURL != null) {

				// Set referer
				request.SetReferrer(tab.RefererURL, ReferrerPolicy.Always);

			}

			return CefSharp.CefReturnValue.Continue;
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
		//     Called on the UI thread to handle requests for URLs with an unknown protocol
		//     component. SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS
		//     BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
		//
		// Parameters:
		//   url:
		//     the request url
		//
		// Returns:
		//     return to true to attempt execution via the registered OS protocol handler,
		//     if any. Otherwise return false.
		public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url) {
			return true;
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
		//     Called on the CEF IO thread when a resource load has completed.
		//
		// Parameters:
		//   frame:
		//     The frame that is being redirected.
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		//   response:
		//     the response object - cannot be modified in this callback
		//
		//   status:
		//     indicates the load completion status
		//
		//   receivedContentLength:
		//     is the number of response bytes actually read.
		public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength) {
		}
		//
		// Summary:
		//     Called on the IO thread when a resource load is redirected. The CefSharp.IRequest.Url
		//     parameter will contain the old URL and other request-related information.
		//
		// Parameters:
		//   frame:
		//     The frame that is being redirected.
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		//   newUrl:
		//     the new URL and can be changed if desired
		public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl) {
		}
		//
		// Summary:
		//     Called on the CEF IO thread when a resource response is received.  To allow
		//     the resource to load normally return false.  To redirect or retry the resource
		//     modify request (url, headers or post body) and return true.  The response
		//     object cannot be modified in this callback.
		//
		// Parameters:
		//   frame:
		//     The frame that is being redirected.
		//
		//   request:
		//     the request object
		//
		//   response:
		//     the response object - cannot be modified in this callback
		//
		// Returns:
		//     To allow the resource to load normally return false.  To redirect or retry
		//     the resource modify request (url, headers or post body) and return true.
		public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response) {


			int code = response.StatusCode;


			// if NOT FOUND
			if (code == 404) {

				if (!request.Url.IsURLLocalhost()) {

					// redirect to web archive to try and find older version
					request.Url = "http://web.archive.org/web/*/" + request.Url;

				} else {

					// show offline "file not found" page
					request.Url = MainForm.FileNotFoundURL + "?path=" + request.Url.EncodeURL();
				}

				return true;
			}


			// if FILE NOT FOUND
			if (code == 0 && request.Url.IsURLOfflineFile()) {
				string path = request.Url.FileURLToPath();
				if (path.FileNotExists()) {

					// show offline "file not found" page
					request.Url = MainForm.FileNotFoundURL + "?path=" + path.EncodeURL();
					return true;

				}
			} else {

				// if CANNOT CONNECT
				if (code == 0 || code == 444 || (code >= 500 && code <= 599)) {

					// show offline "cannot connect to server" page
					request.Url = MainForm.CannotConnectURL;
					return true;
				}

			}
			
			return false;
		}

	}
}
