using System.Drawing;
using System.Threading;

namespace FF_WPF.Filters
{
    public class NoFilter : ImageFilter
    {
        protected override Bitmap ProcessImage(Bitmap image, FilterParams param, CancellationToken ct)
        {
            return image;
        }
    }
}