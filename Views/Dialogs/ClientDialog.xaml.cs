using GAS.Models;
using System.Windows;

namespace GAS.Views.Dialogs
{
    public partial class ClientDialog : Window
    {
        public ClientDialog(Client vm)
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

