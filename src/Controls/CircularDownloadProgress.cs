using SharpBrowser.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace SharpBrowser.Controls
{

    /// <summary>
    /// Draws Download Progress as Circle    to Given Button..  
    /// </summary>
    public  class CircularDownloadProgress
    {

        public CircularDownloadProgress(Button btnDL)
        {

            init_downloads_indicator(btnDL);
        }
        //------ draw Circular downloading Progress 
        
        Button _btnDL;
        public void init_downloads_indicator(Button btnDL)
        {
            _btnDL = btnDL;
            btnDL.Paint += btnDL_Paint;
            var tmr_downloader = new System.Windows.Forms.Timer();
            tmr_downloader.Interval = 500;
            tmr_downloader.Tick += Tmr_downloader_Tick;
            tmr_downloader.Start();
        }

        int testdl_pct = 0;
        private void Tmr_downloader_Tick(object sender, EventArgs e)
        {
            _btnDL.Refresh();
            //BtnDownloads.Invalidate();


            testdl_pct = testdl_pct + 10;
            if (testdl_pct > 100)
                testdl_pct = 0;
        }

        private void btnDL_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var isDownloading = DownloadManager.DownloadsInProgress();
                float pct_ofRecentDownloadingItem = 0;
                if (isDownloading)
                {
                    var curdlitemKV = DownloadManager.Downloads.Where(x => x.Value.IsInProgress).FirstOrDefault();
                    var curdlitem = curdlitemKV.Value;
                    pct_ofRecentDownloadingItem = (int)(curdlitem.ReceivedBytes * 100.0f / curdlitem.TotalBytes);
                }

                //var isDownloading = true;
                if (isDownloading)
                {
                    //var pct1 = 100; // 100 %;
                    //var pct2 = 50; // 50 %;
                    //var pct3 =25; // 20 %;
                    var pct = testdl_pct; //test val;  //  <<<<----  input download percentage HERE;;
                    pct = (int)pct_ofRecentDownloadingItem;

                    var pctAs360val = pct / 100.0f * 360;
                    var arc_StartOffset = 90;

                    //Color activeColorORG = Color.FromArgb(11, 87, 208);
                    //Color activeLightColor = Color.FromArgb(76, 194, 255);
                    Color activeColor = Color.FromArgb(27, 117, 208);
                    int gray = 200;
                    var myGray = Color.FromArgb(gray, gray, gray);

                    //var loc = BtnDownloads.Location;
                    //var sz = BtnDownloads.Size;
                    var loc = new Point(0, 0);
                    var sz = _btnDL.ClientRectangle;
                    var pad = 0;
                    //var btng = BtnDownloads.CreateGraphics();

                    var thickness = 4;

                    var btng = e.Graphics;
                    //var btng = BtnDownloads.CreateGraphics();
                    btng.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    Rectangle rect = new Rectangle(
                        loc.X + pad + thickness / 2,
                        loc.Y + pad + thickness / 2,
                        sz.Width - 1 * pad - thickness / 2 * 2 - 1,
                        sz.Height - 1 * pad - thickness / 2 * 2 - 1);
                    //btng.FillRectangle(new Pen(new SolidBrush(Color.Black), 10).Brush, rect);
                    btng.DrawArc(new Pen(new SolidBrush(myGray), thickness), rect, 0 + arc_StartOffset, 360);
                    btng.DrawArc(new Pen(new SolidBrush(activeColor), thickness), rect, 0 + arc_StartOffset, pctAs360val);

                }
            }
            catch (Exception ex)
            {
                var x = 123;
            }
        }



    }
}
