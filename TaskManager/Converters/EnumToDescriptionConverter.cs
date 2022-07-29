using System;
using System.Globalization;
using System.Windows.Data;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelper.GetDescription(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
