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
            if (handleSelectionChangedEvent)
            {
                handleSelectionChangedEvent = false;
                switch (DatabaseProviderName)
                {
                    case Constant.Sqlserver:
                        if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(Constant.ChangeDbTitleMsg, $"{Constant.DbChangeResultsIn}{Constant.Sqlserver} database. {Constant.ConfirmChangeDb}", MessageDialogStyle.AffirmativeAndNegative))
                        {
                            Application.Current.Properties[Constant.Database] = Constant.Sqlserver;
                        }
                        else
                        {
                            DatabaseProviderName = Constant.Sqlite;
                        }

                        break;
                    default:
                        if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(Constant.ChangeDbTitleMsg, $"{Constant.DbChangeResultsIn}{Constant.Sqlite} database. {Constant.ConfirmChangeDb}", MessageDialogStyle.AffirmativeAndNegative))
                        {
                            Application.Current.Properties[Constant.Database] = Constant.Sqlite;
                        }
                        else
                        {
                            DatabaseProviderName = Constant.Sqlserver;
                        }

                        break;
                }
                DisplayHomeView();
                handleSelectionChangedEvent = true;
            }
        }

        public void DisplayHomeView()
        {
            ActivateItemAsync(_container.GetInstance<HomeViewModel>());
        }
    }
}
