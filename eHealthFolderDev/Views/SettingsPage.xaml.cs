namespace eHealthFolderDev.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

	private async void OnMyProfileTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("ProfileDetailsPage");
	}
}

