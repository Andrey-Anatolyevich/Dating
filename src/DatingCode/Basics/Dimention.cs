using System;
using System.Diagnostics;
using System.Drawing;

namespace DatingCode.Basics
{
    [DebuggerDisplay("IsRaw: {IsRaw} Height: {Height} Width: {Width}")]
    public class Dimention
    {
        public Dimention(int width, int height)
        {
            if (height <= 0)
                throw new ArgumentException($"{height} <= 0", nameof(height));
            if (width <= 0)
                throw new ArgumentException($"{width} <= 0", nameof(width));


            Height = height;
            Width = width;
        }

        public Dimention(int width, int height, bool isRaw)
            : this(width: width, height: height)
        {
            IsRaw = isRaw;
        }

        public int Height;
        public int Width;
        public bool IsRaw;

        public double GetRatio()
        {
            var result = Convert.ToDouble(Height) / Convert.ToDouble(Width);
            return result;
        }

        public Dimention GetSizeToFitInto(Dimention targetSize)
        {
            if (targetSize == null)
                throw new ArgumentNullException(nameof(targetSize));


            var thisRatio = GetRatio();
            var targetRatio = targetSize.GetRatio();
            int fitHeight, fitWidth;

            // This is higher than MaxSize
            if (thisRatio > targetRatio)
            {
                fitHeight = targetSize.Height;
                fitWidth = Convert.ToInt32(fitHeight / thisRatio);
            }
            // This is wider than MaxSize
            else
            {
                fitWidth = targetSize.Width;
                fitHeight = Convert.ToInt32(fitWidth * thisRatio);
            }
            var result = new Dimention(height: fitHeight, width: fitWidth);
            return result;
        }

        // if this:     W:100 H:300
        // targetSize:  W: 80 H: 80
        // Returns :    x: 10 y: 110 W: 80 H: 80 => Rectangle
        public Rectangle GetCenterRectangleToFill(Dimention targetSize)
        {
            var thisRatio = GetRatio();
            var targetRatio = targetSize.GetRatio();

            int x, y;

            var scaledToThisTargetSize = targetSize.GetSizeToFitInto(this);

            // This is higher than MaxSize
            if (thisRatio > targetRatio)
            {
                x = 0;
                y = (Height - scaledToThisTargetSize.Height) / 2;
            }
            // This is wider than MaxSize
            else
            {
                x = (Width - scaledToThisTargetSize.Width) / 2;
                y = 0;
            }

            return new Rectangle(x: x, y: y, width: scaledToThisTargetSize.Width, height: scaledToThisTargetSize.Height);
        }
    }
}
