
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
	internal class SchemeHandler : IResourceHandler, IDisposable {
		private static string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\";

		private string mimeType;
		private Stream stream;
		MainForm myForm;
		private Uri uri;
		private string fileName;

		public SchemeHandler(MainForm form) {
			myForm = form;
		}

		public void Dispose() {

		}

		//
		// Summary:
		//     Open the response stream. - To handle the request immediately set handleRequest
		//     to true and return true. - To decide at a later time set handleRequest to false,
		//     return true, and execute callback to continue or cancel the request. - To cancel
		//     the request immediately set handleRequest to true and return false. This method
		//     will be called in sequence but not from a dedicated thread. For backwards compatibility
		//     set handleRequest to false and return false and the CefSharp.IResourceHandler.ProcessRequest(CefSharp.IRequest,CefSharp.ICallback)
		//     method will be called.
		//
		// Parameters:
		//   request:
		//     request
		//
		//   handleRequest:
		//     see main summary
		//
		//   callback:
		//     callback
		//
		// Returns:
		//     see main summary
		public bool Open(IRequest request, out bool handleRequest, ICallback callback) {
			uri = new Uri(request.Url);
			fileName = uri.AbsolutePath;

			// if url is blocked
			/*if (...request.Url....) {

				// cancel the request - set handleRequest to true and return false
				handleRequest = true;
				return false;
			}*/

			// if url is browser file
			if (uri.Host == "storage") {
				fileName = appPath + uri.Host + fileName;
				if (File.Exists(fileName)) {
					Task.Factory.StartNew(() => {
						using (callback) {
							FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
							mimeType = ResourceHandler.GetMimeType(Path.GetExtension(fileName));
							stream = fStream;
							callback.Continue();
						}
					});

					// handle the request at a later time
					handleRequest = false;
					return true;
				}
			}

			// if url is request for icon of another file
			if (uri.Host == "fileicon") {
				Task.Factory.StartNew(() => {
					using (callback) {
						stream = FileIconUtils.GetFileIcon(fileName, FileIconSize.Large);
						mimeType = ResourceHandler.GetMimeType(".png");
						callback.Continue();
					}
				});

				// handle the request at a later time
				handleRequest = false;
				return true;
			}


			// by default reject
			callback.Dispose();

			// cancel the request - set handleRequest to true and return false
			handleRequest = true;
			return false;
		}
		//
		// Summary:
		//     Retrieve response header information. If the response length is not known
		//     set responseLength to -1 and ReadResponse() will be called until it returns
		//     false. If the response length is known set responseLength to a positive value
		//     and ReadResponse() will be called until it returns false or the specified
		//     number of bytes have been read. If an error occured while setting up the
		//     request you can set CefSharp.IResponse.ErrorCode to indicate the error condition.
		//
		// Parameters:
		//   response:
		//     Use the response object to set the mime type, http status code and other
		//     optional header values.
		//
		//   responseLength:
		//     If the response length is not known set responseLength to -1
		//
		//   redirectUrl:
		//     To redirect the request to a new URL set redirectUrl to the new Url.
		public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl) {

			responseLength = stream != null ? stream.Length : 0; 
			redirectUrl = null;

			response.StatusCode = (int)HttpStatusCode.OK;
			response.StatusText = "OK";
			response.MimeType = mimeType;

			//return stream;
		}
		//
		// Summary:
		//     Read response data. If data is available immediately copy to dataOut, set
		//     bytesRead to the number of bytes copied, and return true.  To read the data
		//     at a later time set bytesRead to 0, return true and call ICallback.Continue()
		//     when the data is available. To indicate response completion return false.
		//
		// Parameters:
		//   dataOut:
		//     Stream to write to
		//
		//   bytesRead:
		//     Number of bytes copied to the stream
		//
		//   callback:
		//     The callback used to Continue or Cancel the request (async).
		//
		// Returns:
		//     If data is available immediately copy to dataOut, set bytesRead to the number
		//     of bytes copied, and return true.To indicate response completion return false.
		//
		// Remarks:
		//     Depending on this size of your response this method may be called multiple
		//     times
		public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback) {

			//Dispose the callback as it's an unmanaged resource, we don't need it in this case
			callback.Dispose();

			if (stream == null) {
				bytesRead = 0;
				return false;
			}

			//Data out represents an underlying buffer (typically 32kb in size).
			var buffer = new byte[dataOut.Length];
			bytesRead = stream.Read(buffer, 0, buffer.Length);

			dataOut.Write(buffer, 0, buffer.Length);

			return bytesRead > 0;

		}

		//
		// Summary:
		//     Read response data. If data is available immediately copy up to dataOut.Length
		//     bytes into dataOut, set bytesRead to the number of bytes copied, and return true.
		//     To read the data at a later time keep a pointer to dataOut, set bytesRead to
		//     0, return true and execute callback when the data is available (dataOut will
		//     remain valid until the callback is executed). To indicate response completion
		//     set bytesRead to 0 and return false. To indicate failure set bytesRead to < 0
		//     (e.g. -2 for ERR_FAILED) and return false. This method will be called in sequence
		//     but not from a dedicated thread. For backwards compatibility set bytesRead to
		//     -1 and return false and the ReadResponse method will be called.
		//
		// Parameters:
		//   dataOut:
		//     If data is available immediately copy up to System.IO.Stream.Length bytes into
		//     dataOut.
		//
		//   bytesRead:
		//     To indicate response completion set bytesRead to 0 and return false.
		//
		//   callback:
		//     set bytesRead to 0, return true and execute callback when the data is available
		//     (dataOut will remain valid until the callback is executed). If you have no need
		//     of the callback then Dispose of it immeduately.
		//
		// Returns:
		//     return true or false depending on the criteria, see summary.
		public bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback) {

			// For backwards compatibility set bytesRead to
			//     -1 and return false and the ReadResponse method will be called.
			bytesRead = -1;
			return false;
		}

		//
		// Summary:
		//     Skip response data when requested by a Range header. Skip over and discard bytesToSkip
		//     bytes of response data. - If data is available immediately set bytesSkipped to
		//     the number of of bytes skipped and return true. - To read the data at a later
		//     time set bytesSkipped to 0, return true and execute callback when the data is
		//     available. - To indicate failure set bytesSkipped to < 0 (e.g. -2 for ERR_FAILED)
		//     and return false. This method will be called in sequence but not from a dedicated
		//     thread.
		//
		// Parameters:
		//   bytesToSkip:
		//     number of bytes to be skipped
		//
		//   bytesSkipped:
		//     If data is available immediately set bytesSkipped to the number of of bytes skipped
		//     and return true. To read the data at a later time set bytesSkipped to 0, return
		//     true and execute callback when the data is available.
		//
		//   callback:
		//     To read the data at a later time set bytesSkipped to 0, return true and execute
		//     callback when the data is available.
		//
		// Returns:
		//     See summary
		public bool Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback) {
			// To indicate failure set bytesSkipped to < 0 (e.g. -2 for ERR_FAILED)
			//     and return false.
			bytesSkipped = -2;
			return false;
		}

		// Summary:
		//     Request processing has been canceled.
		public void Cancel() {
		}
		//
		// Summary:
		//     Return true if the specified cookie can be sent with the request or false
		//     otherwise. If false is returned for any cookie then no cookies will be sent
		//     with the request.
		public bool CanGetCookie(CefSharp.Cookie cookie) {
			return true;
		}
		//
		// Summary:
		//     Return true if the specified cookie returned with the response can be set
		//     or false otherwise.
		public bool CanSetCookie(CefSharp.Cookie cookie) {
			return true;
		}

		//
		// Summary:
		//     Begin processing the request.
		//
		// Parameters:
		//   request:
		//     The request object.
		//
		//   callback:
		//     The callback used to Continue or Cancel the request (async).
		//
		// Returns:
		//     To handle the request return true and call CefSharp.ICallback.Continue once the
		//     response header information is available CefSharp.ICallback.Continue can also
		//     be called from inside this method if header information is available immediately).
		//     To cancel the request return false.
		public bool ProcessRequest(IRequest request, ICallback callback) {
			return false;
		}


	}
}