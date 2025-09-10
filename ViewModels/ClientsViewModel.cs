using GAS.Commands;
using GAS.Models;
using GAS.Services;
using GAS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly DataService _data = new();

        public ObservableCollection<Client> Items { get; } = new();
        private Client? _selected;
        public Client? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); } }

        public string SearchText { get; set; } = "";

        public ICommand ReloadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }

        public ClientsViewModel()
        {
            ReloadCommand = new RelayCommand(_ => Load());
            AddCommand = new RelayCommand(_ => Add());
            EditCommand = new RelayCommand(_ => Edit(), _ => Selected != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => Selected != null);
            SearchCommand = new RelayCommand(_ => Search());

            Load();
        }

        private void Load()
        {
            Items.Clear();
            foreach (var c in _data.GetClients())
                Items.Add(c);
        }

        private void Add()
        {
            var vm = new Client { FullName = "", Phone = "" };
            var dlg = new ClientDialog(vm);
            if (dlg.ShowDialog() == true)
            {
                _data.AddClient(vm);
                Load();
            }
        }

        private void Edit()
        {
            if (Selected == null) return;
            var copy = new Client { Id = Selected.Id, FullName = Selected.FullName, Phone = Selected.Phone };
            var dlg = new ClientDialog(copy);
            if (dlg.ShowDialog() == true)
            {
                _data.UpdateClient(copy);
                Load();
            }
        }

        private void Delete()
        {
            if (Selected == null) return;
            if (MessageBox.Show("Удалить клиента?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _data.DeleteClient(Selected);
                Load();
            }
        }

        private void Search()
        {
            var all = _data.GetClients();
            var q = SearchText?.Trim().ToLower() ?? "";
            var filtered = string.IsNullOrEmpty(q) ? all : all.Where(c =>
                (c.FullName ?? "").ToLower().Contains(q) ||
                (c.Phone ?? "").ToLower().Contains(q)).ToList();

            Items.Clear();
            foreach (var c in filtered)
                Items.Add(c);
        }
    }
}

