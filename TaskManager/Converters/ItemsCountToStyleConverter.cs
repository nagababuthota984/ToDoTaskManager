﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TaskManager.Converters
{
    public class ItemsCountToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((int)value == 0)
            {
                switch(parameter.ToString())
                {
                    case "0":
                        return Application.Current.FindResource("TM.ListBox.NoNewTasksListBoxStyle");
                    case "1":
                        return Application.Current.FindResource("TM.ListBox.NoInProgressTasksListBoxStyle");
                    case "2":
                        return Application.Current.FindResource("TM.ListBox.NoCompletedTasksListBoxStyle");
                    default:
                        return Application.Current.FindResource("TM.ListBox.DefaultListBoxStyle");
                }
            }
            return Application.Current.FindResource("TM.ListBox.DefaultListBoxStyle");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
