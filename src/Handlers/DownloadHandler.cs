using CefSharp;

namespace SharpBrowser
{
    public class DownloadHandler : IDownloadHandler
    {
        MainForm myForm;

        public DownloadHandler(MainForm form)
        {
            myForm = form;
        }

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    myForm.UpdateDownloadItem(downloadItem);
                    callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
                }
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            myForm.UpdateDownloadItem(downloadItem);
            if (downloadItem.IsInProgress && myForm.CancelRequests.Contains(downloadItem.Id)) callback.Cancel();
            //Console.WriteLine(downloadItem.Url + " %" + downloadItem.PercentComplete + " complete");
        }
    }
}
