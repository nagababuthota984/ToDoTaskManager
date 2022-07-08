using System;
using System.Globalization;
using System.Windows.Data;
using TaskManager.Common;

namespace TaskManager.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result=string.Empty;
            if(value is DateTime dueDate)
            {
                if (dueDate.Date == DateTime.Today.Date)
                    result = MessageStrings.Today;
                else if (dueDate.Date == DateTime.Today.Date.AddDays(1))
                    result = MessageStrings.Tomorrow;
                else
                    result += $"{dueDate.ToShortDateString()}";
                result += $" | {dueDate.ToShortTimeString()}";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
