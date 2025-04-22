using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpBrowser.Utils {
	public static class ImageUtils {
		/// <summary>
		///    btn.BackgroundImage Has no Disabled Effect . wee need lighter
		/// </summary>
		/// <param name="imgLight"></param>
		/// <param name="level">0 to 100</param>
		/// <param name="nRed"></param>
		/// <param name="nGreen"></param>
		/// <param name="nBlue"></param>
		/// <returns></returns>
		public static Image Lighter(this Image imgLight, int level, int nRed, int nGreen, int nBlue) {
			//convert image to graphics object
			Graphics graphics = Graphics.FromImage(imgLight);
			int conversion = (5 * (level - 50)); //calculate new alpha value
												 //create mask with blended alpha value and chosen color as pen 
			Pen pLight = new Pen(Color.FromArgb(conversion, nRed, nGreen, nBlue), imgLight.Width * 2);
			//apply created mask to graphics object
			graphics.DrawLine(pLight, -1, -1, imgLight.Width, imgLight.Height);
			//save created graphics object and modify image object by that
			graphics.Save();
			graphics.Dispose(); //dispose graphics object
			return imgLight; //return modified image
		}

		/// <summary>
		/// recommended defaults  level:90, color:Color.white
		/// 90 level - fade effect is instense,
		/// <para/> 10 level - has nearly no effect;
		/// </summary>
		/// <param name="imgLight"></param>
		/// <param name="level"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public static Image Lighter(this Image imgLight, int level, Color col)
			=> imgLight.Lighter(level, col.R, col.G, col.B);

		/// <summary>
		/// recommended defaults  level:90, color:Color.white
		/// 90 level - fade effect is instense,
		/// <para/> 10 level - has nearly no effect;
		/// </summary>
		/// <param name="imgLight"></param>
		/// <param name="level"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public static Image Lighter(this Image imgLight, int level = 90) {
			var col = Color.White;
			return imgLight.Lighter(level, col.R, col.G, col.B);
		}
	}
}