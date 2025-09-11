using GAS.Commands;
using GAS.Services;
using System.Windows;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly AuthService _auth = new();

        public string FullName { get; set; } = "";
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(_ => DoRegister());
        }

        private void DoRegister()
        {
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            if (_auth.Register(FullName, Login, Password, out var error))
            {
                MessageBox.Show("Регистрация успешна");
                // Закрыть окно регистрации
                foreach (Window w in Application.Current.Windows)
                    if (w is Views.RegisterView) { w.Close(); break; }
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }
}

