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
            float width, float height, float radius)
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
            float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = rectangle.GetRoundedRect(radius);
            g.DrawPath(pen, path);
        }


        /// <summary>
        /// Get the desired Rounded Rectangle path. 
        /// </summary>
        private static GraphicsPath GetRoundedRect(this RectangleF baseRect, float radius)
        {
            // if corner radius is less than or equal to zero, 
            // return the original rectangle 
            if (radius <= 0.0F)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner radius is greater than or equal to 
            // half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 
            if (radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
                return GetCapsule(baseRect);

            // create the arc for the rectangle sides and declare 
            // a graphics path object for the drawing 
            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }


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
        public static GraphicsPath CreateTabPath_roundAll(this  RectangleF tabRect, float cornerRadius)
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

        public static GraphicsPath CreateTabPath_Roundtop_RoundBottomOut(this RectangleF tabRect, float cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (cornerRadius <= 0)
            {
                path.AddRectangle(tabRect);
                return path;
            }

            float diameter = cornerRadius * 2;
            RectangleF arcRectTop = new RectangleF(tabRect.Location, new SizeF(diameter, diameter));
            RectangleF arcRectBottom = new RectangleF(tabRect.Left, tabRect.Bottom - diameter, diameter, diameter); // For bottom round out

            // Top-Left Arc (Inward Round - same as roundTop)
            path.AddArc(arcRectTop, 180, 90);

            // Top Line
            PointF topRightCornerStart = new PointF(tabRect.Left + cornerRadius, tabRect.Top);
            PointF topRightCornerEnd = new PointF(tabRect.Right - cornerRadius, tabRect.Top);
            path.AddLine(topRightCornerStart, topRightCornerEnd);

            // Top-Right Arc (Inward Round - same as roundTop)
            arcRectTop.X = tabRect.Right - diameter;
            path.AddArc(arcRectTop, 270, 90);

            // Right Line (Connect top-right arc to bottom-right arc)
            path.AddLine(new PointF(tabRect.Right, tabRect.Top + cornerRadius), new PointF(tabRect.Right, tabRect.Bottom - cornerRadius));

            // Bottom-Right Arc (Outward Round - like roundAll)
            arcRectBottom.X = tabRect.Right - diameter;
            path.AddArc(arcRectBottom, 0, 90); // Start at 0 degrees (right), sweep 90 degrees clockwise (outward)


            // Bottom Line (Not needed, arc goes to bottom-left corner)

            // Bottom-Left Arc (Outward Round - like roundAll)
            arcRectBottom.X = tabRect.Left; // X is already set
            path.AddArc(arcRectBottom, 90, 90); // Start at 90 degrees (bottom), sweep 90 degrees clockwise (outward)


            // Left Line (Connect bottom-left arc to top-left arc)
            path.AddLine(new PointF(tabRect.Left, tabRect.Bottom - cornerRadius), new PointF(tabRect.Left, tabRect.Top + cornerRadius));


            path.CloseFigure();

            return path;
        }



        /// <summary>
        /// Gets the desired Capsular path. 
        /// </summary>
        private static GraphicsPath GetCapsule(this RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule 

                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule 

                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle 

                    path.AddEllipse(baseRect);
                }
            }
            catch (Exception ex)
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        }
        
    }
}