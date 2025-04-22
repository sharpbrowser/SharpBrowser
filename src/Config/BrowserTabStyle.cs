using System.Drawing;

namespace SharpBrowser.Config {
	internal static class BrowserTabStyle {

		// Tab styles

		public static int TabHeight = 40;
		public static int TabLeftPadding = 10;

		public static int TabCloseButton_XOffset = 28;
		public static int TabButton_Y = 10;
		public static int Tab_IconSize = 16;

		public static Color SelectedTabBackColor = Color.FromArgb(247, 247, 247);
		public static Color NormalTabBackColor = Color.FromArgb(225, 225, 225);

		public static SolidBrush BackColor = new SolidBrush(NormalTabBackColor);

		public static Color TabBorderColor = Color.LightGray;
		public static int TabBorderThickness = 2;

		// Close tab button (X)

		public static SolidBrush TabCloseButton_TextColor = new SolidBrush(Color.DarkSlateGray);
		public static SolidBrush TabCloseButton_RollOverColor = new SolidBrush(Color.LightGray);

		// New tab button (+)

		public static SolidBrush TabNewButton_TextColor = new SolidBrush(Color.DarkSlateGray);
		public static SolidBrush TabNewButton_RollOverColor = new SolidBrush(Color.LightGray);


	}
}
