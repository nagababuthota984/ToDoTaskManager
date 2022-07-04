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
    public class CategoryToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(parameter.ToString(), out int category))
            {
                return (int)value == category;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && int.TryParse(parameter.ToString(), out int category))
            {
                switch(category)
                {
                    case 0:
                        return Category.NewFeature;
                    case 1:
                        return Category.BugFix;
                    case 2:
                        return Category.LearningTask;
                    default:
                        return Category.Others;
                }
            }
            return Category.Others;
        }
    }
}
