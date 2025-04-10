using System.Drawing;
using System.Windows.Forms;

namespace SharpBrowser.Controls.BrowserTabStrip.Buttons {
	internal abstract class TabButtonBase {

		public Rectangle Rect = Rectangle.Empty;
		public Rectangle RedrawRect = Rectangle.Empty;
		public bool IsMouseOver;
		public bool IsVisible;
		public ToolStripProfessionalRenderer Renderer;

		internal TabButtonBase(ToolStripProfessionalRenderer renderer) {
			Renderer = renderer;
		}

		public void CalcBounds(BrowserTabItem tab, bool displayInButton) {
			var tabrect = tab.StripRect;
			var x = displayInButton ? (int)tab.StripRect.Right - BrowserTabStyle.TabCloseButton_XOffset : (int)tab.StripRect.Right + 10;
			var y = (int)tab.StripRect.Top + BrowserTabStyle.TabButton_Y;
			Rect = new Rectangle(x, y, 20, 20);
			RedrawRect = new Rectangle(Rect.X - 2, Rect.Y - 2, Rect.Width + 4, Rect.Height + 4);
		}

		public virtual void Draw(Graphics g) {
		}

		public void ProcessRolloverEvents(Control parent, MouseEventArgs e) {
			if (IsVisible) {
				if (Rect.Contains(e.Location)) {
					IsMouseOver = true;
					parent.Invalidate(RedrawRect);
				}
				else if (IsMouseOver) {
					IsMouseOver = false;
					parent.Invalidate(RedrawRect);
				}
			}
		}
		public void ProcessRolloutEvents(Control parent) {
			IsMouseOver = false;
			if (IsVisible) {
				parent.Invalidate(RedrawRect);
			}
		}

	}
}
