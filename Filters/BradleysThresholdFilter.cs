using System.Drawing;
using System.Drawing.Imaging;
using FF_WPF.Models;
using FF_WPF.ViewModels;

namespace FF_WPF.Filters
{
    /// <summary>
    /// Bradley's Adaptive Threshold filter implementation based on
    /// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.420.7883&rep=rep1&type=pdf
    /// </summary>
    public class BradleysThresholdFilter : FilterParamConsumer
    {
        public override Bitmap Filter(Bitmap image, FilterParams param)
        {
            var thrParams = (BradleysThresholdParams) param;
            var outputImage = new Bitmap(image);
            var bitmapData = outputImage.LockBits(new Rectangle(0, 0, outputImage.Width, outputImage.Height),
                ImageLockMode.ReadWrite, outputImage.PixelFormat);
            var bitsPerPixel = GetBitsPerPixel(bitmapData.PixelFormat);
            
            unsafe
            {
                var scan0 = (byte*) bitmapData.Scan0.ToPointer();

                var intImg = new int[bitmapData.Height][];
                for (var i = 0; i < bitmapData.Height; ++i)
                {
                    var sum = 0;
                    intImg[i] = new int[bitmapData.Width];

                    for (var j = 0; j < bitmapData.Width; ++j)
                    {
                        var data = scan0 + i * bitmapData.Stride + j * bitsPerPixel / 8;

                        var magnitude = (data[0] + data[1] + data[2]) / 3;

                        sum += magnitude;

                        if (i == 0) intImg[i][j] = sum;
                        else intImg[i][j] = intImg[i - 1][j] + sum;
                    }
                }

                var s = thrParams.S;
                var t = thrParams.T;

                for (var i = 0; i < bitmapData.Height; ++i)
                for (var j = 0; j < bitmapData.Width; ++j)
                {
                    var x1 = Between(1, i - s / 2, bitmapData.Height - 1);
                    var x2 = Between(1, i + s / 2, bitmapData.Height - 1);
                    var y1 = Between(1, j - s / 2, bitmapData.Width - 1);
                    var y2 = Between(1, j + s / 2, bitmapData.Width - 1);
                    var count = (x2 - x1) * (y2 - y1);
                    var sum = intImg[x2][y2] - intImg[x2][y1 - 1] - intImg[x1 - 1][y2] + intImg[x1 - 1][y1 - 1];

                    var data = scan0 + i * bitmapData.Stride + j * bitsPerPixel / 8;
                    var magnitude = (data[0] + data[1] + data[2]) / 3;

                    if (magnitude * count <= sum * (100 - t) / 100.0)
                        data[0] = data[1] = data[2] = 0;
                    else
                        data[0] = data[1] = data[2] = 255;
                }
            }

            outputImage.UnlockBits(bitmapData);

            return outputImage;
        }

        private int Between(int min, int val, int max)
        {
            return val < min ? min : val > max ? max : val;
        }
    }
}