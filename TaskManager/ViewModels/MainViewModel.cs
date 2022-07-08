using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        #region Fields
        private readonly SimpleContainer _container;
        private string _dbProviderName;
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
            switch (DatabaseProviderName)
            {
                case MessageStrings.Sqlserver:
                    await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ChangeDbTitleMsg, $"{MessageStrings.DbChangeResultsIn}{MessageStrings.Sqlserver} database. {MessageStrings.ConfirmChangeDb}", MessageDialogStyle.Affirmative);
                        Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlserver;
                    break;
                default:
                    await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ChangeDbTitleMsg, $"{MessageStrings.DbChangeResultsIn}{MessageStrings.Sqlite} database. {MessageStrings.ConfirmChangeDb}", MessageDialogStyle.Affirmative);
                        Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlite;
                    break;
            }
            DisplayHomeView();
        }
        public void DisplayHomeView()
        {
            ActivateItemAsync(_container.GetInstance<HomeViewModel>());
        }
    }
}
