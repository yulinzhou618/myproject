using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AIVisualwfpnew.Converters
{
    [ValueConversion(typeof(DateTime), typeof(DateTime))]
    public class DateTimeAdjustConverter : IValueConverter
    {
        public double AddDays { get; set; } = 0;

        public double SubDays { get; set; } = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime time = (DateTime)value;
            time = time.AddDays(AddDays);
            time -= TimeSpan.FromDays(SubDays);
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
