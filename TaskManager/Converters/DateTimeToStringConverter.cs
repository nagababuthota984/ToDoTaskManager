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
                if (dueDate < DateTime.Now)
                    result = Constant.Overdue;
                else if (dueDate.Date == DateTime.Today.Date.AddDays(1))
                    result += Constant.Tomorrow;
                else if (dueDate.Date == DateTime.Today.Date)
                    result += Constant.Today;
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
