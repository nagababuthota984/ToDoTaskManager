using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        private readonly SimpleContainer _container;

        public MainViewModel( SimpleContainer container)
        {
            _container = container;
            DisplayHomeView();
        }
        public void ChangeDb()
        {
            if (Application.Current.Properties[MessageStrings.Database] == MessageStrings.Sqlite)
                Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlserver;
            else
                Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlite;
            DisplayHomeView();
        }
        public void DisplayHomeView()
        {
            ActivateItemAsync(_container.GetInstance<HomeViewModel>());
        }
    }
}
