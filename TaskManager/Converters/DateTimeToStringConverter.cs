﻿using System;
using System.Globalization;
using System.Windows.Data;

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
                    result = "Today";
                else if (dueDate.Date == DateTime.Today.Date.AddDays(1))
                    result = "Tomorrow";
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