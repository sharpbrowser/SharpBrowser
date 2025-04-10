using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser.Controls {
	internal static class BrowserTabStyle {

		public static int TabHeight = 40;
		public static int TabLeftPadding = 10;

		public static int TabCloseButton_XOffset = 28;
		public static int TabButton_Y = 10;

		public static Color SelectedTabBackColor = Color.FromArgb(247, 247, 247);
		public static Color NormalTabBackColor = Color.FromArgb(230, 230, 230);

		public static SolidBrush BackColor = new SolidBrush(NormalTabBackColor);

		public static SolidBrush TabCloseButton_TextColor = new SolidBrush(Color.DarkSlateGray);
		public static SolidBrush TabCloseButton_RollOverColor = new SolidBrush(Color.LightGray);

		public static SolidBrush TabNewButton_TextColor = new SolidBrush(Color.DarkSlateGray);
		public static SolidBrush TabNewButton_RollOverColor = new SolidBrush(Color.LightGray);

	}
}
