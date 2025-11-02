using eHealthFolderDev.Models;
using eHealthFolderDev.Services;
using System.ComponentModel;


namespace eHealthFolderDev.Views;

public partial class PersonFolderPage : ContentPage
{
	private readonly eHealthFolderDatabase _database;
    public PersonFolderPage(eHealthFolderDatabase database)
	{
		InitializeComponent();
		_database = database;

		LoadVisits();
		LoadAppointments();
	}
	private async void LoadVisits()
	{
		var visits = await _database.GetVisitsAsync();
		VisitsCollectionView.ItemsSource = visits;
	}
    private async void LoadAppointments()
    {
        var appointments = await _database.GetAppointmentsAsync();
        AppointmentsCollectionView.ItemsSource = appointments;
    }
    private void VisitsButton_Clicked(object sender, EventArgs e)
    {
        VisitsCollectionView.IsVisible = true;
        AppointmentsCollectionView.IsVisible = false;

        VisitsButton.BackgroundColor = Color.FromArgb("#A5D6A7");
        AppointmentsButton.BackgroundColor = Color.FromArgb("#E0F7FA");
    }
    private void AppointmentsButton_Clicked(object sender, EventArgs e)
    {
        VisitsCollectionView.IsVisible = false;
        AppointmentsCollectionView.IsVisible = true;

        VisitsButton.BackgroundColor = Color.FromArgb("#E0F7FA");
        AppointmentsButton.BackgroundColor = Color.FromArgb("#FFF3E0");
    }

    private async void VisitItem_Tapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        if (frame?.BindingContext is Visits visit)
        {
            await DisplayAlert("Visit Details",
                $"Doctor: {visit.Doctor}\nDiagnosis: {visit.Diagnosis}\nNotes: {visit.Notes}",
                "OK");
        }
    }

    private async void AppointmentItem_Tapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        if (frame?.BindingContext is Appointments appointment)
        {
            await DisplayAlert("Appointment Details",
                $"Hospital: {appointment.Hospital}\nReason: {appointment.Reason}\nDate: {appointment.Date:dd/MM/yyyy}",
                "OK");
        }
    }
}





