using Caliburn.Micro;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class MainViewModel:Conductor<Screen>
    {
        
        public MainViewModel()
        {
            ActivateItemAsync(new HomeViewModel());
        }
    }
}
