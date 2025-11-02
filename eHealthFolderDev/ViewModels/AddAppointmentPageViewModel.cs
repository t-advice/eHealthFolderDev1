using System;
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
    public class AddAppointmentPageViewModel : INotifyPropertyChanged
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

        public Color Step1IndicatorColor => CurrentStep >= 1 ? Color.FromArgb("#FF9800") : Color.FromArgb("#E0E0E0");
        public Color Step2IndicatorColor => CurrentStep >= 2 ? Color.FromArgb("#FF9800") : Color.FromArgb("#E0E0E0");
        public Color Step3IndicatorColor => CurrentStep >= 3 ? Color.FromArgb("#FF9800") : Color.FromArgb("#E0E0E0");

        public string StepTitle => CurrentStep switch
        {
            1 => "Step 1: Location Details",
            2 => "Step 2: Date & Time",
            3 => "Step 3: Reason & Notes",
            _ => "Add Appointment"
        };

        public string BackButtonText => CurrentStep == 1 ? "Cancel" : "Back";
        public string NextButtonText => CurrentStep == 3 ? "Save Appointment" : "Next";

        // ----------------------------------------
        // Data model
        // ----------------------------------------
        private Appointments _newAppointment = new Appointments 
        { 
            Date = DateTime.Today
        };
        public Appointments NewAppointment
        {
            get => _newAppointment;
            set
            {
                _newAppointment = value;
                OnPropertyChanged();
            }
        }

        // TimeSpan for TimePicker binding
        private TimeSpan _appointmentTime = DateTime.Now.TimeOfDay;
        public TimeSpan AppointmentTime
        {
            get => _appointmentTime;
            set
            {
                _appointmentTime = value;
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

        public AddAppointmentPageViewModel(eHealthFolderDatabase database)
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
                if (string.IsNullOrWhiteSpace(NewAppointment.Hospital))
                {
                    await Application.Current.MainPage.DisplayAlert("Required", "Please enter hospital/clinic name", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(NewAppointment.Location))
                {
                    await Application.Current.MainPage.DisplayAlert("Required", "Please enter location/address", "OK");
                    return;
                }
                CurrentStep++;
            }
            else if (CurrentStep == 2)
            {
                if (NewAppointment.Date < DateTime.Today)
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Date", "Appointment date cannot be in the past", "OK");
                    return;
                }
                CurrentStep++;
            }
            else if (CurrentStep == 3)
            {
                // Save the appointment
                await SaveAppointmentAsync();
            }
        }

        private async Task SaveAppointmentAsync()
        {
            try
            {
                IsLoading = true;

                // Trim all string properties
                NewAppointment.Hospital = NewAppointment.Hospital?.Trim();
                NewAppointment.Location = NewAppointment.Location?.Trim();
                NewAppointment.Reason = NewAppointment.Reason?.Trim();
                NewAppointment.Notes = NewAppointment.Notes?.Trim();
                NewAppointment.Time = AppointmentTime.ToString(@"hh\:mm");

                await _database.SaveAppointmentAsync(NewAppointment);

                await Application.Current.MainPage.DisplayAlert("Success", "Appointment saved successfully!", "OK");

                // Reset for next entry
                NewAppointment = new Appointments { Date = DateTime.Today };
                AppointmentTime = DateTime.Now.TimeOfDay;
                CurrentStep = 1;

                // Navigate back to previous page
                await Shell.Current.GoToAsync("../..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save appointment: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
