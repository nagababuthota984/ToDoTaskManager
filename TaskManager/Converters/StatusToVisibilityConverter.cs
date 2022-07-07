using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static TaskManager.Models.Enums;

namespace TaskManager.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is Status status && status == Status.InProgress) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
