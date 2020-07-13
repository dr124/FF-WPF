using System.Drawing;
using System.Drawing.Imaging;

namespace FF_WPF.Filters.Implementations
{
    public class GaussianBlurFilter : ImageFilter
    {
        public override Bitmap Filter(Bitmap image, FilterParams param)
        {
            var gaussParams = (GaussianBlurParams) param;

            var inputImage = new Bitmap(image);
            var outputImage = new Bitmap(image);
            
            var outBitmapData = outputImage.LockBits(new Rectangle(0, 0, outputImage.Width, outputImage.Height),
                ImageLockMode.WriteOnly, outputImage.PixelFormat);
            var inBitmapData = inputImage.LockBits(new Rectangle(0, 0, outputImage.Width, outputImage.Height),
                ImageLockMode.ReadWrite, outputImage.PixelFormat);
            
            var bitsPerPixel = GetBitsPerPixel(inBitmapData.PixelFormat);

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
                    var currPixel = outScan0 + i * outBitmapData.Stride + j * bitsPerPixel / 8;

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
                            var kernelPixel = inScan0 + ii * inBitmapData.Stride + (j + kj) * bitsPerPixel / 8;
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