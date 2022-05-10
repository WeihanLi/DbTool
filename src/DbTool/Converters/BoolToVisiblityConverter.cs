// Copyright (c) Juster zhu. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DbTool.Converters;
public class BoolToVisiblityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool)
        {
            var isLoad = (bool)value;
            return isLoad ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
