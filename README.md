![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/logo.png)

# SharpBrowser
The fastest open source C# web browser there is! Slightly faster than Google Chrome when rendering web pages due to lightweight CEF renderer. We compared every available .NET browsing browsing engine and finally settled on the high-performance [CefSharp](https://github.com/cefsharp/CefSharp/). Released under the permissive MIT license.

## Features

- HTML5, CSS3, JS, HTML5 Video, WebGL 3D, etc
- Tabbed browsing (open in new tab, etc)
- Address bar (also opens Google Search)
- Back, Forward, Stop, Refresh
- Developer tools
- Downloads window (download progress, cancellation, etc)
- Custom context menu
- Easy to add any vendor-specific buttons or commands

## Code

- `MainForm.cs` - main web browser UI and related functionality
- `Handlers` - various handlers that we have registered with CefSharp that enable deeper integration between us and CefSharp
- `Data/JSON.cs` - fast JSON serializer/deserializer
- `bin` - Binaries are included in the `bin` folder due to the complex CefSharp setup required. Don't empty this folder.
- `bin/storage` - JS code and associated assets required for "downloads" page

## Screenshots

### Apple Homepage

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/1.png)

### Google Maps

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/2.png)

### Downloads Tab

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/3.png)

### Developer Tools

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/4.png)