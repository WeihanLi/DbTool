using System;
using System.Globalization;
using System.Windows.Data;
using DbTool.Core.Entity;

namespace DbTool.Converters
{
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
}
