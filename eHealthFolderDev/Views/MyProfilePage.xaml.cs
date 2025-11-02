using eHealthFolderDev.Services;
using eHealthFolderDev.ViewModels;

namespace eHealthFolderDev.Views;

[QueryProperty(nameof(PersonId), "PersonId")]
public partial class MyProfilePage : ContentPage
{
    private readonly MyProfilePageViewModel _viewModel;

    public int PersonId
    {
        set => _viewModel.PersonId = value;
    }

    public MyProfilePage(eHealthFolderDatabase database)
    {
        InitializeComponent();
        _viewModel = new MyProfilePageViewModel(database);
        BindingContext = _viewModel;
    }
}