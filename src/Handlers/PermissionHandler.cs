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


			// DENY FEATURES THAT ARE NOT SUPPORTED

			if (requestedPermissions == PermissionRequestType.Notifications) {
				return false;
			}
			if (requestedPermissions == PermissionRequestType.StorageAccess) {
				return false;
			}
			if (requestedPermissions == PermissionRequestType.TopLevelStorageAccess) {
				return false;
			}
			if (requestedPermissions == PermissionRequestType.WindowManagement) {
				return false;
			}
			if (requestedPermissions == PermissionRequestType.RegisterProtocolHandler) {
				return false;
			}


			// ACCEPT/DENY BASED ON APP CONFIG

			if (requestedPermissions == PermissionRequestType.MidiSysex) {
				return BrowserConfig.WebMidi;
			}
			if (requestedPermissions == PermissionRequestType.CameraStream) {
				return BrowserConfig.Camera;
			}
			if (requestedPermissions == PermissionRequestType.CameraPanTiltZoom) {
				return BrowserConfig.Camera;
			}
			if (requestedPermissions == PermissionRequestType.Clipboard) {
				return BrowserConfig.JavascriptClipboard;
			}


			// ACCEPT THINGS THAT ARE SUPPORTED

			if (requestedPermissions == PermissionRequestType.LocalFonts) {
				return true;
			}

			if (requestedPermissions == PermissionRequestType.MultipleDownloads) {
				return true;
			}


			// DENY UNKNOWN BY DEFAULT

			return false;
		}
	}
}
