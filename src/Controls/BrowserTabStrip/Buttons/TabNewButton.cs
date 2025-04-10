using SharpBrowser.Config;
using System.Drawing;
using System.Windows.Forms;

namespace SharpBrowser.Controls.BrowserTabStrip.Buttons {
	internal class TabNewButton(ToolStripProfessionalRenderer renderer) : TabButtonBase(renderer) {
	
		public override void Draw(Graphics g) {
			if (IsVisible) {
				//g.FillRectangle(Brushes.White, Rect);
				if (IsMouseOver) {
					g.FillRoundRectangle(BrowserTabStyle.TabNewButton_RollOverColor, Rect, 10);
				}
				int num = 4;
				int centerX = Rect.X + Rect.Width / 2;
				int centerY = Rect.Y + Rect.Height / 2;
				Pen pen = new Pen(BrowserTabStyle.TabNewButton_TextColor, 1.6f);
				g.DrawLine(pen, centerX, Rect.Top + num, centerX, Rect.Bottom - num);
				g.DrawLine(pen, Rect.Right - num, centerY, Rect.Left + num, centerY);
				pen.Dispose();
			}
		}

	}
}