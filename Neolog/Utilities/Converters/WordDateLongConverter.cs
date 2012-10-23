using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Neolog.Utilities.Converters
{
    public class WordDateLongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return AppSettings.DoLongDate((DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
