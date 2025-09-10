using System.Windows;
using System.Windows.Controls;

namespace GAS.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Pwd_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }
    }
}

