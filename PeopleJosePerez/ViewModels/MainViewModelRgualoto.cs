using PeopleRgualoto.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PeopleRgualoto.ViewModels
{
    public class MainViewModelRgualoto : INotifyPropertyChanged
    {
        private string _newPersonName;
        private string _statusMessage;

        public string NewPersonName
        {
            get => _newPersonName;
            set => SetProperty(ref _newPersonName, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ObservableCollection<PersonRgualoto> People { get; }
        public ICommand AddPersonCommand { get; }
        public ICommand GetPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public MainViewModelRgualoto()
        {
            People = new ObservableCollection<PersonRgualoto>();
            AddPersonCommand = new Command(AddPerson);
            GetPeopleCommand = new Command(GetPeople);
            DeletePersonCommand = new Command<int>(DeletePerson);
        }

        private void AddPerson()
        {
            if (string.IsNullOrWhiteSpace(NewPersonName))
            {
                StatusMessage = "Debe ingresar  un nombre por favor.";
                return;
            }

            App.PersonRepo.AddNewPerson(NewPersonName);
            StatusMessage = App.PersonRepo.StatusMessage;
            NewPersonName = string.Empty;
            GetPeople();
        }

        private void GetPeople()
        {
            try
            {
                People.Clear();
                foreach (var person in App.PersonRepo.GetAllPeople())
                {
                    People.Add(person);
                }
                StatusMessage = "Todas las personas que ingreso han sido guardadas correctamente.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar personas: {ex.Message}";
            }
        }

        private async void DeletePerson(int id)
        {
            App.PersonRepo.DeletePerson(id);
            StatusMessage = App.PersonRepo.StatusMessage;

            // Mostrar mensaje personalizado
            if (App.PersonRepo.StatusMessage.Contains("eliminado"))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Registro Eliminado",
                    "Romel Gualoto acaba de eliminar un registro.",
                    "Proceso terminado");
            }

            GetPeople();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
