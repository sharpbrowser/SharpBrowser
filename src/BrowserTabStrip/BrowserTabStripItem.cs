using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SharpBrowser.BrowserTabStrip {
	[ToolboxItem(false)]
	[DefaultProperty("Title")]
	[DefaultEvent("Changed")]
	public class BrowserTabStripItem : Panel {
		private RectangleF stripRect = Rectangle.Empty;

		private Image image;

		private bool canClose = true;

		private bool selected;

		private bool visible = true;

		private bool isDrawn;

		private string title = string.Empty;

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size Size {
			get {
				return base.Size;
			}
			set {
				base.Size = value;
			}
		}

		[DefaultValue(true)]
		public new bool Visible {
			get {
				return visible;
			}
			set {
				if (visible != value) {
					visible = value;
					OnChanged();
				}
			}
		}

		internal RectangleF StripRect {
			get {
				return stripRect;
			}
			set {
				stripRect = value;
			}
		}

		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public bool IsDrawn {
			get {
				return isDrawn;
			}
			set {
				if (isDrawn != value) {
					isDrawn = value;
				}
			}
		}

		[DefaultValue(null)]
		public Image Image {
			get {
				return image;
			}
			set {
				image = value;
			}
		}

		[DefaultValue(true)]
		public bool CanClose {
			get {
				return canClose;
			}
			set {
				canClose = value;
			}
		}

		[DefaultValue("Name")]
		public string Title {
			get {
				return title;
			}
			set {
				if (!(title == value)) {
					title = value;
					OnChanged();
				}
			}
		}

		[DefaultValue(false)]
		[Browsable(false)]
		public bool Selected {
			get {
				return selected;
			}
			set {
				if (selected != value) {
					selected = value;
				}
			}
		}

		[Browsable(false)]
		public string Caption => Title;

		public event EventHandler Changed;

		public BrowserTabStripItem()
			: this(string.Empty, null) {
		}

		public BrowserTabStripItem(Control displayControl)
			: this(string.Empty, displayControl) {
		}

		public BrowserTabStripItem(string caption, Control displayControl) {
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.ContainerControl, value: true);
			selected = false;
			Visible = true;
			UpdateText(caption, displayControl);
			if (displayControl != null) {
				base.Controls.Add(displayControl);
			}
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			if (disposing && image != null) {
				image.Dispose();
			}
		}

		public bool ShouldSerializeIsDrawn() {
			return false;
		}

		public bool ShouldSerializeDock() {
			return false;
		}

		public bool ShouldSerializeControls() {
			if (base.Controls != null) {
				return base.Controls.Count > 0;
			}
			return false;
		}

		public bool ShouldSerializeVisible() {
			return true;
		}

		private void UpdateText(string caption, Control displayControl) {
			if (caption.Length <= 0 && displayControl != null) {
				Title = displayControl.Text;
			}
			else if (caption != null) {
				Title = caption;
			}
			else {
				Title = string.Empty;
			}
		}

		public void Assign(BrowserTabStripItem item) {
			Visible = item.Visible;
			Text = item.Text;
			CanClose = item.CanClose;
			base.Tag = item.Tag;
		}

		protected internal virtual void OnChanged() {
			if (this.Changed != null) {
				this.Changed(this, EventArgs.Empty);
			}
		}

		public override string ToString() {
			return Caption;
		}
	}
}