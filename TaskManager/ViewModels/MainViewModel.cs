using Caliburn.Micro;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        #region Fields

        private readonly SimpleContainer _container;
        private bool _isProgressRingActive=false;
        private string _dbProviderName;

        #endregion

        #region Properties

        public List<string> DatabaseProviders
        {
            get { return new List<string>() { Constant.Sqlite, Constant.AzureSql }; }
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

        public bool IsProgressRingActive
        {
            get 
            { 
                return _isProgressRingActive; 
            }
            set
            {
                _isProgressRingActive = value;
                NotifyOfPropertyChange(nameof(IsProgressRingActive));
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
                case Constant.AzureSql:
                    if (IsConnectedToInternet())
                    {
                        DbContextFactory.TaskRepository = _container.GetInstance<AzureSqlRepository>();
                        Application.Current.Properties[Constant.AzureSql] = Constant.AzureSql;
                    }
                    else
                    {
                        DatabaseProviderName = Constant.Sqlite;
                        MessageBox.Show(Constant.NoActiveInternet, Constant.AzureConnectionFailed);
                    }
                    break;
                default:
                    Application.Current.Properties[Constant.Database] = Constant.Sqlite;
                    DbContextFactory.TaskRepository = _container.GetInstance<SqliteRepository>();
                    break;
            }
            await System.Threading.Tasks.Task.Run(DisplayHomeView);
        }

        private bool IsConnectedToInternet()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        public void Open()
        {
            (Application.Current.MainWindow as MetroWindow)?.Show();
        }

        public void Quit()
        {
            Application.Current.Shutdown();
        }

        public void GotoSource()
        {
            Process.Start(new ProcessStartInfo(Constant.GitHubRepoUrl) { UseShellExecute = true });
        }

        public void SwitchTheme()
        {
            switch (ThemeManager.Current.DetectTheme(Application.Current).Name)
            {
                case "Dark.Blue":
                    ThemeManager.Current.ChangeTheme(Application.Current, "Light.Blue");
                    break;
                case "Light.Blue":
                    ThemeManager.Current.ChangeTheme(Application.Current, "Dark.Blue");
                    break;
            }
        }

        public async void DisplayHomeView()
        {
            IsProgressRingActive = true;
            HomeViewModel homeViewModel = _container.GetInstance<HomeViewModel>();
            HomeViewModel.ActiveHomeViewModelId = homeViewModel.GetHashCode();
            await ActivateItemAsync(homeViewModel);
            IsProgressRingActive=false;
        }
    }
}
