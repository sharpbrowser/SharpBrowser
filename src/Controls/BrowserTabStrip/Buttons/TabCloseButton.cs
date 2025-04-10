using SharpBrowser.Config;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SharpBrowser.Controls.BrowserTabStrip.Buttons {
	internal class TabCloseButton(ToolStripProfessionalRenderer renderer) : TabButtonBase(renderer) {
		
		public override void Draw(Graphics g) {
			if (IsVisible) {
				//g.FillRectangle(Brushes.White, Rect);
				if (IsMouseOver) {
					g.FillRoundRectangle(BrowserTabStyle.TabCloseButton_RollOverColor, Rect, 10);
				}
				int padding = 6;
				Pen pen = new Pen(BrowserTabStyle.TabCloseButton_TextColor, 1.6f);
				g.DrawLine(pen, Rect.Left + padding, Rect.Top + padding, Rect.Right - padding, Rect.Bottom - padding);
				g.DrawLine(pen, Rect.Right - padding, Rect.Top + padding, Rect.Left + padding, Rect.Bottom - padding);
				pen.Dispose();
			}
		}

	}
}