using CefSharp;

namespace SharpBrowser {
    internal class DownloadHandler : IDownloadHandler
    {
        MainForm myForm;

        public DownloadHandler(MainForm form)
        {
            myForm = form;
        }

        public void OnBeforeDownload(IBrowser browser, DownloadItem item, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {

                    myForm.UpdateDownloadItem(item);

					// ask browser what path it wants to save the file into
					string path = myForm.CalcDownloadPath(item);

					// if file should not be saved, path will be null, so skip file
					if (path != null) {

						// skip file
						callback.Continue(path, false);

					} else {

						// download file
						callback.Dispose();

						// open the downloads tab
						myForm.OpenDownloadsTab();

					}

                }
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            myForm.UpdateDownloadItem(downloadItem);
			if (downloadItem.IsInProgress && myForm.CancelRequests.Contains(downloadItem.Id)) {
				callback.Cancel();
			}
            //Console.WriteLine(downloadItem.Url + " %" + downloadItem.PercentComplete + " complete");
        }
    }
}
