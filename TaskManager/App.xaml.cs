using System.Windows;
using TaskManager.Common;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Properties.Add(MessageStrings.Database, MessageStrings.Sqlite);
        }
    }
}
