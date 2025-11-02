using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using eHealthFolderDev.Models;
using eHealthFolderDev.Services;
using eHealthFolderDev.Views.Details;

namespace eHealthFolderDev.ViewModels
{
    public class PersonFolderPageViewModel : INotifyPropertyChanged
    {
        private readonly eHealthFolderDatabase _database;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ----------------------------------------
        // Collections
        // ----------------------------------------
        private ObservableCollection<Visits> _visits = new();
        public ObservableCollection<Visits> Visits
        {
            get => _visits;
            set
            {
                _visits = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Appointments> _appointments = new();
        public ObservableCollection<Appointments> Appointments
        {
            get => _appointments;
            set
            {
                _appointments = value;
                OnPropertyChanged();
            }
        }

        // ----------------------------------------
        // Tab Visibility
        // ----------------------------------------
        private bool _isVisitsVisible = true;
        public bool IsVisitsVisible
        {
            get => _isVisitsVisible;
            set
            {
                _isVisitsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isAppointmentsVisible = false;
        public bool IsAppointmentsVisible
        {
            get => _isAppointmentsVisible;
            set
            {
                _isAppointmentsVisible = value;
                OnPropertyChanged();
            }
        }

        // Button colors
        public Color VisitsButtonColor => IsVisitsVisible ? Color.FromArgb("#A5D6A7") : Color.FromArgb("#E0E0E0");
        public Color AppointmentsButtonColor => IsAppointmentsVisible ? Color.FromArgb("#FFB74D") : Color.FromArgb("#E0E0E0");

        // ----------------------------------------
        // Commands
        // ----------------------------------------
        public ICommand ShowVisitsCommand { get; }
        public ICommand ShowAppointmentsCommand { get; }
        public ICommand VisitTappedCommand { get; }
        public ICommand AppointmentTappedCommand { get; }

        public PersonFolderPageViewModel(eHealthFolderDatabase database)
        {
            _database = database;
            
            ShowVisitsCommand = new Command(() =>
            {
                IsVisitsVisible = true;
                IsAppointmentsVisible = false;
                OnPropertyChanged(nameof(VisitsButtonColor));
                OnPropertyChanged(nameof(AppointmentsButtonColor));
            });

            ShowAppointmentsCommand = new Command(() =>
            {
                IsVisitsVisible = false;
                IsAppointmentsVisible = true;
                OnPropertyChanged(nameof(VisitsButtonColor));
                OnPropertyChanged(nameof(AppointmentsButtonColor));
            });

            VisitTappedCommand = new Command<Visits>(async (visit) => await OnVisitTapped(visit));
            AppointmentTappedCommand = new Command<Appointments>(async (appointment) => await OnAppointmentTapped(appointment));

            // Load data
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            await LoadVisitsAsync();
            await LoadAppointmentsAsync();
        }

        private async Task LoadVisitsAsync()
        {
            var visitsList = await _database.GetVisitsAsync();
            Visits.Clear();
            foreach (var visit in visitsList)
            {
                Visits.Add(visit);
            }
        }

        private async Task LoadAppointmentsAsync()
        {
            var appointmentsList = await _database.GetAppointmentsAsync();
            Appointments.Clear();
            foreach (var appointment in appointmentsList)
            {
                Appointments.Add(appointment);
            }
        }

        private async Task OnVisitTapped(Visits visit)
        {
            if (visit == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Visit", visit }
            };

            await Shell.Current.GoToAsync(nameof(VisitsDetailsPage), navigationParameter);
        }

        private async Task OnAppointmentTapped(Appointments appointment)
        {
            if (appointment == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Appointment", appointment }
            };

            await Shell.Current.GoToAsync(nameof(AppointmentsDetailsPage), navigationParameter);
        }

        // Method to refresh data when returning from add pages
        public async Task RefreshDataAsync()
        {
            await LoadDataAsync();
        }
    }
}
