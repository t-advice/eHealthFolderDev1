using eHealthFolderDev.Models;
using eHealthFolderDev.Services;

namespace eHealthFolderDev.Views
{
    public partial class AddAppointmentPage : ContentPage
    {
        private readonly eHealthFolderDatabase _database;
        private int currentStep = 1;

        public AddAppointmentPage(eHealthFolderDatabase database)
        {
            InitializeComponent();
            _database = database;
            DatePicker.Date = DateTime.Today;
            TimePicker.Time = DateTime.Now.TimeOfDay;
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Update step visibility
            Step1.IsVisible = currentStep == 1;
            Step2.IsVisible = currentStep == 2;
            Step3.IsVisible = currentStep == 3;

            // Update progress indicators
            Step1Indicator.BackgroundColor = currentStep >= 1 ? Color.FromArgb("#FF9800") : Color.FromArgb("#E0E0E0");
            Step2Indicator.BackgroundColor = currentStep >= 2 ? Color.FromArgb("#FF9800") : Color.FromArgb("#E0E0E0");
            Step3Indicator.BackgroundColor = currentStep >= 3 ? Color.FromArgb("#FF9800") : Color.FromArgb("#E0E0E0");

            // Update step title
            StepTitle.Text = currentStep switch
            {
                1 => "Step 1: Location Details",
                2 => "Step 2: Date & Time",
                3 => "Step 3: Reason & Notes",
                _ => "Add Appointment"
            };

            // Update buttons
            BackButton.Text = currentStep == 1 ? "Cancel" : "Back";
            NextButton.Text = currentStep == 3 ? "Save Appointment" : "Next";
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            if (currentStep == 1)
            {
                // Cancel - go back to previous page
                bool confirm = await DisplayAlert("Cancel", "Are you sure you want to cancel? All data will be lost.", "Yes", "No");
                if (confirm)
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                // Go to previous step
                currentStep--;
                UpdateUI();
            }
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {
            // Validate current step
            if (currentStep == 1)
            {
                if (string.IsNullOrWhiteSpace(HospitalEntry.Text))
                {
                    await DisplayAlert("Required", "Please enter hospital/clinic name", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(LocationEntry.Text))
                {
                    await DisplayAlert("Required", "Please enter location/address", "OK");
                    return;
                }
                currentStep++;
                UpdateUI();
            }
            else if (currentStep == 2)
            {
                if (DatePicker.Date < DateTime.Today)
                {
                    await DisplayAlert("Invalid Date", "Appointment date cannot be in the past", "OK");
                    return;
                }
                currentStep++;
                UpdateUI();
            }
            else if (currentStep == 3)
            {
                // Save the appointment
                await SaveAppointment();
            }
        }

        private async Task SaveAppointment()
        {
            try
            {
                LoadingOverlay.IsVisible = true;

                var appointment = new Appointments
                {
                    Hospital = HospitalEntry.Text?.Trim(),
                    Location = LocationEntry.Text?.Trim(),
                    Date = DatePicker.Date,
                    Time = TimePicker.Time.ToString(@"hh\:mm"),
                    Reason = ReasonEditor.Text?.Trim(),
                    Notes = NotesEditor.Text?.Trim()
                };

                await _database.SaveAppointmentAsync(appointment);

                LoadingOverlay.IsVisible = false;

                await DisplayAlert("Success", "Appointment saved successfully!", "OK");
                
                // Navigate back to home or person folder
                await Shell.Current.GoToAsync("../..");
            }
            catch (Exception ex)
            {
                LoadingOverlay.IsVisible = false;
                await DisplayAlert("Error", $"Failed to save appointment: {ex.Message}", "OK");
            }
        }
    }
}