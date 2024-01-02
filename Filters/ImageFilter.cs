using System.Drawing;
using System.Drawing.Imaging;

namespace FF.WPF.Filters;

public abstract class ImageFilter
{
    public async Task<Bitmap> Filter(Bitmap image, FilterParams param, CancellationToken ct)
    {
        if (image == null || param == null)
            return image; //todo: throw ex

        return await Task.FromResult(ProcessImage(image, param, ct));
    }

    protected abstract Bitmap ProcessImage(Bitmap image, FilterParams param, CancellationToken ct);

    protected byte GetBitsPerPixel(PixelFormat pixelFormat)
    {
        return pixelFormat switch
        {
            PixelFormat.Format24bppRgb => 24,
            PixelFormat.Format32bppArgb => 32,
            PixelFormat.Format32bppPArgb => 32,
            PixelFormat.Format32bppRgb => 32,
            _ => throw new ArgumentException("Only 24 and 32 bit images are supported")
        };
    }

    protected BitmapData LockBits(Bitmap image, ImageLockMode lockMode)
    {
        var rect = new Rectangle(0, 0, image.Width, image.Height);
        return image.LockBits(rect, lockMode, image.PixelFormat);
    }

    protected (Bitmap, BitmapData) CreateImage(Bitmap image, ImageLockMode lockMode)
    {
        var newImage = new Bitmap(image);
        return (newImage, LockBits(newImage, lockMode));
    }

    protected unsafe int Magnitude(byte* pixel)
    {
        return (pixel[0] + pixel[1] + pixel[2]) / 3;
    }

    protected unsafe byte* GetPixelPointer(byte* basePointer, int y, int x, int stride, int channels)
    {
        return basePointer + y * stride + x * channels;
    }
}