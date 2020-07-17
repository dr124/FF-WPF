
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using FF_WPF.Utils;

namespace FF_WPF.Converters
{
    public class BitmapToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Bitmap b)
            {
                return b.ToImageSource();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // not needed
        }
    }
}
