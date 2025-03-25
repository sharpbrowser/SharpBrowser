using System;
using System.Drawing;
using System.Drawing.Drawing2D;

// A simple extension to the Graphics class for extended 
// graphic routines, such, 
// as for creating rounded rectangles. 
// Because, Graphics class is an abstract class, 
// that is why it can not be inherited. Although, 
// I have provided a simple constructor 
// that builds the ExtendedGraphics object around a 
// previously created Graphics object. 
// Please contact: aaronreginald@yahoo.com for the most 
// recent implementations of
// this class. 
namespace System.Drawing
{
    /// <SUMMARY> 
    /// Inherited child for the class Graphics encapsulating 
    /// additional functionality for curves and rounded rectangles. 
    /// </SUMMARY> 

    public static partial class GraphicsEx
    {
        /// <summary>
        /// Fills a Rounded Rectangle with integers. 
        public static void FillRoundRectangle(this Graphics g, Brush brush, RectangleF rect, int radius)
            => g.FillRoundRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, radius);
        /// <summary>
        /// Fills a Rounded Rectangle with integers. 
        public static void FillRoundRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
            => g.FillRoundRectangle( brush, rect.X, rect.Y, rect.Width, rect.Height, radius);

        /// <summary>
        /// Fills a Rounded Rectangle with continuous numbers.
        /// </summary>
        public static void FillRoundRectangle(this Graphics g,  Brush brush, float x, float y,
            float width, float height, int radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = rectangle.GetRoundedRect( radius);
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



}