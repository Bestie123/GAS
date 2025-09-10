using GAS.Commands;
using GAS.Services;
using GAS.Views;
using System.Windows;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _auth = new();

        public string Login { get; set; } = "";
        public string Password { get; set; } = "";

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => DoLogin());
            OpenRegisterCommand = new RelayCommand(_ => OpenRegister());
        }

        private void DoLogin()
        {
            var user = _auth.Login(Login, Password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            var main = new MainWindow();
            // Закрыть окно логина:
            foreach (Window w in Application.Current.Windows)
                if (w is LoginView) { main.Show(); w.Close(); return; }
        }

        private void OpenRegister()
        {
            var reg = new RegisterView();
            reg.ShowDialog();
        }
    }
}

