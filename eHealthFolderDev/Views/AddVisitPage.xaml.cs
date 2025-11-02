using eHealthFolderDev.Services;
using eHealthFolderDev.ViewModels;

namespace eHealthFolderDev.Views;

public partial class AddVisitPage : ContentPage
{
	public AddVisitPage(eHealthFolderDatabase database)
	{
		InitializeComponent();
		BindingContext = new AddVisitPageViewModel(database);
	}
}