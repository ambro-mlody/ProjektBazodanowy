using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GUI.Behaviors
{
    public class BoolToLabelConverter<T> : IValueConverter
    {
        public T FalseLabel { get; set; }
        public T TrueLabel { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueLabel : FalseLabel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((T)value).Equals(TrueLabel);
        }
    }
}
