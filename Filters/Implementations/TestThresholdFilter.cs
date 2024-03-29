﻿using System.Drawing;
using System.Drawing.Imaging;

namespace FF.WPF.Filters.Implementations;

/// <summary>
///     This class was made to test if everything's alright. Like a ping->return pong or foo->return bar code
/// </summary>
public class TestThresholdFilter : ImageFilter
{
    protected override Bitmap ProcessImage(Bitmap image, FilterParams param, CancellationToken ct)
    {
        var thrParams = (TestThresholdParams) param;
        var (outputImage, bitmapData) = CreateImage(image, ImageLockMode.ReadWrite);
        var channels = GetBitsPerPixel(bitmapData.PixelFormat) / 8;

        unsafe
        {
            var scan0 = (byte*) bitmapData.Scan0.ToPointer();

            for (var i = 0; i < bitmapData.Height; ++i)
            for (var j = 0; j < bitmapData.Width; ++j)
            {
                ct.ThrowIfCancellationRequested();

                var pixel = GetPixelPointer(scan0, i, j, bitmapData.Stride, channels);

                if (Magnitude(pixel) / 255f < thrParams.Ratio)
                    pixel[0] = pixel[1] = pixel[2] = 0;
                else
                    pixel[0] = pixel[1] = pixel[2] = 255;
            }
        }

        outputImage.UnlockBits(bitmapData);

        return outputImage;
    }
}