using eHealthFolderDev.Models;
using eHealthFolderDev.Services;
using eHealthFolderDev.ViewModels;

namespace eHealthFolderDev.Views
{
    public partial class AddAppointmentPage : ContentPage
    {
        public AddAppointmentPage(eHealthFolderDatabase database)
        {
            InitializeComponent();
            BindingContext = new AddAppointmentPageViewModel(database);
        }
    }
}