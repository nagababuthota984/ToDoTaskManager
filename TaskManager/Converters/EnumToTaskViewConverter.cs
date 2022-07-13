using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TaskManager.Converters
{
    public class EnumToTaskViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((int)value)
            {
                case 0: return Application.Current.FindResource("TM.ListBox.TaskCardViewTemplate");
                case 1: return Application.Current.FindResource("TM.ListBox.TaskListViewTemplate");
                default: return Application.Current.FindResource("TM.ListBox.TaskCardViewTemplate");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
