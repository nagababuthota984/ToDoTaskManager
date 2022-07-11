using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Common;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        #region Fields
        private readonly SimpleContainer _container;

        private string _dbProviderName;

        private static bool handleSelectionChangedEvent = true;

        #endregion

        #region Properties
        public List<string> DatabaseProvidersList
        {
            get { return new List<string>() { MessageStrings.Sqlite, MessageStrings.Sqlserver }; }
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
            DatabaseProviderName = Application.Current.Properties[MessageStrings.Database].ToString();
            DisplayHomeView();
        }
        public async void ChangeDb()
        {
            if (handleSelectionChangedEvent)
            {
                handleSelectionChangedEvent = false;
                switch (DatabaseProviderName)
                {
                    case MessageStrings.Sqlserver:
                        if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ChangeDbTitleMsg, $"{MessageStrings.DbChangeResultsIn}{MessageStrings.Sqlserver} database. {MessageStrings.ConfirmChangeDb}", MessageDialogStyle.AffirmativeAndNegative))
                        {
                            Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlserver;
                        }
                        else
                        {
                            DatabaseProviderName = MessageStrings.Sqlite;
                        }

                        break;
                    default:
                        if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ChangeDbTitleMsg, $"{MessageStrings.DbChangeResultsIn}{MessageStrings.Sqlite} database. {MessageStrings.ConfirmChangeDb}", MessageDialogStyle.AffirmativeAndNegative))
                        {
                            Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlite;
                        }
                        else
                        {
                            DatabaseProviderName = MessageStrings.Sqlserver;
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