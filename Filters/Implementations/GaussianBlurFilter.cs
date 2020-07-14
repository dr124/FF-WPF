using System.Drawing;
using System.Drawing.Imaging;

namespace FF_WPF.Filters.Implementations
{
    public class GaussianBlurFilter : ImageFilter
    {
        protected override Bitmap ProcessImage(Bitmap image, FilterParams param)
        {
            var gaussParams = (GaussianBlurParams) param;

            var (outputImage, outBitmapData) = CreateImage(image, ImageLockMode.WriteOnly);
            var (inputImage, inBitmapData) = CreateImage(image, ImageLockMode.ReadOnly);
            var channels = GetBitsPerPixel(inBitmapData.PixelFormat) / 8;

            var kernel = gaussParams.Kernel;
            var kernelRadius = gaussParams.Kernel.Length / 2;

            unsafe
            {
                var inScan0 = (byte*) inBitmapData.Scan0.ToPointer();
                var outScan0 = (byte*)outBitmapData.Scan0.ToPointer();

                //for every pixel
                for (var i = 0; i < inBitmapData.Height; ++i)
                for (var j = 0; j < inBitmapData.Width; ++j)
                {
                    var currPixel = GetPixelPointer(outScan0, i, j, outBitmapData.Stride, channels);

                    //for every channel
                    for (var c = 0; c < 3; c++)
                    {
                        var pixelSum = 0f;
                        var kernelSum = 0f;

                        //for every element in kernel
                        for (var ki = -kernelRadius; ki <= kernelRadius; ki++)
                        for (var kj = -kernelRadius; kj <= kernelRadius; kj++)
                        {
                            var ii = i + ki;
                            var jj = j + kj;
                            if (ii < 0 || ii >= inBitmapData.Height || jj < 0 || jj >= inBitmapData.Width) 
                                continue;
                            var kernelPixel = GetPixelPointer(inScan0, ii, jj, inBitmapData.Stride, channels);
                            var k = kernel[ki + kernelRadius][kj + kernelRadius];
                            kernelSum += k;
                            pixelSum += kernelPixel[c] * k;
                        }

                        //todo: if kernelsum is 0 then normalize
                        if (kernelSum == 0) kernelSum = 1; //if kernel == 0 (for eg. edge detection)
                        currPixel[c] = (byte) (pixelSum / kernelSum);
                    }
                }
            }

            outputImage.UnlockBits(outBitmapData);
            inputImage.UnlockBits(inBitmapData);

            return outputImage;
        }
    }
}