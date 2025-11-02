namespace eHealthFolderDev.Views;

public partial class AddRecordsPage : ContentPage
{
	public AddRecordsPage()
	{
		InitializeComponent();
	}

	private void OnRecordTypeChanged(object sender, EventArgs e)
	{
		if (RecordTypePicker.SelectedIndex != -1)
		{
			SelectionFrame.IsVisible = true;
			ContinueButton.IsVisible = true;

			if (RecordTypePicker.SelectedIndex == 0) // Visit
			{
				SelectionLabel.Text = "📋 Visit Record";
				SelectionDescription.Text = "Record details about a past doctor's visit, including diagnosis, treatment, and notes.";
				SelectionFrame.BackgroundColor = Color.FromArgb("#E0F7FA");
				SelectionFrame.BorderColor = Color.FromArgb("#0097A7");
				SelectionLabel.TextColor = Color.FromArgb("#006064");
			}
			else // Appointment
			{
				SelectionLabel.Text = "📅 Appointment";
				SelectionDescription.Text = "Schedule an upcoming medical appointment with date, time, and location details.";
				SelectionFrame.BackgroundColor = Color.FromArgb("#FFF3E0");
				SelectionFrame.BorderColor = Color.FromArgb("#FF9800");
				SelectionLabel.TextColor = Color.FromArgb("#E65100");
			}
		}
	}

	private async void OnContinueClicked(object sender, EventArgs e)
	{
		if (RecordTypePicker.SelectedIndex == -1)
		{
			await DisplayAlert("Selection Required", "Please select a record type to continue.", "OK");
			return;
		}

		if (RecordTypePicker.SelectedIndex == 0) // Visit
		{
			await Shell.Current.GoToAsync("addvisit");
		}
		else // Appointment
		{
			await Shell.Current.GoToAsync("addappointment");
		}
	}

	private async void OnCancelClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}