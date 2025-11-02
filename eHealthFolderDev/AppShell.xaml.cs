using eHealthFolderDev.Views;

namespace eHealthFolderDev
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute("addrecords", typeof(AddRecordsPage));
            Routing.RegisterRoute("addvisit", typeof(AddVisitsPage));
            Routing.RegisterRoute("addappointment", typeof(AddAppointmentsPage));
        }
    }
}
