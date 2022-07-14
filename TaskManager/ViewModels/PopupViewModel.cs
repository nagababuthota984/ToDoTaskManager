using Caliburn.Micro;
using static TaskManager.Models.Enums;

namespace TaskManager.ViewModels
{
    public class PopupViewModel : Screen
    {
        private string _winTitle;
        private string _message;

        public string WinTitle
        {
            get { return _winTitle; }
            set { _winTitle = value; NotifyOfPropertyChange(nameof(WinTitle)); }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; NotifyOfPropertyChange(nameof(Message)); }
        }

        public PopupViewModel(string winTitle, string message)
        {
            WinTitle = winTitle;
            Message = message;
        }

        public void Yes()
        {
            
        }
        public void No()
        {
        }
        
    }
}
