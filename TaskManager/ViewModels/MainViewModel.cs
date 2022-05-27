using Caliburn.Micro;

namespace TaskManager.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {

        public MainViewModel()
        {
            ActivateItemAsync(new HomeViewModel());
        }
    }
}
