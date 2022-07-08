using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static TaskManager.Models.Enums;

namespace TaskManager.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                switch (priority)
                {
                    case Priority.Low:
                        return Application.Current.FindResource("TM.Colors.LowPriority");
                    case Priority.Medium:
                        return Application.Current.FindResource("TM.Colors.MediumPriority");
                }
            }
            return Application.Current.FindResource("TM.Colors.HighPriority");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
