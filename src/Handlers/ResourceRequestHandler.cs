
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CefSharp;
using System.Windows.Forms;
using System.Drawing;
using CefSharp.Callback;

namespace SharpBrowser {
	internal class ResourceRequestHandler : IResourceRequestHandler {
		MainForm myForm;
		public ResourceRequestHandler(MainForm form) {
			myForm = form;
		}

		//
		// Summary:
		//     Called on the CEF IO thread before a resource request is loaded. .
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object - can be modified in this callback.
		//
		// Returns:
		//     To optionally filter cookies for the request return a ICookieAccessFilter instance
		//     otherwise return null.
		public ICookieAccessFilter GetCookieAccessFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request) {
			return null;
		}
		//
		// Summary:
		//     Called on the CEF IO thread before a resource is loaded. To specify a handler
		//     for the resource return a CefSharp.IResourceHandler object
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The browser UI control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		// Returns:
		//     To allow the resource to load using the default network loader return null otherwise
		//     return an instance of CefSharp.IResourceHandler with a valid stream
		public IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request) {
			
			// allow the resource to load using the default network loader
			return null;
		}
		//
		// Summary:
		//     Called on the CEF IO thread to optionally filter resource response content.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		//   response:
		//     the response object - cannot be modified in this callback
		//
		// Returns:
		//     Return an IResponseFilter to intercept this response, otherwise return null
		public IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response) {
			return null;
		}
		//
		// Summary:
		//     Called on the CEF IO thread before a resource request is loaded. To redirect
		//     or change the resource load optionally modify request. Modification of the request
		//     URL will be treated as a redirect
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object - can be modified in this callback.
		//
		//   callback:
		//     Callback interface used for asynchronous continuation of url requests.
		//
		// Returns:
		//     Return CefSharp.CefReturnValue.Continue to continue the request immediately.
		//     Return CefSharp.CefReturnValue.ContinueAsync and call CefSharp.IRequestCallback.Continue(System.Boolean)
		//     or CefSharp.IRequestCallback.Cancel at a later time to continue or the cancel
		//     the request asynchronously. Return CefSharp.CefReturnValue.Cancel to cancel the
		//     request immediately.
		public CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback) {

			// if referer given
			var tab = myForm.GetTabByBrowser(chromiumWebBrowser);
			if (tab != null && tab.RefererURL != null) {

				// Set referer
				request.SetReferrer(tab.RefererURL, ReferrerPolicy.Always);

			}

			return CefSharp.CefReturnValue.Continue;
		}
		//
		// Summary:
		//     Called on the CEF UI thread to handle requests for URLs with an unknown protocol
		//     component. SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS
		//     BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		// Returns:
		//     return to true to attempt execution via the registered OS protocol handler, if
		//     any. Otherwise return false.
		public bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request) {
			return true;
		}
		//
		// Summary:
		//     Called on the CEF IO thread when a resource load has completed. This method will
		//     be called for all requests, including requests that are aborted due to CEF shutdown
		//     or destruction of the associated browser. In cases where the associated browser
		//     is destroyed this callback may arrive after the CefSharp.ILifeSpanHandler.OnBeforeClose(CefSharp.IWebBrowser,CefSharp.IBrowser)
		//     callback for that browser. The CefSharp.IFrame.IsValid method can be used to
		//     test for this situation, and care should be taken not to call browser or frame
		//     methods that modify state (like LoadURL, SendProcessMessage, etc.) if the frame
		//     is invalid.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
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
		public void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength) {

			int code = response.StatusCode;


			// if NOT FOUND
			if (code == 404) {

				if (!request.Url.IsURLLocalhost()) {

					// redirect to web archive to try and find older version
					frame.LoadUrl("http://web.archive.org/web/*/" + request.Url);

				}
				else {

					// show offline "file not found" page
					frame.LoadUrl(MainForm.FileNotFoundURL + "?path=" + request.Url.EncodeURL());
				}

			}


			// if FILE NOT FOUND
			else if (request.Url.IsURLOfflineFile()) {
				string path = request.Url.FileURLToPath();
				if (path.FileNotExists()) {

					// show offline "file not found" page
					frame.LoadUrl(MainForm.FileNotFoundURL + "?path=" + path.EncodeURL());

				}
			}
			else {


				// if CANNOT CONNECT
				if (code == 0 || code == 444 || (code >= 500 && code <= 599)) {

					// show offline "cannot connect to server" page
					frame.LoadUrl(MainForm.CannotConnectURL);
				}

			}

		}
		//
		// Summary:
		//     Called on the CEF IO thread when a resource load is redirected. The request parameter
		//     will contain the old URL and other request-related information. The response
		//     parameter will contain the response that resulted in the redirect. The newUrl
		//     parameter will contain the new URL and can be changed if desired.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object - cannot be modified in this callback
		//
		//   response:
		//     the response object - cannot be modified in this callback
		//
		//   newUrl:
		//     the new URL and can be changed if desired
		public void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl) {
		}
		//
		// Summary:
		//     Called on the CEF IO thread when a resource response is received. To allow the
		//     resource load to proceed without modification return false. To redirect or retry
		//     the resource load optionally modify request and return true. Modification of
		//     the request URL will be treated as a redirect. Requests handled using the default
		//     network loader cannot be redirected in this callback. WARNING: Redirecting using
		//     this method is deprecated. Use OnBeforeResourceLoad or GetResourceHandler to
		//     perform redirects.
		//
		// Parameters:
		//   chromiumWebBrowser:
		//     The ChromiumWebBrowser control
		//
		//   browser:
		//     the browser object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   frame:
		//     the frame object - may be null if originating from ServiceWorker or CefURLRequest
		//
		//   request:
		//     the request object
		//
		//   response:
		//     the response object - cannot be modified in this callback
		//
		// Returns:
		//     To allow the resource load to proceed without modification return false. To redirect
		//     or retry the resource load optionally modify request and return true. Modification
		//     of the request URL will be treated as a redirect. Requests handled using the
		//     default network loader cannot be redirected in this callback.
		public bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response) {
			
			return false;

		}

	}
}
