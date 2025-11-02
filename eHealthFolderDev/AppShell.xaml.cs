using eHealthFolderDev.Views;
using eHealthFolderDev.Views.Details;

namespace eHealthFolderDev
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute(nameof(VisitsDetailsPage), typeof(VisitsDetailsPage));
            Routing.RegisterRoute(nameof(AppointmentsDetailsPage), typeof(AppointmentsDetailsPage));
            Routing.RegisterRoute(nameof(eHealthFolderDev.Views.Details.ProfileDetailsPage), typeof(eHealthFolderDev.Views.Details.ProfileDetailsPage));
            Routing.RegisterRoute(nameof(MyProfilePage), typeof(MyProfilePage));
            Routing.RegisterRoute(nameof(PersonFolderPage), typeof(PersonFolderPage));
            Routing.RegisterRoute(nameof(AddRecordsPage), typeof(AddRecordsPage));
            Routing.RegisterRoute(nameof(AddVisitPage), typeof(AddVisitPage));
            Routing.RegisterRoute(nameof(AddAppointmentPage), typeof(AddAppointmentPage));
        }
    }
}
