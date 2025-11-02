using eHealthFolderDev.Models;

namespace eHealthFolderDev.Views.Details;

[QueryProperty(nameof(Visit), "Visit")]
public partial class VisitsDetailsPage : ContentPage
{
    private Visits _visit;
    public Visits Visit
    {
        get => _visit;
        set
        {
            _visit = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasDepartment));
            OnPropertyChanged(nameof(HasDiagnosis));
            OnPropertyChanged(nameof(HasNotes));
        }
    }

    public bool HasDepartment => !string.IsNullOrWhiteSpace(Visit?.Department);
    public bool HasDiagnosis => !string.IsNullOrWhiteSpace(Visit?.Diagnosis);
    public bool HasNotes => !string.IsNullOrWhiteSpace(Visit?.Notes);

    public VisitsDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}