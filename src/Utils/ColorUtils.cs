using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpBrowser.Utils {
	public static class ColorUtils {
		/// <summary>
		/// Creates color with corrected brightness.
		/// </summary>
		/// <param name="color">Color to correct.</param>
		/// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
		/// Negative values produce darker colors.</param>
		/// <returns>
		/// Corrected <see cref="Color"/> structure.
		/// </returns>
		public static Color ChangeColorBrightness(this Color color, float correctionFactor) {
			correctionFactor = Math.Clamp(correctionFactor, -1, 1);

			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0) {
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else {
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}

		/// <summary>
		/// Creates color with corrected brightness.
		/// </summary>
		/// <param name="color">Color to correct.</param>
		/// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
		/// Negative values produce darker colors.</param>
		/// <returns>
		/// Corrected <see cref="Color"/> structure.
		/// </returns>
		public static Color ChangeColorBrightness(this Color color, double correctionFactor)
			=> ChangeColorBrightness(color, (float)correctionFactor);


	}
}