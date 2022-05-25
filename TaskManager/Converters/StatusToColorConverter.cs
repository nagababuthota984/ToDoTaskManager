using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static TaskManager.Models.Enums;

namespace TaskManager.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Priority priority)
            {
                if (priority == Priority.Low)
                    return Application.Current.FindResource("TM.Colors.LowPriority");
                else if (priority == Priority.Medium)
                    return Application.Current.FindResource("TM.Colors.MediumPriority");
            }
            return Application.Current.FindResource("TM.Colors.HighPriority");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
