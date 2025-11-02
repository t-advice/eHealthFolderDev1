using eHealthFolderDev.Models;
using eHealthFolderDev.Services;
using eHealthFolderDev.ViewModels;
using System.ComponentModel;


namespace eHealthFolderDev.Views;

public partial class PersonFolderPage : ContentPage
{
    private readonly PersonFolderPageViewModel _viewModel;

    public PersonFolderPage(eHealthFolderDatabase database)
    {
        InitializeComponent();
        _viewModel = new PersonFolderPageViewModel(database);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Refresh data when page appears (e.g., after adding new records)
        await _viewModel.RefreshDataAsync();
    }
}





