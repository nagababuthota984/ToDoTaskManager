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
            string result = string.Empty;
            if (value is DateTime dateTime)
            {
                if (dateTime < DateTime.Now)
                    result = Constant.Overdue;
                else if (dateTime.Date == DateTime.Today.Date.AddDays(1))
                    result += Constant.Tomorrow;
                else if (dateTime.Date == DateTime.Today.Date)
                    result += Constant.Today;
                else
                    result += $"{dateTime.ToShortDateString()}";

                result += $" | {dateTime.ToShortTimeString()}";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
