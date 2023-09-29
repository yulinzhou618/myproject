using System;
using System.Globalization;
using System.Windows.Data;

namespace AIVisualwfpnew.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpan2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string defaultvalue = "00:00:00";
            if (value == null)
                return defaultvalue;

            if (!(value is TimeSpan ts))
                return defaultvalue;

            return $"{ts.Hours.ToString("D2")}:{ts.Minutes.ToString("D2")}:{ts.Seconds.ToString("D2")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
