using System.Windows;
using System.Windows.Controls;

namespace GAS.Views
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            Owner = Application.Current.Windows.Count > 0 ? Application.Current.Windows[0] : null;
        }

        private void Pwd_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.RegisterViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

        private void Pwd2_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.RegisterViewModel vm)
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}

