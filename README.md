![SharpBrowser](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/logo3.png)

SharpBrowser is the fastest open source C# web browser there is! Slightly faster than Google Chrome when rendering web pages due to lightweight CEF renderer. We compared every available .NET browsing browsing engine and finally settled on the high-performance [CefSharp](https://github.com/cefsharp/CefSharp/). Released under the permissive MIT license.

## Features

- HTML5, CSS3, JS, HTML5 Video, WebGL 3D, etc
- Tabbed browsing (open in new tab, etc)
- Address bar (also opens Google Search)
- Back, Forward, Stop, Refresh
- Developer tools
- Search bar (highlights all instances, find next, previous)
- Download manager (download progress, cancellation, etc)
- Custom error pages (cannot connect, file not found)
- Custom context menu
- Open online & offline webpages (localhost, filesystem)
- Easy to add any vendor-specific branding, buttons or hotkeys
- Uses CefSharp 51, NET Framework 4.5.2

## Hotkeys

Hotkeys | Function
------------ | -------------
Ctrl+T		| Add a new tab
Ctrl+W		| Add a new window (simply opens another instance of SharpBrowser)
Ctrl+W		| Close active tab
F5			| Refresh active tab
F12			| Open developer tools
Ctrl+Tab	| Switch to the next tab
Ctrl+Shift+Tab	| Switch to the previous tab
Ctrl+F		| Open search bar (Enter to find next, Esc to close)

## Code

- `MainForm.cs` - main web browser UI and related functionality
- `Handlers` - various handlers that we have registered with CefSharp that enable deeper integration between us and CefSharp
- `Data/JSON.cs` - fast JSON serializer/deserializer
- `bin` - Binaries are included in the `bin` folder due to the complex CefSharp setup required. Don't empty this folder.
- `bin/storage` - HTML and JS required for downloads manager and custom error pages

## Credits

- [Harsh Gupta](https://github.com/hgupta9) - SharpBrowser project.
- [Alex Maitland](https://github.com/amaitland) - CefSharp project, wrapper for CEF embeddable browser.
- [Ahmet Uzun](https://github.com/postacik) - Original browser project.

## Screenshots

### Apple Homepage

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/1.png)

### Google Maps

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/2.png)

### Downloads Tab

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/3.png)

### Developer Tools

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/4.png)

### Search Bar

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/search.png)

### Error Pages

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/error1.png)

![](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/error2.png)

