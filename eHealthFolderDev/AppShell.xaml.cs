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
        }
    }
}
