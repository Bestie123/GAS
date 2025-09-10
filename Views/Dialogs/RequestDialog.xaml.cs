using GAS.Models;
using System.Windows;

namespace GAS.Views.Dialogs
{
    public partial class RequestDialog : Window
    {
        public RequestDialog(AppRequest vm)
        {
            InitializeComponent();
            Owner = Application.Current.Windows.Count > 0 ? Application.Current.Windows[0] : null;
            DataContext = vm;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

