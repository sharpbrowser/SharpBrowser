using System.Drawing;
using System.Windows.Forms;

namespace SharpBrowser.BrowserTabStrip {
	internal class BrowserTabStripCloseButton {
		public Rectangle Rect = Rectangle.Empty;

		public Rectangle RedrawRect = Rectangle.Empty;

		public bool IsMouseOver;

		public bool IsVisible;

		public ToolStripProfessionalRenderer Renderer;

		internal BrowserTabStripCloseButton(ToolStripProfessionalRenderer renderer) {
			Renderer = renderer;
		}

		public void CalcBounds(BrowserTabStripItem tab) {
			Rect = new Rectangle((int)tab.StripRect.Right - 20, (int)tab.StripRect.Top + 5, 15, 15);
			RedrawRect = new Rectangle(Rect.X - 2, Rect.Y - 2, Rect.Width + 4, Rect.Height + 4);
		}

		public void Draw(Graphics g) {
			if (IsVisible) {
				Color color = (IsMouseOver ? Color.White : Color.DarkGray);
				g.FillRectangle(Brushes.White, Rect);
				if (IsMouseOver) {
					g.FillEllipse(Brushes.IndianRed, Rect);
				}
				int num = 4;
				Pen pen = new Pen(color, 1.6f);
				g.DrawLine(pen, Rect.Left + num, Rect.Top + num, Rect.Right - num, Rect.Bottom - num);
				g.DrawLine(pen, Rect.Right - num, Rect.Top + num, Rect.Left + num, Rect.Bottom - num);
				pen.Dispose();
			}
		}
	}
}