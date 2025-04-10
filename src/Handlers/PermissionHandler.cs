using CefSharp;
using SharpBrowser.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser.Handlers {
	internal class PermissionHandler : IPermissionHandler {
		/// <summary>
		/// Called when a permission prompt handled via OnShowPermissionPrompt(IWebBrowser, IBrowser, UInt64, String, PermissionRequestType, IPermissionPromptCallback) is dismissed. result will be the value passed to Continue(PermissionRequestResult) or Ignore if the dialog was dismissed for other reasons such as navigation, browser closure, etc. This method will not be called if OnShowPermissionPrompt(IWebBrowser, IBrowser, UInt64, String, PermissionRequestType, IPermissionPromptCallback) returned false for promptId.
		/// </summary>
		/// <param name="promptId">Will match the value that was passed to OnShowPermissionPrompt</param>
		/// <param name="result">will be the value passed to Continue(PermissionRequestResult) or Ignore if the dialog was dismissed for other reasons such as navigation, browser closure, etc. This method will not be called if OnShowPermissionPrompt(IWebBrowser, IBrowser, UInt64, String, PermissionRequestType, IPermissionPromptCallback) returned false for promptId.</param>
		void IPermissionHandler.OnDismissPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, PermissionRequestResult result) {
			
		}

		/// <summary>
		/// Called when a page requests permission to access media.
		/// With the Chrome runtime, default handling will display the permission request UI.
		/// With the Alloy runtime, default handling will deny the request.
		/// This method will not be called if the "--enable-media-stream" command-line switch is used to grant all permissions.
		/// </summary>
		/// <param name="requestingOrigin">is the URL origin requesting permission.</param>
		/// <param name="requestedPermissions">is a combination of values that represent the requested permissions</param>
		/// <param name="callback">Callback interface used for asynchronous continuation of media access.</param>
		bool IPermissionHandler.OnRequestMediaAccessPermission(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string requestingOrigin, MediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback) {
			return true;
		}

		/// <summary>
		/// Called when a page should show a permission prompt.
		/// </summary>
		/// <param name="promptId">Uniquely identifies the prompt.</param>
		/// <param name="requestingOrigin">Is the URL origin requesting permission.</param>
		/// <param name="requestedPermissions">Is a combination of values from PermissionRequestType that represent the requested permissions.</param>
		/// <param name="callback">Callback interface used for asynchronous continuation of permission prompts.</param>
		bool IPermissionHandler.OnShowPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, string requestingOrigin, PermissionRequestType requestedPermissions, IPermissionPromptCallback callback) {

			var allow = false;


			// DENY FEATURES THAT ARE NOT SUPPORTED
			switch (requestedPermissions) {

				case PermissionRequestType.Notifications:
				case PermissionRequestType.StorageAccess:
				case PermissionRequestType.TopLevelStorageAccess:
				case PermissionRequestType.WindowManagement:
				case PermissionRequestType.RegisterProtocolHandler:
					allow = false;
					break;



				// ACCEPT/DENY BASED ON APP CONFIG

				case PermissionRequestType.MidiSysex:
					allow = BrowserConfig.WebMidi;
					break;
				case PermissionRequestType.CameraStream:
				case PermissionRequestType.CameraPanTiltZoom:
					allow = BrowserConfig.Camera;
					break;
				case PermissionRequestType.Clipboard:
					allow = BrowserConfig.JavascriptClipboard;
					break;


				// ACCEPT THINGS THAT ARE SUPPORTED

				case PermissionRequestType.LocalFonts:
					allow = true;
					break;

				case PermissionRequestType.MultipleDownloads:
					allow = true;
					break;

			}


			// DENY UNKNOWN BY DEFAULT
			if (allow) {
				callback.Continue(PermissionRequestResult.Accept);
			}else {
				callback.Continue(PermissionRequestResult.Deny);
			}

			// returning true means the browser has handled this request
			return true;
		}
	}
}
