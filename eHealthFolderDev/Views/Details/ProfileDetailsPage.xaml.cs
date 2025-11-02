using eHealthFolderDev.Services;
using eHealthFolderDev.ViewModels;

namespace eHealthFolderDev.Views.Details;

public partial class ProfileDetailsPage : ContentPage
{
    private readonly ProfileDetailsPageViewModel _viewModel;

    public ProfileDetailsPage(eHealthFolderDatabase database)
    {
        InitializeComponent();
        _viewModel = new ProfileDetailsPageViewModel(database);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProfileAsync();
    }
}