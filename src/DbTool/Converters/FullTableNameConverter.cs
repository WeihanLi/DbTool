// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core.Entity;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DbTool.Converters;

[ValueConversion(typeof(TableEntity), typeof(string))]
public class FullTableNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is TableEntity tableEntity ? tableEntity.GetFullTableName() : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
