using GAS.Commands;
using GAS.Models;
using GAS.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GAS.ViewModels
{
    public class RequestDialogViewModel : ViewModelBase
    {
        private readonly DataService _data = new();

        public AppRequest EditableRequest { get; }
        private readonly AppRequest _original;

        public ObservableCollection<Client> Clients { get; }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public RequestDialogViewModel(AppRequest original)
        {
            _original = original;

            // Создаём копию для редактирования
            EditableRequest = new AppRequest
            {
                Id = original.Id,
                Status = original.Status,
                ClientId = original.ClientId,
                City = original.City,
                Street = original.Street,
                House = original.House,
                Index = original.Index,
                Corp = original.Corp,
                Entrance = original.Entrance,
                Apartment = original.Apartment,
                Essence = original.Essence,
                CompletedWork = original.CompletedWork,
                DateRequest = original.DateRequest
            };

            Clients = new ObservableCollection<Client>(_data.GetClients());

            OkCommand = new RelayCommand(_ => Confirm());
            CancelCommand = new RelayCommand(_ => Close(false));
        }

        private void Confirm()
        {
            if (EditableRequest.ClientId.HasValue == false ||
                Clients.Any(c => c.Id == EditableRequest.ClientId.Value) == false)
            {
                MessageBox.Show("Выберите клиента из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Копируем значения обратно в оригинал
            _original.ClientId = EditableRequest.ClientId;
            _original.Status = EditableRequest.Status;
            _original.City = EditableRequest.City;
            _original.Street = EditableRequest.Street;
            _original.House = EditableRequest.House;
            _original.Index = EditableRequest.Index;
            _original.Corp = EditableRequest.Corp;
            _original.Entrance = EditableRequest.Entrance;
            _original.Apartment = EditableRequest.Apartment;
            _original.Essence = EditableRequest.Essence;
            _original.CompletedWork = EditableRequest.CompletedWork;
            _original.DateRequest = EditableRequest.DateRequest;

            Close(true);
        }

        private void Close(bool result)
        {
            foreach (Window w in Application.Current.Windows)
                if (w.DataContext == this)
                {
                    w.DialogResult = result;
                    w.Close();
                    break;
                }
        }
    }
}
