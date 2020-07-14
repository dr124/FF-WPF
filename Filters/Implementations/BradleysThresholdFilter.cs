using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace FF_WPF.Filters.Implementations
{
    /// <summary>
    ///     Bradley's Adaptive Threshold filter implementation based on
    ///     http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.420.7883&rep=rep1&type=pdf
    /// </summary>
    public class BradleysThresholdFilter : ImageFilter
    {
        protected override Bitmap ProcessImage(Bitmap image, FilterParams param, CancellationToken ct)
        {
            var thrParams = (BradleysThresholdParams) param;
            var (outputImage, bitmapData) = CreateImage(image, ImageLockMode.ReadWrite);
            var channels = GetBitsPerPixel(bitmapData.PixelFormat) / 8;

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
                        ct.ThrowIfCancellationRequested();

                        var pixel = GetPixelPointer(scan0, i, j, bitmapData.Stride, channels);
                        sum += Magnitude(pixel);

                        if (i == 0) intImg[i][j] = sum;
                        else intImg[i][j] = intImg[i - 1][j] + sum;
                    }
                }

                var s = thrParams.S;
                var t = thrParams.T;

                for (var i = 0; i < bitmapData.Height; ++i)
                for (var j = 0; j < bitmapData.Width; ++j)
                {
                    ct.ThrowIfCancellationRequested();

                    var x1 = Between(1, i - s / 2, bitmapData.Height - 1);
                    var x2 = Between(1, i + s / 2, bitmapData.Height - 1);
                    var y1 = Between(1, j - s / 2, bitmapData.Width - 1);
                    var y2 = Between(1, j + s / 2, bitmapData.Width - 1);
                    var count = (x2 - x1) * (y2 - y1);
                    var sum = intImg[x2][y2] - intImg[x2][y1 - 1] - intImg[x1 - 1][y2] + intImg[x1 - 1][y1 - 1];

                    var pixel = GetPixelPointer(scan0, i, j, bitmapData.Stride, channels);

                    if (Magnitude(pixel) * count <= sum * (100 - t) / 100.0)
                        pixel[0] = pixel[1] = pixel[2] = 0;
                    else
                        pixel[0] = pixel[1] = pixel[2] = 255;
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