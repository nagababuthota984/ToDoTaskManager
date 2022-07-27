using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        #region Fields

        private readonly SimpleContainer _container;

        private string _dbProviderName;

        #endregion

        #region Properties

        public List<string> DatabaseProviders
        {
            get { return new List<string>() { Constant.Sqlite, Constant.Sqlserver }; }
        }

        public string DatabaseProviderName
        {
            get { return _dbProviderName; }
            set
            {
                _dbProviderName = value;
                NotifyOfPropertyChange(nameof(DatabaseProviderName));
            }
        }

        #endregion

        public MainViewModel(SimpleContainer container)
        {
            _container = container;
            DatabaseProviderName = Application.Current.Properties[Constant.Database].ToString();
            DbContextFactory.TaskRepository = _container.GetInstance<SqliteRepository>();
            DisplayHomeView();
        }

        public async void ChangeDb()
        {
            switch (DatabaseProviderName)
            {
                case Constant.Sqlserver:
                    Application.Current.Properties[Constant.Database] = Constant.Sqlserver;
                    DbContextFactory.TaskRepository = _container.GetInstance<SqlServerRepository>();
                    break;
                default:
                    Application.Current.Properties[Constant.Database] = Constant.Sqlite;
                    DbContextFactory.TaskRepository = _container.GetInstance<SqliteRepository>();
                    break;
            }
            await Constant.ShowMessageDialog(Constant.ChangeDbTitleMsg, $"{Constant.DbSwitchSuccessMsg} {DatabaseProviderName}", MessageDialogStyle.Affirmative);
            DisplayHomeView();
        }

        public void Open()
        {
            (Application.Current.MainWindow as MetroWindow)?.Show();
        }

        public void Quit()
        {
            Application.Current.Shutdown();
        }

        public void DisplayHomeView()
        {
            HomeViewModel homeViewModel = _container.GetInstance<HomeViewModel>();
            HomeViewModel.ActiveHomeViewModelId = homeViewModel.GetHashCode();
            ActivateItemAsync(homeViewModel);
        }
    }
}
