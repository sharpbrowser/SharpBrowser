using Newtonsoft.Json;
using SharpBrowser.Managers;

namespace SharpBrowser.Handlers {

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
			lock (DownloadManager.Downloads) {
				var json = JsonConvert.SerializeObject(DownloadManager.Downloads.Values);
				return json;
			}
		}

		public bool cancelDownload(int downloadId) {
			DownloadManager.Cancel(downloadId);
			return true;
		}
		public void refreshActiveTab() {
			myForm.RefreshActiveTab();
		}
	}

}
