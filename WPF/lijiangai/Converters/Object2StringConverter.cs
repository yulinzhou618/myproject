﻿using AIVisualwfpnew.Entitys;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIVisualwfpnew.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class Object2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "" : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
