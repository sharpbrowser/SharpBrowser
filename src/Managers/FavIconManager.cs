using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser.Managers {
	/// <summary>
	/// Downloads and caches favicons for any given URL.
	/// 
	/// The first time it will be slow as we have to:
	/// 1. Check if the website has a favicon at the default path.
	/// 2. Parse the HTML and look for 'link' tags that might link to an icon.
	/// 3. Download the icon and store the bitmap file data.
	/// 
	/// We use byte[] for cache storage instead of Bitmap,
	/// because storing as Bitmap causes a lot of GDI errors during rendering.
	/// </summary>
	internal static class FavIconManager {

		/// <summary>
		/// Callback to parent.
		/// </summary>
		public static Action<ChromiumWebBrowser, byte[]> OnLoaded;

		/// <summary>
		/// Thread-safe cache with concurrent dictionary
		/// </summary>
		private static readonly ConcurrentDictionary<string, byte[]> FaviconCache = new ConcurrentDictionary<string, byte[]>();
		/// <summary>
		///  Reuse HttpClient for better performance
		/// </summary>
		private static readonly HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) };
		private static readonly object _lockObject = new object();

		public static async void OnFrameLoadEnd(ChromiumWebBrowser browser, FrameLoadEndEventArgs e) {

			//try {
			var uri = new Uri(browser.Address);
			var domain = uri.Host;

			//--------------------------------------------------------------
			// 1. Check cache first for performance
			if (FaviconCache.TryGetValue(domain, out byte[] cachedIcon)) {

				//Console.WriteLine("Favicon loaded from cache.");
				OnLoaded(browser, cachedIcon);
				return;
			}

			byte[] iconBitmap = null;

			//--------------------------------------------------------------
			// 2. Try the standard favicon paths first
			iconBitmap = await TryGetFaviconFromUrl($"{uri.Scheme}://{domain}/favicon.ico");
			if (iconBitmap == null) {
				iconBitmap = await TryGetFaviconFromUrl($"{uri.Scheme}://{domain}/favicon.png");
			}
			if (iconBitmap != null) {
				//Console.WriteLine("Favicon loaded from default path.");
				StoreFavicon(domain, iconBitmap);
				OnLoaded(browser, iconBitmap);
				return;
			}

			//--------------------------------------------------------------
			// 3. Search for the link tag on the page for the icon path
			var result = await browser.EvaluateScriptAsync(FavIconJS);
			if (result.Success && result.Result is string iconHref && !string.IsNullOrWhiteSpace(iconHref)) {
				try {
					var iconUri = new Uri(iconHref, UriKind.RelativeOrAbsolute);
					if (!iconUri.IsAbsoluteUri) {
						iconUri = new Uri(uri, iconUri);
					}

					iconBitmap = await TryGetFaviconFromUrl(iconUri.ToString());
					if (iconBitmap != null) {
						//Console.WriteLine("Favicon loaded from <link> tag.");
						StoreFavicon(domain, iconBitmap);
						OnLoaded(browser, iconBitmap);
						return;
					}
				}
				catch (Exception ex) {
					//Console.WriteLine($"Error processing link favicon: {ex.Message}");
				}
			}

			// NOT FOUND!

			//Console.WriteLine("No favicon could be retrieved.");
			/*}
			catch (Exception ex) {
				Console.WriteLine($"Error in favicon retrieval process: {ex.Message}");
			}*/
		}

		/// <summary>
		/// Helper method to download the favicon bitmap from any favicon URL.
		/// </summary>
		private static async Task<byte[]> TryGetFaviconFromUrl(string iconUrl) {
			try {
				var response = await httpClient.GetAsync(iconUrl);

				if (!response.Content.Headers.ContentType.MediaType.StartsWith("image")) {
					// non image returned!!
					return null;
				}

				if (response.IsSuccessStatusCode) {
					using (var stream = await response.Content.ReadAsStreamAsync()) {
						try {
							// Create memory stream to avoid stream disposal issues
							using (var memoryStream = new MemoryStream()) {
								await stream.CopyToAsync(memoryStream);
								memoryStream.Position = 0;
								return memoryStream.ToArray();
							}
						}
						catch (ArgumentException) {
							// Invalid image format - silently fail
							return null;
						}
					}
				}
			}
			catch (Exception ex) {
				//Console.WriteLine($"HTTP error for {iconUrl}: {ex.Message}");
			}
			return null;
		}

		/// <summary>
		/// Helper method to safely store favicon in cache, using thread-safe code to prevent GDI errors.
		/// </summary>
		private static void StoreFavicon(string domain, byte[] icon) {
			if (icon != null) {
				FaviconCache[domain] = icon;
			}
		}


		/// <summary>
		/// JS script to search for HTML `link` tags on the loaded webpage, in order of preference.
		/// </summary>
		private static string FavIconJS = @"
            (function() {
                // Order of preference for rel attributes (most common first)
                var relPreference = ['icon', 'shortcut icon', 'apple-touch-icon', 'apple-touch-icon-precomposed'];
                var links = document.getElementsByTagName('link');
                    
                // First pass: try to find icons in preference order
                for (var p = 0; p < relPreference.length; p++) {
                    var preferredRel = relPreference[p];
                    for (var i = 0; i < links.length; i++) {
                        var rel = links[i].rel.toLowerCase();
                        if (rel === preferredRel && links[i].href) {
							var iconUrl = links[i].href;
							if (iconUrl.indexOf('.svg') == -1){
								return iconUrl;
							}
                        }
                    }
                }
                    
                // Second pass: any icon-like rel as a fallback
                for (var i = 0; i < links.length; i++) {
                    var rel = links[i].rel.toLowerCase();
                    if ((rel.indexOf('icon') !== -1) && links[i].href) {
						var iconUrl = links[i].href;
						if (iconUrl.indexOf('.svg') == -1){
							return iconUrl;
						}
                    }
                }
                    
                return null;
            })()
        ";
	}
}
