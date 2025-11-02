using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using eHealthFolderDev.Models;
using eHealthFolderDev.Services;

namespace eHealthFolderDev.ViewModels
{
    public class AddVisitPageViewModel : INotifyPropertyChanged
    {
        private readonly eHealthFolderDatabase _database;
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ----------------------------------------
        // Multi-step logic
        // ----------------------------------------
        private int _currentStep = 1;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (_currentStep != value)
                {
                    _currentStep = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsStep1));
                    OnPropertyChanged(nameof(IsStep2));
                    OnPropertyChanged(nameof(IsStep3));
                    OnPropertyChanged(nameof(Step1IndicatorColor));
                    OnPropertyChanged(nameof(Step2IndicatorColor));
                    OnPropertyChanged(nameof(Step3IndicatorColor));
                    OnPropertyChanged(nameof(StepTitle));
                    OnPropertyChanged(nameof(BackButtonText));
                    OnPropertyChanged(nameof(NextButtonText));
                }
            }
        }

        public bool IsStep1 => CurrentStep == 1;
        public bool IsStep2 => CurrentStep == 2;
        public bool IsStep3 => CurrentStep == 3;

        public Color Step1IndicatorColor => CurrentStep >= 1 ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");
        public Color Step2IndicatorColor => CurrentStep >= 2 ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");
        public Color Step3IndicatorColor => CurrentStep >= 3 ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");

        public string StepTitle => CurrentStep switch
        {
            1 => "Step 1: Basic Information",
            2 => "Step 2: Doctor Details",
            3 => "Step 3: Diagnosis & Notes",
            _ => "Add Visit"
        };

        public string BackButtonText => CurrentStep == 1 ? "Cancel" : "Back";
        public string NextButtonText => CurrentStep == 3 ? "Save Visit" : "Next";

        // ----------------------------------------
        // Data model
        // ----------------------------------------
        private Visits _newVisit = new Visits { Date = DateTime.Today };
        public Visits NewVisit
        {
            get => _newVisit;
            set
            {
                _newVisit = value;
                OnPropertyChanged();
            }
        }

        // ----------------------------------------
        // Loading state
        // ----------------------------------------
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

        // ----------------------------------------
        // Commands
        // ----------------------------------------
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }

        public AddVisitPageViewModel(eHealthFolderDatabase database)
        {
            _database = database;
            BackCommand = new Command(async () => await OnBackAsync());
            NextCommand = new Command(async () => await OnNextAsync());
        }

        private async Task OnBackAsync()
        {
            if (CurrentStep == 1)
            {
                // Cancel - go back to previous page
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Cancel", 
                    "Are you sure you want to cancel? All data will be lost.", 
                    "Yes", 
                    "No");
                
                if (confirm)
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                // Go to previous step
                CurrentStep--;
            }
        }

        private async Task OnNextAsync()
        {
            // Validate current step
            if (CurrentStep == 1)
            {
                if (string.IsNullOrWhiteSpace(NewVisit.Hospital))
                {
                    await Application.Current.MainPage.DisplayAlert("Required", "Please enter hospital/clinic name", "OK");
                    return;
                }
                CurrentStep++;
            }
            else if (CurrentStep == 2)
            {
                if (string.IsNullOrWhiteSpace(NewVisit.Doctor))
                {
                    await Application.Current.MainPage.DisplayAlert("Required", "Please enter doctor's name", "OK");
                    return;
                }
                CurrentStep++;
            }
            else if (CurrentStep == 3)
            {
                // Save the visit
                await SaveVisitAsync();
            }
        }

        private async Task SaveVisitAsync()
        {
            try
            {
                IsLoading = true;

                // Trim all string properties
                NewVisit.Hospital = NewVisit.Hospital?.Trim();
                NewVisit.Doctor = NewVisit.Doctor?.Trim();
                NewVisit.Department = NewVisit.Department?.Trim();
                NewVisit.Diagnosis = NewVisit.Diagnosis?.Trim();
                NewVisit.Notes = NewVisit.Notes?.Trim();

                await _database.SaveVisitAsync(NewVisit);

                await Application.Current.MainPage.DisplayAlert("Success", "Visit saved successfully!", "OK");

                // Reset for next entry
                NewVisit = new Visits { Date = DateTime.Today };
                CurrentStep = 1;

                // Navigate back to previous page
                await Shell.Current.GoToAsync("../..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save visit: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}