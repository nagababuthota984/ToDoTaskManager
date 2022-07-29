using System.Windows;
using System.Windows.Controls;

namespace TaskManager.Behaviors
{
    public class DigitsOnlyBehavior
    {
        public static bool GetIsDigitOnlyProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDigitOnlyProperty);
        }

        public static void SetIsDigitOnlyProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDigitOnlyProperty, value);
        }

        public static readonly DependencyProperty IsDigitOnlyProperty = DependencyProperty.RegisterAttached("IsDigitOnly", typeof(bool), typeof(DigitsOnlyBehavior), new PropertyMetadata(false, OnIsDigitOnlyPropertyChanged));


        private static void OnIsDigitOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb && tb != null)
            {
                tb.TextChanged -= OnTextChanged;
                if ((bool)e.NewValue)
                    tb.TextChanged += OnTextChanged;
            }
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && tb != null)
            {
                if (int.TryParse(tb.Text, out int value))
                    tb.Style = (Style)Application.Current.FindResource("TM.TextBox.Default");
                else
                    tb.Style = (Style)Application.Current.FindResource("TM.TextBox.IsDigitOnlyError");
            }


        }
    }
}
