using System.Drawing;
using System.Drawing.Imaging;

namespace FF_WPF.Filters.Implementations
{
    /// <summary>
    /// This class was made to test if everything's alright. Like a ping->return pong or foo->return bar code
    /// </summary>
    public class TestThresholdFilter : ImageFilter
    {
        public override Bitmap Filter(Bitmap image, FilterParams param)
        {
            var thrParams = (TestThresholdParams) param;
            var outputImage = new Bitmap(image);
            var bitmapData = outputImage.LockBits(new Rectangle(0, 0, outputImage.Width, outputImage.Height),
                ImageLockMode.ReadWrite, outputImage.PixelFormat);
            var bitsPerPixel = GetBitsPerPixel(bitmapData.PixelFormat);

            unsafe
            {
                var scan0 = (byte*) bitmapData.Scan0.ToPointer();

                for (var i = 0; i < bitmapData.Height; ++i)
                for (var j = 0; j < bitmapData.Width; ++j)
                {
                    var data = scan0 + i * bitmapData.Stride + j * bitsPerPixel / 8;

                    var magnitude = (data[0] + data[1] + data[2]) / 3f / 255;
                    if (magnitude < thrParams.Ratio)
                        data[0] = data[1] = data[2] = 0;
                    else
                        data[0] = data[1] = data[2] = 255;
                }
            }

            outputImage.UnlockBits(bitmapData);

            return outputImage;
        }
    }
}