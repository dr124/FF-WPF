using System.Drawing;

namespace FF.WPF.Filters;

public class NoFilter : ImageFilter
{
    protected override Bitmap ProcessImage(Bitmap image, FilterParams param, CancellationToken ct)
    {
        return image;
    }
}