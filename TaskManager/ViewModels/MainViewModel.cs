using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using TaskManager.Common;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        private readonly SimpleContainer _container;
        private string _dbProvider;
        public string DatabaseProvider
        {
            get { return _dbProvider; }
            set { _dbProvider = value; NotifyOfPropertyChange(nameof(DatabaseProvider)); }
        }

        public MainViewModel( SimpleContainer container)
        {
            _container = container;
            DatabaseProvider = Application.Current.Properties[MessageStrings.Database].ToString();
            DisplayHomeView();
        }
        public async void ChangeDb()
        {
            switch (Application.Current.Properties[MessageStrings.Database])
            {
                case MessageStrings.Sqlite:
                    if(MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ChangeDbTitleMsg, $"{MessageStrings.DbChangeResultsIn}{MessageStrings.Sqlserver} database. {MessageStrings.ConfirmChangeDb}", MessageDialogStyle.AffirmativeAndNegative))
                        Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlserver;
                    break;
                default:
                    if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(MessageStrings.ChangeDbTitleMsg, $"{MessageStrings.DbChangeResultsIn}{MessageStrings.Sqlite} database. {MessageStrings.ConfirmChangeDb}", MessageDialogStyle.AffirmativeAndNegative))
                        Application.Current.Properties[MessageStrings.Database] = MessageStrings.Sqlite;
                    break;
            }
            DatabaseProvider = Application.Current.Properties[MessageStrings.Database].ToString();
            DisplayHomeView();
        }
        public void DisplayHomeView()
        {
            ActivateItemAsync(_container.GetInstance<HomeViewModel>());
        }
    }
}
