# SharpBrowser
A fast and functional web browser built using C# and CefSharp. Slightly faster than Google Chrome when rendering web pages due to lightweight CEF renderer.

Features:

- Tabbed browsing (open in new tab, etc)
- URL / Address bar
- Back, Forward, Stop/Refresh
- Developer tools
- Downloads window (percentage complete, cancelling, etc)
- Custom context menu

Code organization:

- `MainForm.cs` - web browser UI and functionality
- `Handlers` - various handlers that we have registered with CefSharp that enable deeper integration between the SharpBrowser and CefSharp
- `Data/JSON.cs` - fast JSON serializer/deserializer
- `bin` - Binaries are included in the `bin` folder due to the complex CefSharp setup required. Don't empty this folder.
- `bin/storage` - JS code and associated assets required for "downloads" page

Apple Homepage
![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/1.png)

Google Maps
![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/2.png)

Downloads Tab
![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/3.png)

Developer Tools
![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/4.png)