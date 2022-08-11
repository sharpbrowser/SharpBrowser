# How to distribute SharpBrowser with your own branding

#### Step 1: Clone the source using our tutorial

#### Step 2: Go to Solution explorer, and navigate `Model > BrowserConfig.cs`

![image](https://user-images.githubusercontent.com/104514709/183605344-97a50c0f-666a-4132-bf30-760525dc253e.png)

#### Step 3: Edit the configuration strings

![image](https://user-images.githubusercontent.com/104514709/183605417-67f274b2-fe9d-47b7-9d4e-1722387d2fb8.png)

#### Step 4: Compile the application

Make sure you select `Release` mode and then compile the application using the Build or Start button.

![image](https://user-images.githubusercontent.com/104514709/183605667-47ce966c-3167-4d34-9bd5-7feadf0710e5.png)

#### Step 5: Create a new installer package
To recreate the setup file, install [InnoSetup](https://jrsoftware.org/isinfo.php) and run the InnoSetup script in the `setup` folder.

#### Step 6: Share your installer package.
Share the newly generated installer package with your friends!