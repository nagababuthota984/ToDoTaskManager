using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static TaskManager.Models.Enums;

namespace TaskManager.Converters
{
    public class PriorityToBooleanConverter : IValueConverter
    {
        //to whether check the radio button or not.. returns true/false
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(int.TryParse(parameter.ToString(), out int priority))
                return (int)value == priority;
            return false;
               
        }

        //to set the selectedpriority property
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int.TryParse(parameter.ToString(), out int priority);
            switch (priority)
            {
                case 0: 
                    return Priority.Low; 
                case 1:
                    return Priority.Medium;
                case 2:
                    return Priority.High;
                default:
                    return Priority.Low;
            }
            
        }
    }
}
