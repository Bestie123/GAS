using GAS.Commands;
using GAS.Models;
using GAS.Services;
using GAS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class RequestsViewModel : ViewModelBase
    {
        private readonly DataService _data = new();

        public ObservableCollection<AppRequest> Items { get; } = new();
        private AppRequest? _selected;
        public AppRequest? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); } }

        public ICommand ReloadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public RequestsViewModel()
        {
            ReloadCommand = new RelayCommand(_ => Load());
            AddCommand = new RelayCommand(_ => Add());
            EditCommand = new RelayCommand(_ => Edit(), _ => Selected != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => Selected != null);

            Load();
        }

        private void Load()
        {
            Items.Clear();
            foreach (var r in _data.GetRequests())
                Items.Add(r);
        }

        private void Add()
        {
            var vm = new AppRequest();
            var dlg = new RequestDialog(vm);
            if (dlg.ShowDialog() == true)
            {
                _data.AddRequest(vm);
                Load();
            }
        }

        private void Edit()
        {
            if (Selected == null) return;
            var copy = new AppRequest
            {
                Id = Selected.Id,
                Status = Selected.Status,
                ClientId = Selected.ClientId,
                City = Selected.City,
                Street = Selected.Street,
                House = Selected.House,
                Index = Selected.Index,
                Corp = Selected.Corp,
                Entrance = Selected.Entrance,
                Apartment = Selected.Apartment,
                Essence = Selected.Essence,
                CompletedWork = Selected.CompletedWork,
                DateRequest = Selected.DateRequest
            };
            var dlg = new RequestDialog(copy);
            if (dlg.ShowDialog() == true)
            {
                _data.UpdateRequest(copy);
                Load();
            }
        }

        private void Delete()
        {
            if (Selected == null) return;
            _data.DeleteRequest(Selected);
            Load();
        }
    }
}

