using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FF.WPF.Utils;

public static class ImageExtensions
{
    private static readonly Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;

    private static bool ThumbnailCallback()
    {
        return false;
    }

    public static Bitmap ResizeImage(this Bitmap bitmap, float scale)
    {
        return (Bitmap) bitmap.GetThumbnailImage(
            (int) (bitmap.Width * scale),
            (int) (bitmap.Height * scale),
            myCallback, IntPtr.Zero);
    }

    public static BitmapSource ToImageSource(this Bitmap bitmap)
    {
        var xd = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
            Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        xd.Freeze();
        return xd;
    }
}