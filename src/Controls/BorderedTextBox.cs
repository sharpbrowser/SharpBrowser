using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBrowser.Controls
{
    public static class TextBox_Helper
    {
        /// <summary>
        /// wrap your exisiting textbox, into a panel with customColor Roudned borders
        /// </summary>
        /// <returns>retunrs  BorderedTextbox ,  aka The Panel That Wraps The your Exisiting Textbox</returns>
        public static BorderedTextBox MakeTextbox_CustomBorderColor(this TextBox tbx) 
        {
            //backup previous TBX status
            var tbxOriginalParent =  tbx.Parent;
            var loc = tbx.Location;
            var size = tbx.Size;
            var anchor = tbx.Anchor;
            var dock = tbx.Dock;

            
            //restore them to Panel
            var bordered_tbx = new BorderedTextBox(tbx);
            bordered_tbx.Location = loc;
            bordered_tbx.Size= size;
            //bordered_tbx.Height= size.Height+5;
            bordered_tbx.Anchor = anchor;
            bordered_tbx.Dock = dock;
            tbxOriginalParent.Controls.Add(bordered_tbx);

            //remove tbx min size. it causes tbx getting clipped.
            if (tbx.MinimumSize.Height != 0 && tbx.MinimumSize.Height >= bordered_tbx.MinimumSize.Height)
            {
                tbx.MinimumSize = new Size();
            }
                

            return bordered_tbx;
        }
    }

    // https://stackoverflow.com/questions/17466067/change-border-color-in-textbox-c-sharp/39420512#39420512
    public class BorderedTextBox : Panel
    {
        private TextBox textBox;
        private bool focusedAlways = false;
        private Color normalBorderColor = Color.LightGray;
        private Color focusedBorderColor = Color.FromArgb(0,00,225);
        //private Color focusedBorderColor = Color.FromArgb(86,156,214);

       
        public TextBox TextBox
        {
            get { return textBox; }
            //set { textBox = value; }
        }
        public bool FocusedAlways
        {
            get { return focusedAlways; }
            set { focusedAlways = value; }
        }

        public BorderedTextBox(TextBox tbx = null)
        {
            this.DoubleBuffered = true;
            this.Padding = new Padding(2);

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

            //this.textBox.FontChanged += TextBox_FontChanged;
            //RefreshHeight(textBox);

            //debug helper
            //this.TextBox.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.Red;

        }

       

        private void TextBox_Refresh(object sender, EventArgs e) => this.Invalidate();

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Window);

            using (Pen borderPen = new Pen(this.TextBox.Focused || focusedAlways ?
                focusedBorderColor : normalBorderColor))
            {
                //e.Graphics.DrawRectangle(borderPen,
                e.Graphics.DrawRoundRectangle(borderPen,
                    new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1)
                    ,5);
            }
            base.OnPaint(e);
        }


        //private void TextBox_FontChanged(object sender, EventArgs e)
        //{
        //    RefreshHeight(textBox);
        //    this.Height = textBox.Height + 5;
        //}
        ///// <summary>
        ///// fix Textbox.borderNone bottom gets clipped issue
        ///// </summary>
        ///// <param name="textbox"></param>
        //static void RefreshHeight(TextBox textbox)
        //{
        //    return;

        //    textbox.Multiline = true;
        //    Size s = TextRenderer.MeasureText("AĞÜüğGgpPa", textbox.Font, Size.Empty, TextFormatFlags.TextBoxControl);
        //    textbox.MinimumSize = new Size(0, s.Height + 1 + 3);
        //    textbox.Multiline = false;
        //}


    }



}
