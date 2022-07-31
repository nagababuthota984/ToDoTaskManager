using System.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls.Primitives;
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
            Properties.Add(Constant.Database, Constant.Sqlite);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
    }
}
