using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace TaskManager.Helpers
{
    public class DialogHelper
    {
        public static async System.Threading.Tasks.Task<bool> ShowMessageDialog(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            if (MessageDialogResult.Affirmative == await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync(title, message, style))
                return true;
            return false;
        }
    }
}
