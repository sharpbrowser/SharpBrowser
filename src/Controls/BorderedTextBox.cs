using SharpBrowser.Config;
using SharpBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBrowser.Controls {
	public static class BorderedTextBoxUtils {

		/// <summary>
		/// Wraps an exisiting textbox with a panel which has roundd borders and a custom backcolor.
		/// Returns the panel which wraps the textbox.
		/// </summary>
		public static BorderedTextBox ToBordered(this TextBox tbx) {
			
			//backup previous TBX status
			var tbxOriginalParent = tbx.Parent;
			var loc = tbx.Location;
			var size = tbx.Size;
			var anchor = tbx.Anchor;
			var dock = tbx.Dock;


			//restore them to Panel
			var panel = new BorderedTextBox(tbx);
			panel.Location = loc;
			panel.Size = size;
			//bordered_tbx.Height= size.Height+5;
			panel.Anchor = anchor;
			panel.Dock = dock;
			tbxOriginalParent.Controls.Add(panel);


			panel.BackColor = Color.Transparent;

			//remove tbx min size. it causes tbx getting clipped.
			if (tbx.MinimumSize.Height != 0 && tbx.MinimumSize.Height >= panel.MinimumSize.Height) {
				tbx.MinimumSize = new Size();
			}


			return panel;
		}
	}

	public class BorderedTextBox : Panel {
		private TextBox textBox;
		private bool focusedAlways = false;
		private Color normalBorderColor = Color.LightGray;
		private Color focusedBorderColor = Color.FromArgb(153, 187, 239); //edge light blue
		public int borderThickness = 2;

		public TextBox TextBox {
			get { return textBox; }
			//set { textBox = value; }
		}
		public bool FocusedAlways {
			get { return focusedAlways; }
			set { focusedAlways = value; }
		}

		public BorderedTextBox(TextBox tbx = null) {
			this.DoubleBuffered = true;
			this.Padding = new Padding(1 + borderThickness * 2);
			this.Height += borderThickness * 2;

			if (tbx == null)
				textBox = new TextBox();
			else
				textBox = tbx;

			this.TextBox.AutoSize = false;
			this.TextBox.BorderStyle = BorderStyle.None;
			this.TextBox.Dock = DockStyle.Fill;
			this.TextBox.Enter += TextBox_Refresh;
			this.TextBox.Leave += TextBox_Refresh;
			this.TextBox.Resize += TextBox_Refresh;
			this.Controls.Add(this.TextBox);

		}



		private void TextBox_Refresh(object sender, EventArgs e) => this.Invalidate();

		protected override void OnPaint(PaintEventArgs e) {
			this.Padding = new Padding(
				left: 15 + borderThickness,
				right: 15 + borderThickness,
				top: 1 + borderThickness + 5,
				bottom: borderThickness + 5
				);

			this.AutoSize = false;
			var txtHeight = MeasureHeight(textBox);
			this.Height = txtHeight
				+ Padding.Top + Padding.Bottom;


			var color = this.TextBox.Focused || focusedAlways ? focusedBorderColor : normalBorderColor;
			using (Pen borderPen = new Pen(color, borderThickness)) {
				var brushbg = new SolidBrush(textBox.BackColor);

				var w = this.ClientSize.Width - borderThickness * 2;
				var h = this.ClientSize.Height - borderThickness * 2;
				var radius = h / 2;

				e.Graphics.FillRoundRectangle(brushbg,
					new Rectangle(0 + borderThickness, 0 + borderThickness,w,h),radius);

				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

				e.Graphics.DrawRoundRectangle(borderPen,
					new Rectangle(0 + borderThickness, 0 + borderThickness,w,h),radius);
			}
			base.OnPaint(e);
		}


		static int MeasureHeight(TextBox textbox) {
			Size size = TextRenderer.MeasureText("AĞÜüğGgpPa", textbox.Font, Size.Empty, TextFormatFlags.TextBoxControl);
			//textbox.MinimumSize = new Size(0, s.Height + 1 + 3);
			return size.Height;

		}


	}



}