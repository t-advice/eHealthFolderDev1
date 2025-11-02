using eHealthFolderDev.Models;
using eHealthFolderDev.Services;

namespace eHealthFolderDev.Views;

public partial class DetailsPage : ContentPage
{
	private readonly eHealthFolderDatabase _database;
	private int currentStep = 1;

	public DetailsPage(eHealthFolderDatabase database)
	{
		InitializeComponent();
		_database = database;
		DatePicker.Date = DateTime.Today;
		UpdateUI();
	}

	private void UpdateUI()
	{
		// Update step visibility
		Step1.IsVisible = currentStep == 1;
		Step2.IsVisible = currentStep == 2;
		Step3.IsVisible = currentStep == 3;

		// Update progress indicators
		Step1Indicator.BackgroundColor = currentStep >= 1 ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");
		Step2Indicator.BackgroundColor = currentStep >= 2 ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");
		Step3Indicator.BackgroundColor = currentStep >= 3 ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");

		// Update step title
		StepTitle.Text = currentStep switch
		{
			1 => "Step 1: Basic Information",
			2 => "Step 2: Doctor Details",
			3 => "Step 3: Diagnosis & Notes",
			_ => "Add Visit"
		};

		// Update buttons
		BackButton.Text = currentStep == 1 ? "Cancel" : "Back";
		NextButton.Text = currentStep == 3 ? "Save Visit" : "Next";
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
			currentStep++;
			UpdateUI();
		}
		else if (currentStep == 2)
		{
			if (string.IsNullOrWhiteSpace(DoctorEntry.Text))
			{
				await DisplayAlert("Required", "Please enter doctor's name", "OK");
				return;
			}
			currentStep++;
			UpdateUI();
		}
		else if (currentStep == 3)
		{
			// Save the visit
			await SaveVisit();
		}
	}

	private async Task SaveVisit()
	{
		try
		{
			LoadingOverlay.IsVisible = true;

			var visit = new Visits
			{
				Hospital = HospitalEntry.Text?.Trim(),
				Date = DatePicker.Date,
				Doctor = DoctorEntry.Text?.Trim(),
				Department = DepartmentEntry.Text?.Trim(),
				Diagnosis = DiagnosisEditor.Text?.Trim(),
				Notes = NotesEditor.Text?.Trim()
			};

			await _database.SaveVisitAsync(visit);

			LoadingOverlay.IsVisible = false;

			await DisplayAlert("Success", "Visit saved successfully!", "OK");
			
			// Navigate back to home or person folder
			await Shell.Current.GoToAsync("../..");
		}
		catch (Exception ex)
		{
			LoadingOverlay.IsVisible = false;
			await DisplayAlert("Error", $"Failed to save visit: {ex.Message}", "OK");
		}
	}
}