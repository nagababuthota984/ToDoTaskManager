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
        public TaskbarIcon taskbarIcon { get; set; }
        public App()
        {
            Properties.Add(Constant.database, Constant.sqlite);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Popup p = new()
            {
                Child = taskbarIcon
            };
        }
    }
}
