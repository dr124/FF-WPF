using System.IO;
using System.Windows.Media.Imaging;

namespace FF_WPF.Utils
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this BitmapSource bitmap)
        {
            var encoder = new PngBitmapEncoder(); // or any other encoder
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static BitmapImage ToBitmapImage(this byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}