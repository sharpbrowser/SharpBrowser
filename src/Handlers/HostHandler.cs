using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace SharpBrowser {

	/// <summary>
	/// functions in this class are accessible by JS using the code `host.X()`
	/// </summary>
	internal class HostHandler {
		MainForm myForm;

		public HostHandler(MainForm form) {
			myForm = form;
		}
		public void addNewBrowserTab(string url, bool focusNewTab = true) {
			myForm.AddNewBrowserTab(url, focusNewTab);
		}
		public string getDownloads() {
			lock (myForm.downloads) {
				var json = JsonConvert.SerializeObject(myForm.downloads.Values);
				return json;
			}
		}

		public bool cancelDownload(int downloadId) {
			lock (myForm.downloadCancelRequests) {
				if (!myForm.downloadCancelRequests.Contains(downloadId)) {
					myForm.downloadCancelRequests.Add(downloadId);
				}
			}
			return true;
		}
		public void refreshActiveTab() {
			myForm.RefreshActiveTab();
		}
	}

}
