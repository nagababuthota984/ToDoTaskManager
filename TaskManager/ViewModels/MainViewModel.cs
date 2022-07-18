using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Common;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        #region Fields
        private readonly SimpleContainer _container;

        private string _dbProviderName;


        #endregion

        #region Properties
        public List<string> DatabaseProvidersList
        {
            get { return new List<string>() { Constant.Sqlite, Constant.Sqlserver }; }
        }
        public string DatabaseProviderName
        {
            get { return _dbProviderName; }
            set { _dbProviderName = value; NotifyOfPropertyChange(nameof(DatabaseProviderName)); }
        }
        #endregion


        public MainViewModel(SimpleContainer container)
        {
            _container = container;
            DatabaseProviderName = Application.Current.Properties[Constant.Database].ToString();
            DisplayHomeView();
        }
        public async void ChangeDb()
        {
            switch (DatabaseProviderName)
            {
                case Constant.Sqlserver:
                    Application.Current.Properties[Constant.Database] = Constant.Sqlserver;
                    break;
                default:
                    Application.Current.Properties[Constant.Database] = Constant.Sqlite;
                    break;
            }
            await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(Constant.ChangeDbTitleMsg,$"{Constant.DbSwitchSuccessMsg} {DatabaseProviderName}", MessageDialogStyle.Affirmative);

            DisplayHomeView();
        }

        public void DisplayHomeView()
        {
            ActivateItemAsync(_container.GetInstance<HomeViewModel>());
        }
    }
}