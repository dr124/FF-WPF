using System;
using System.Drawing;
using System.Drawing.Imaging;
using FF_WPF.ViewModels;

namespace FF_WPF.Filters
{
    public abstract class ImageFilter
    {
        public abstract Bitmap Filter(Bitmap image, FilterParams param);

        protected byte GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                default:
                    throw new ArgumentException("Only 24 and 32 bit images are supported");
            }
        }
    }
}