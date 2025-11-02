using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using eHealthFolderDev.Models;
using eHealthFolderDev.Services;

namespace eHealthFolderDev.ViewModels
{
    public class MyProfilePageViewModel : INotifyPropertyChanged
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
            }
        }

        private string _pageTitle = "Create Profile";
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        private string _saveButtonText = "Save Profile";
        public string SaveButtonText
        {
            get => _saveButtonText;
            set
            {
                _saveButtonText = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private int _personId;
        public int PersonId
        {
            get => _personId;
            set
            {
                _personId = value;
                _ = LoadPersonDataAsync();
            }
        }

        // ----------------------------------------
        // Commands
        // ----------------------------------------
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public MyProfilePageViewModel(eHealthFolderDatabase database)
        {
            _database = database;
            CurrentPerson = new Person { DateOfBirth = DateTime.Today.AddYears(-30) };
            
            SaveCommand = new Command(async () => await SaveProfileAsync());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task LoadPersonDataAsync()
        {
            if (_personId > 0)
            {
                var persons = await _database.GetPersonsAsync();
                var person = persons.FirstOrDefault(p => p.Id == _personId);

                if (person != null)
                {
                    CurrentPerson = person;
                    PageTitle = "Edit Profile";
                    SaveButtonText = "Update Profile";
                }
            }
        }

        private async Task SaveProfileAsync()
        {
            // Validation
            if (string.IsNullOrWhiteSpace(CurrentPerson.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Required", "Please enter your first name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentPerson.LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Required", "Please enter your last name", "OK");
                return;
            }

            if (CurrentPerson.IDNumber <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Required", "Please enter a valid ID number", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentPerson.email))
            {
                await Application.Current.MainPage.DisplayAlert("Required", "Please enter your email", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentPerson.Gender))
            {
                await Application.Current.MainPage.DisplayAlert("Required", "Please select your gender", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                // Trim string properties
                CurrentPerson.FirstName = CurrentPerson.FirstName?.Trim();
                CurrentPerson.LastName = CurrentPerson.LastName?.Trim();
                CurrentPerson.Address = CurrentPerson.Address?.Trim();
                CurrentPerson.Allergies = CurrentPerson.Allergies?.Trim();
                CurrentPerson.email = CurrentPerson.email?.Trim();

                // Calculate age
                CurrentPerson.Age = CalculateAge(CurrentPerson.DateOfBirth).ToString();

                await _database.SavePersonAsync(CurrentPerson);

                await Application.Current.MainPage.DisplayAlert("Success", "Profile saved successfully!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save profile: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
