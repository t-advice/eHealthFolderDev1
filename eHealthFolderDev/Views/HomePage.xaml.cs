using eHealthFolderDev.Models;
using eHealthFolderDev.Services;

namespace eHealthFolderDev.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly eHealthFolderDatabase _database;
        private List<Visits> _visits;
        private List<Appointments> _appointments;

        public HomePage(eHealthFolderDatabase db)
        {
            InitializeComponent();
            _database = db;
            InitializeDataAsync();
        }

        // Initialize and load data from the database
        private async void InitializeDataAsync()
        {
            // Optional: Seed dummy data for testing
            //await _database.SeedDataAsync();
            await LoadDataAsync();
        }

        // Load all visits & appointments from database
        private async Task LoadDataAsync()
        {
            _visits = await _database.GetVisitsAsync();
            _appointments = await _database.GetAppointmentsAsync();

            // Show the most recent ones first
            _visits = _visits.OrderByDescending(v => v.Date).ToList();
            _appointments = _appointments.OrderByDescending(a => a.Date).ToList();

            VisitsCollectionView.ItemsSource = _visits;
            AppointmentsCollectionView.ItemsSource = _appointments;

            System.Diagnostics.Debug.WriteLine($"Loaded {_visits.Count} visits, {_appointments.Count} appointments");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = LoadDataAsync(); // reload every time page appears
        }

        private async void AddRecord_Clicked(object sender, EventArgs e)
        {
            await AddRecordButton.ScaleTo(0.9, 100);
            await AddRecordButton.ScaleTo(1, 100);
            await Shell.Current.GoToAsync("AddRecordsPage");
        }

        // Visit tapped
        private async void VisitItem_Tapped(object sender, EventArgs e)
        {
            if (sender is BindableObject bind && bind.BindingContext is Visits)
            {
                await Navigation.PushAsync(new PersonFolderPage(_database));
            }
        }

        // Appointment tapped
        private async void AppointmentItem_Tapped(object sender, EventArgs e)
        {
            if (sender is BindableObject bind && bind.BindingContext is Appointments)
            {
                await Navigation.PushAsync(new PersonFolderPage(_database));
            }
        }

        // Delete a Visit
        private async void VisitDeleted_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is ImageButton imageButton && imageButton.BindingContext is Visits visit)
                {
                    bool confirm = await DisplayAlert("Delete Visit",
                        "Are you sure you want to delete this visit?",
                        "Yes", "No");

                    if (confirm)
                    {
                        await imageButton.ScaleTo(0.9, 100);
                        await imageButton.ScaleTo(1, 100);

                        await _database.DeleteVisitAsync(visit);
                        _visits.Remove(visit);

                        VisitsCollectionView.ItemsSource = null;
                        VisitsCollectionView.ItemsSource = _visits;

                        await DisplayAlert("Success", "Visit deleted successfully.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete visit: {ex.Message}", "OK");
            }
        }

        // Delete an Appointment
        private async void AppointmentDeleted_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is ImageButton imageButton && imageButton.BindingContext is Appointments appointment)
                {
                    bool confirm = await DisplayAlert("Delete Appointment",
                        "Are you sure you want to delete this appointment?",
                        "Yes", "No");

                    if (confirm)
                    {
                        await imageButton.ScaleTo(0.9, 100);
                        await imageButton.ScaleTo(1, 100);

                        await _database.DeleteAppointmentAsync(appointment);
                        _appointments.Remove(appointment);

                        AppointmentsCollectionView.ItemsSource = null;
                        AppointmentsCollectionView.ItemsSource = _appointments;

                        await DisplayAlert("Success", "Appointment deleted successfully.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete appointment: {ex.Message}", "OK");
            }
        }
    }
}

