using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Drawing
{

    public static partial class GraphicsEx
    {
        /// <summary>
        /// Fills a Rounded Rectangle with integers. 
        public static void FillRoundRectangle(this Graphics g, Brush brush, RectangleF rect, int radius)
            => g.FillRoundRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, radius);
        /// <summary>
        /// Fills a Rounded Rectangle with integers. 
        public static void FillRoundRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
            => g.FillRoundRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// Fills a Rounded Rectangle with continuous numbers.
        /// </summary>
        public static void FillRoundRectangle(this Graphics g, Brush brush, float x, float y,
            float width, float height, int radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = rectangle.GetRoundedRect(radius);
            g.FillPath(brush, path);
        }

        /// <summary>
        /// Draws a Rounded Rectangle border with integers. 
        /// </summary>
        public static void DrawRoundRectangle(this Graphics g, Pen pen, RectangleF rect, int radius)
            => g.DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, radius);
        /// <summary>
        /// Draws a Rounded Rectangle border with integers. 
        /// </summary>
        public static void DrawRoundRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
            => g.DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// Draws a Rounded Rectangle border with continuous numbers. 
        /// </summary>
        public static void DrawRoundRectangle(this Graphics g, Pen pen, float x, float y,
            float width, float height, int radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = rectangle.GetRoundedRect(radius);
            g.DrawPath(pen, path);
        }

        private static GraphicsPath GetRoundedRect(this RectangleF bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            RectangleF arc = new RectangleF(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }


    public static partial class GraphicsEx
    {
        public static GraphicsPath CreateTabPath_roundTop(this RectangleF tabRect, float cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (cornerRadius <= 0)
            {
                path.AddRectangle(tabRect); // If no corner radius, just a rectangle
                return path;
            }

            float diameter = cornerRadius * 2;
            RectangleF arcRect = new RectangleF(tabRect.Location, new SizeF(diameter, diameter));

            // Top-Left Arc
            path.AddArc(arcRect, 180, 90);

            // Top Line
            PointF topRightCornerStart = new PointF(tabRect.Left + cornerRadius, tabRect.Top);
            PointF topRightCornerEnd = new PointF(tabRect.Right - cornerRadius, tabRect.Top);
            path.AddLine(topRightCornerStart, topRightCornerEnd);

            // Top-Right Arc
            arcRect.X = tabRect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // Right Line
            path.AddLine(new PointF(tabRect.Right, tabRect.Top + cornerRadius), new PointF(tabRect.Right, tabRect.Bottom));

            // Bottom Line
            path.AddLine(new PointF(tabRect.Right, tabRect.Bottom), new PointF(tabRect.Left, tabRect.Bottom));

            // Left Line - Closing the path
            path.AddLine(new PointF(tabRect.Left, tabRect.Bottom), new PointF(tabRect.Left, tabRect.Top + cornerRadius));

            path.CloseFigure();

            return path;
        }
        public static GraphicsPath CreateTabPath_roundAll(this RectangleF tabRect, float cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (cornerRadius <= 0)
            {
                path.AddRectangle(tabRect);
                return path;
            }

            float diameter = cornerRadius * 2;
            RectangleF arcRect = new RectangleF(tabRect.Location, new SizeF(diameter, diameter));

            // Top-Left Arc
            path.AddArc(arcRect, 180, 90);

            // Top Line
            PointF topRightCornerStart = new PointF(tabRect.Left + cornerRadius, tabRect.Top);
            PointF topRightCornerEnd = new PointF(tabRect.Right - cornerRadius, tabRect.Top);
            path.AddLine(topRightCornerStart, topRightCornerEnd);

            // Top-Right Arc
            arcRect.X = tabRect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // Right Line
            path.AddLine(new PointF(tabRect.Right, tabRect.Top + cornerRadius), new PointF(tabRect.Right, tabRect.Bottom - cornerRadius));

            // Bottom-Right Arc (Bottom Round to Out - now Round All)
            arcRect.Y = tabRect.Bottom - diameter; // Move arcRect to bottom-right corner
            path.AddArc(arcRect, 0, 90); // Start at 0 degrees (right), sweep 90 degrees clockwise

            // Bottom Line (Not needed, arc goes to bottom-left corner)

            // Bottom-Left Arc (Bottom Round to Out - now Round All)
            arcRect.X = tabRect.Left; // Move arcRect to bottom-left corner (X is already set to 0)
            path.AddArc(arcRect, 90, 90); // Start at 90 degrees (bottom), sweep 90 degrees clockwise

            // Left Line - Closing the path
            path.AddLine(new PointF(tabRect.Left, tabRect.Bottom - cornerRadius), new PointF(tabRect.Left, tabRect.Top + cornerRadius));


            path.CloseFigure();

            return path;
        }


        public static GraphicsPath CreateTabPath_Active(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            // Ensure radius is not larger than half the width or height
            radius = Math.Min(radius, Math.Min(rect.Width / 2, rect.Height / 2));

            //// Bottom-left arc
            path.AddArc(rect.X - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, -90);
            //// Top-left arc
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            // Top-right arc
            path.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            // Bottom-right arc
            path.AddArc(rect.X + rect.Width, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, -90 * 2, -90);

            //path.CloseFigure(); // Close the path to connect the last and first points
            return path;
        }
        public static GraphicsPath CreateTabPath_Roundtop_RoundBottomOut(this RectangleF tabRect, float cornerRadius)
            => CreateTabPath_Active(Rectangle.Round(tabRect), (int)cornerRadius);




    }


    public static class ImageExt
    {
        /// <summary>
        ///    btn.BackgroundImage Has no Disabled Effect . wee need lighter
        /// </summary>
        /// <param name="imgLight"></param>
        /// <param name="level">0 to 100</param>
        /// <param name="nRed"></param>
        /// <param name="nGreen"></param>
        /// <param name="nBlue"></param>
        /// <returns></returns>
        public static Image Lighter(this Image imgLight, int level, int nRed, int nGreen, int nBlue)
        {
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
        public static Image Lighter(this Image imgLight, int level, Color col ) 
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
        public static Image Lighter(this Image imgLight, int level = 90)
        {
            var col = Color.White;
            return imgLight.Lighter(level, col.R, col.G, col.B);
        }
    }

    public static class ColorExt 
    {
        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(this Color color, float correctionFactor)
        {
            correctionFactor = Math.Clamp(correctionFactor,-1,1);

            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        public static Color ChangeColorBrightness(this Color color, double correctionFactor)
            => ChangeColorBrightness(color, (float)correctionFactor);


    }

}