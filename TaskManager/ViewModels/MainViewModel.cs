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

        public void GotoSource()
        {
            if (!TryOpenDefaultBrowser(Constant.GitHubRepoUrl))
            {
                TryOpenIE(Constant.GitHubRepoUrl);
            }
        }

        public void SwitchTheme(object sender, RoutedEventArgs e)
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

        private void TryOpenIE(string url)
        {
            try
            {
                string keyValue = Registry.GetValue(Constant.IEBrowserRegistryKeyName, "", null).ToString();
                string ieBrowserPath = keyValue.Replace("%1", "");
                Process.Start(new ProcessStartInfo(ieBrowserPath, url));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public bool TryOpenDefaultBrowser(string url)
        {
            string? progId = Registry.GetValue(Constant.DefaultBrowserRegistryKeyName, Constant.ProgId, null)?.ToString();
            if (!string.IsNullOrWhiteSpace(progId))
            {
                string browserPath = $"{progId}\\shell\\open\\command";
                using (RegistryKey? pathKey = Registry.ClassesRoot.OpenSubKey(browserPath))
                {
                    try
                    {
                        string defaultBrowser = pathKey.GetValue(null).ToString().Replace("%1", url);
                        Process.Start(defaultBrowser);
                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            return false;
        }
        public void DisplayHomeView()
        {
            HomeViewModel homeViewModel = _container.GetInstance<HomeViewModel>();
            HomeViewModel.ActiveHomeViewModelId = homeViewModel.GetHashCode();
            ActivateItemAsync(homeViewModel);
        }
    }
}
