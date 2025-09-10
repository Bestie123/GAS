using GAS.Commands;
using GAS.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserControl? _currentView;
        public UserControl? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand ShowClientsCommand { get; }
        public ICommand ShowRequestsCommand { get; }
        public ICommand ShowEquipmentCommand { get; }

        public MainViewModel()
        {
            ShowClientsCommand = new RelayCommand(_ => CurrentView = new ClientsView());
            ShowRequestsCommand = new RelayCommand(_ => CurrentView = new RequestsView());
            ShowEquipmentCommand = new RelayCommand(_ => CurrentView = new EquipmentView());

            CurrentView = new ClientsView(); // экран по умолчанию
        }
    }
}

