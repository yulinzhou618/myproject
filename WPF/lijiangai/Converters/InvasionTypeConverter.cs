using AIVisualwfpnew.Entitys;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIVisualwfpnew.Converters
{
    [ValueConversion(typeof(InvasionType), typeof(string))]
    public class InvasionTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is InvasionType v)
            {
                switch (v)
                {
                    case InvasionType.Person:
                        return "人类";
                    case InvasionType.Livestock:
                        return "牲畜";
                    case InvasionType.Others:
                        return "其它";
                    default:
                        return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
