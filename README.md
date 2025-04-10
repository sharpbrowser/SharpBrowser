![SharpBrowser](https://github.com/sharpbrowser/SharpBrowser/raw/master/images/logo3.png)

SharpBrowser is the fastest and most full-featured open source C# web browser there is! Slightly faster than Google Chrome when rendering web pages due to lightweight CEF renderer. We compared every available .NET browsing engine and finally settled on the high-performance [CefSharp](https://github.com/cefsharp/CefSharp/). Released under the permissive MIT license.

## Features

- HTML5, CSS3, JS, HTML5 Video, PDF, WebGL 3D, WebAssembly, WebRTC, WebMIDI
- Tabbed browsing
- Address bar (also opens Google or any search engine)
- Back, Forward, Stop, Refresh, Home, Menu button
- Print and Print to PDF
- Developer tools
- Search bar (also highlights all instances)
- File downloads and download manager
- Custom error pages
- Custom context menu
- Custom application main menu
- Easily add [your own branding](#customization), styling, buttons or hotkeys
- View online & offline webpages
- Web app permission handling based on configuration
- Popups open in new tabs
- When the last tab is closed, the browser window closes down

## Hotkeys

Hotkeys | Function
------------ | -------------
Ctrl+MouseWheel		| Zoom in/out
Ctrl+T		| Add a new tab
Ctrl+N		| Add a new window
Ctrl+W		| Close active tab
Ctrl+MiddleClick		| Close Tab
F5			| Refresh active tab
F11			| Toggle fullscreen
F12			| Open developer tools
Ctrl+Tab	| Switch to the next tab
Ctrl+Shift+Tab	| Switch to the previous tab
Ctrl+F		| Open search bar (Enter to find next, Esc to close)
Ctrl+P 		| Print
Ctrl+Shift+P 		| Print to PDF


## System requirements

- You need .NET 8 on Windows 64-bit.

- You need [VC++ 2019 Runtime](https://aka.ms/vs/17/release/vc_redist.x64.exe) (64-bit)
- You might need [VC++ 2017 Runtime](https://www.microsoft.com/en-in/download/details.aspx?id=48145) (64-bit)

- You need to install the version of VC++ Runtime that CEFSharp needs. As per our CefSharp version, according to [this](https://github.com/cefsharp/CefSharp/#release-branches), we need the above versions


## Getting started

- See the [Compilation Guide](docs/Compilation.md) for steps to get started.


## Customization

- To customize the browser branding, name, URL, default search engine, default proxy, modify the `BrowserConfig` class.

- To customize the application icon, change `sharpbrowser.ico` inside the `Resources` folder.

- To customize the tab size and tab colors, modify the `BrowserTabStyle` class.

- To enable or disable Web Camera, Javascript, WebGL, WebRTC, WebMIDI, LocalStorage, modify the `BrowserConfig` class.

- To register hotkeys for your own commands, modify the `MainForm.InitHotkeys` function.

- To register your own commands into the main menu, open the form designed for `MainForm` and click the `MainMenu` object. Add `IconMenuItem` objects into that menu.

- To register your own commands into the page context-menu, modify `ContextMenuHandler.OnBeforeContextMenu` function, and then implement the command inside `ContextMenuHandler.OnContextMenuCommand`.

- To setup how web app permissions are handled, modify `PermissionHandler.OnShowPermissionPrompt` (some flags are already inside `BrowserConfig` and can easily be changed).



## Documentation

- [User Guide](docs/Users.md)
- [Compilation Guide](docs/Compilation.md)
- [Distribution Guide](docs/Distribution.md)


## Code

- SharpBrowser uses CefSharp 134 and is built on NET.
- SharpBrowser only supports Windows x64 platform.
- `MainForm.cs` - main web browser UI and related functionality
- `Handlers` - various handlers that we have registered with CefSharp that enable deeper integration between us and CefSharp
- `bin` - Binaries are included in the `bin` folder due to the complex CefSharp setup required. Don't empty this folder.
- `bin/storage` - HTML and JS required for downloads manager and custom error pages

## Screenshots

<table>
	<tr>
		<td align="center">
			<strong>Apple.com</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/apple.png" width="420"/>
		</td>
		<td align="center">
			<strong>Google Maps</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/googlemaps.png" width="420"/>
		</td>
	</tr>
	<tr>
		<td align="center">
			<strong>WebGL</strong></strong> <a href="https://www.polaris.com/en-us/off-road/rzr/build-model/">(test)</a><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/webgl.png" width="420"/>
		</td>
		<td align="center">
			<strong>PDF Viewer</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/pdf.png" width="420"/>
		</td>
	</tr>
	<tr>
		<td align="center">
			<strong>Web Camera</strong> <a href="https://webcamtests.com/">(test)</a><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/webcam.png" width="420"/>
		</td>
		<td align="center">
			<strong>WebMIDI</strong> <a href="https://muted.io/piano/">(test)</a><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/webmidi.png" width="420"/>
		</td>
	</tr>
	<tr>
		<td align="center">
			<strong>WebAssembly</strong></strong> <a href="https://browserbench.org/JetStream2.0/">(test)</a><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/wasm.png" width="420"/>
		</td>
		<td align="center">
			<strong>YouTube</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/youtube.png" width="420"/>
		</td>
	</tr>
	<tr>
		<td align="center">
			<strong>Search Bar</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/search.png" width="420"/>
		</td>
		<td align="center">
			<strong>Main Menu</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/mainmenu.png" width="420"/>
		</td>
	</tr>
	<tr>
		<td align="center">
			<strong>Downloads Tab</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/downloads.png" width="420"/>
		</td>
		<td align="center">
			<strong>Developer Tools</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/devtools.png" width="420"/>
		</td>
	</tr>
	<tr>
		<td align="center">
			<strong>Custom Error Page 1</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/error1.png" width="420"/>
		</td>
		<td align="center">
			<strong>Custom Error Page 2</strong><br>
			<img src="https://github.com/sharpbrowser/SharpBrowser/raw/master/images/error2.png" width="420"/>
		</td>
	</tr>
</table>