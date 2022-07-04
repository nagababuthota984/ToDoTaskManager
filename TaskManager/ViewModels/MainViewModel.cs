using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        public MainViewModel(HomeViewModel homeViewModel)
        {
            ActivateItemAsync(homeViewModel);
        }
    }
}
