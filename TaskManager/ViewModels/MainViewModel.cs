using Caliburn.Micro;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;
using TaskManager.Helpers;

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
                    Application.Current.Properties[Constant.AzureSql] = Constant.AzureSql;
                    DbContextFactory.TaskRepository = _container.GetInstance<AzureSqlRepository>();
                    break;
                default:
                    Application.Current.Properties[Constant.Database] = Constant.Sqlite;
                    DbContextFactory.TaskRepository = _container.GetInstance<SqliteRepository>();
                    break;
            }
            await DialogHelper.ShowMessageDialog(Constant.ChangeDbTitleMsg, $"{Constant.DbSwitchSuccessMsg} {DatabaseProviderName}", MessageDialogStyle.Affirmative);
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

        public void DisplayHomeView()
        {
            HomeViewModel homeViewModel = _container.GetInstance<HomeViewModel>();
            HomeViewModel.ActiveHomeViewModelId = homeViewModel.GetHashCode();
            ActivateItemAsync(homeViewModel);
        }
    }
}
