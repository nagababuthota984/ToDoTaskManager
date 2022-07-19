using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System.Windows;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
            this.Closing += MainView_Closing;
        }

        private void MainView_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void SwitchTheme(object sender, RoutedEventArgs e)
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

        private void Open(object sender, RoutedEventArgs e)
        {
            this.Show();
        }
        
        private void Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
