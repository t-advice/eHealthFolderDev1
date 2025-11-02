using eHealthFolderDev.Models;

namespace eHealthFolderDev.Views.Details;

[QueryProperty(nameof(Appointment), "Appointment")]
public partial class AppointmentsDetailsPage : ContentPage
{
    private Appointments _appointment;
    public Appointments Appointment
    {
        get => _appointment;
        set
        {
            _appointment = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasNotes));
        }
    }

    public bool HasNotes => !string.IsNullOrWhiteSpace(Appointment?.Notes);

    public AppointmentsDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}