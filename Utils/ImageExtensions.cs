using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FF_WPF.Utils
{
    public static class ImageExtensions
    {
        public static ImageSource ToImageSource(this Bitmap bitmap)
        {

            var xd = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            xd.Freeze();
            return xd;
        }
    }
}