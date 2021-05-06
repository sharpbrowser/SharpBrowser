using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SharpBrowser.BrowserTabStrip {
	[ToolboxItem(false)]
	public class BaseStyledPanel : ContainerControl {
		private static ToolStripProfessionalRenderer renderer;

		[Browsable(false)]
		public ToolStripProfessionalRenderer ToolStripRenderer => renderer;

		[Browsable(false)]
		[DefaultValue(true)]
		public bool UseThemes {
			get {
				if (VisualStyleRenderer.IsSupported && VisualStyleInformation.IsSupportedByOS) {
					return Application.RenderWithVisualStyles;
				}
				return false;
			}
		}

		public event EventHandler ThemeChanged;

		static BaseStyledPanel() {
			renderer = new ToolStripProfessionalRenderer();
		}

		public BaseStyledPanel() {
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
		}

		protected override void OnSystemColorsChanged(EventArgs e) {
			base.OnSystemColorsChanged(e);
			UpdateRenderer();
			Invalidate();
		}

		protected virtual void OnThemeChanged(EventArgs e) {
			if (this.ThemeChanged != null) {
				this.ThemeChanged(this, e);
			}
		}

		private void UpdateRenderer() {
			if (!UseThemes) {
				renderer.ColorTable.UseSystemColors = true;
			}
			else {
				renderer.ColorTable.UseSystemColors = false;
			}
		}
	}
}