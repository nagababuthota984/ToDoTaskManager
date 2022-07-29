using System;
using System.Globalization;
using System.Windows.Data;

namespace TaskManager.Converters
{
    public class DescriptionToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string desc)
                if (string.IsNullOrWhiteSpace(desc))
                    return "No description";
                else
                    return value.ToString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
