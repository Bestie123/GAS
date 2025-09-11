using GAS.Models;
using GAS.ViewModels;
using System.Windows;

namespace GAS.Views.Dialogs
{
    public partial class RequestDialog : Window
    {
        public RequestDialog(AppRequest request)
        {
            InitializeComponent();

            // Устанавливаем VM в DataContext
            DataContext = new RequestDialogViewModel(request);

            Owner = Application.Current.MainWindow;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

