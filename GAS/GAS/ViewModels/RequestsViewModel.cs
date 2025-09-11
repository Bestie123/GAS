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
        public AppRequest? Selected
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); }
        }

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
            var newRequest = new AppRequest();
            var dlg = new RequestDialog(newRequest);
            if (dlg.ShowDialog() == true)
            {
                _data.AddRequest(newRequest);
                Load();
            }
        }

        private void Edit()
        {
            if (Selected == null) return;

            var request = _data.GetRequestById(Selected.Id);
            var dlg = new RequestDialog(request);
            if (dlg.ShowDialog() == true)
            {
                _data.UpdateRequest(request);
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
