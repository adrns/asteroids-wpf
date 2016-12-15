using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AsteroidsWPF.ViewModel
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = (bool)value;
            return visible ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("this is used too");
            return (Visibility)value == Visibility.Visible;
        }
    }

}
