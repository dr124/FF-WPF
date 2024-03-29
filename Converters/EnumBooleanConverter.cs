﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FF.WPF.Converters;

public class EnumBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(parameter is string parameterString))
            return DependencyProperty.UnsetValue;

        if (Enum.IsDefined(value.GetType(), value) == false)
            return DependencyProperty.UnsetValue;

        var parameterValue = Enum.Parse(value.GetType(), parameterString);

        return parameterValue.Equals(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(parameter is string parameterString))
            return DependencyProperty.UnsetValue;

        return Enum.Parse(targetType, parameterString);
    }
}