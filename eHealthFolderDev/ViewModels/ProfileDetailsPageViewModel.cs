using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using eHealthFolderDev.Models;
using eHealthFolderDev.Services;

namespace eHealthFolderDev.ViewModels
{
    public class ProfileDetailsPageViewModel : INotifyPropertyChanged
    {
        private readonly eHealthFolderDatabase _database;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ----------------------------------------
        // Properties
        // ----------------------------------------
        private Person _currentPerson;
        public Person CurrentPerson
        {
            get => _currentPerson;
            set
            {
                _currentPerson = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(IDNumber));
                OnPropertyChanged(nameof(Gender));
                OnPropertyChanged(nameof(Age));
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(Allergies));
                OnPropertyChanged(nameof(HasProfile));
            }
        }

        public bool HasProfile => CurrentPerson != null;
        public string FullName => CurrentPerson != null ? $"{CurrentPerson.FirstName} {CurrentPerson.LastName}" : "";
        public string IDNumber => CurrentPerson?.IDNumber.ToString() ?? "";
        public string Gender => CurrentPerson?.Gender ?? "";
        public string Age => CurrentPerson != null ? $"{CurrentPerson.Age} years" : "";
        public string DateOfBirth => CurrentPerson?.DateOfBirth.ToString("dd MMMM yyyy") ?? "";
        public string Email => CurrentPerson?.email ?? "";
        public string Address => string.IsNullOrWhiteSpace(CurrentPerson?.Address) ? "Not provided" : CurrentPerson.Address;
        public string Allergies => string.IsNullOrWhiteSpace(CurrentPerson?.Allergies) ? "None recorded" : CurrentPerson.Allergies;

        // ----------------------------------------
        // Commands
        // ----------------------------------------
        public ICommand EditProfileCommand { get; }
        public ICommand CreateProfileCommand { get; }
        public ICommand BackCommand { get; }

        public ProfileDetailsPageViewModel(eHealthFolderDatabase database)
        {
            _database = database;

            EditProfileCommand = new Command(async () => await OnEditProfileAsync());
            CreateProfileCommand = new Command(async () => await Shell.Current.GoToAsync("MyProfilePage"));
            BackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            _ = LoadProfileAsync();
        }

        public async Task LoadProfileAsync()
        {
            var persons = await _database.GetPersonsAsync();
            CurrentPerson = persons.FirstOrDefault();
        }

        private async Task OnEditProfileAsync()
        {
            if (CurrentPerson != null)
            {
                await Shell.Current.GoToAsync($"MyProfilePage?PersonId={CurrentPerson.Id}");
            }
        }
    }
}
