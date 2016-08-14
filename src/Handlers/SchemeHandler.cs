
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CefSharp;
using System.Windows.Forms;
using System.Drawing;

namespace SharpBrowser
{
    internal class SchemeHandler : IResourceHandler
    {
        private static readonly IDictionary<string, string> ResourceDictionary;
        private static string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\";

        private string mimeType;
        private Stream stream;
        
        static SchemeHandler()
        {
            ResourceDictionary = new Dictionary<string, string>
            {
                { "/home.html", "" }
            };
        }

        private MemoryStream GetFileIcon(string name, IconReader.IconSize size)
        {
            Icon icon = IconReader.GetFileIcon(name, size, false);
            using (icon)
            {
                using (var bmp = icon.ToBitmap())
                {
                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);
                    return ms;
                }
            }
        }


        public bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            if (uri.Host == "storage")
            {
                fileName = appPath + uri.Host + fileName;
                if (File.Exists(fileName))
                {
                    Task.Factory.StartNew(() =>
                    {
                        using (callback)
                        {
                            //var bytes = Encoding.UTF8.GetBytes(resource);
                            //stream = new MemoryStream(bytes);
                            FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                            mimeType = ResourceHandler.GetMimeType(Path.GetExtension(fileName));
                            stream = fStream;
                            callback.Continue();
                        }
                    });

                    return true;
                }
            }

            if (uri.Host == "fileicon")
            {
                Task.Factory.StartNew(() =>
                {
                    using (callback)
                    {
                        stream = GetFileIcon(fileName, IconReader.IconSize.Large);
                        mimeType = ResourceHandler.GetMimeType(".png");
                        callback.Continue();
                    }
                });
                return true;
            }


            callback.Dispose();
            return false;
        }

        public Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream.Length;
            redirectUrl = null;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusText = "OK";
            response.MimeType = mimeType;

            return stream;
        }
    }
}
