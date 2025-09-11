using GAS.Commands;
using GAS.Models;
using GAS.Services;
using GAS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class EquipmentViewModel : ViewModelBase
    {
        private readonly DataService _data = new();

        public ObservableCollection<Equipment> Items { get; } = new();
        private Equipment? _selected;
        public Equipment? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); } }

        public ICommand ReloadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EquipmentViewModel()
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
            foreach (var e in _data.GetEquipment())
                Items.Add(e);
        }

        private void Add()
        {
            var vm = new Equipment();
            var dlg = new EquipmentDialog(vm);
            if (dlg.ShowDialog() == true)
            {
                _data.AddEquipment(vm);
                Load();
            }
        }

        private void Edit()
        {
            if (Selected == null) return;
            var copy = new Equipment
            {
                Id = Selected.Id,
                City = Selected.City,
                Street = Selected.Street,
                House = Selected.House,
                Index = Selected.Index,
                Corp = Selected.Corp,
                Entrance = Selected.Entrance,
                Antenna = Selected.Antenna,
                DRS = Selected.DRS,
                Note = Selected.Note,
                Serviced = Selected.Serviced,
                InstallationDate = Selected.InstallationDate,
                RevisionDate = Selected.RevisionDate
            };
            var dlg = new EquipmentDialog(copy);
            if (dlg.ShowDialog() == true)
            {
                _data.UpdateEquipment(copy);
                Load();
            }
        }

        private void Delete()
        {
            if (Selected == null) return;
            _data.DeleteEquipment(Selected);
            Load();
        }
    }
}

