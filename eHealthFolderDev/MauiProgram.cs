using Microsoft.Extensions.Logging;
using eHealthFolderDev.Services;
using eHealthFolderDev.Views;
using eHealthFolderDev.Views.Details;

namespace eHealthFolderDev
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Register Database
            builder.Services.AddSingleton<eHealthFolderDatabase>(s =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "eHealthFolder.db3");
                return new eHealthFolderDatabase(dbPath);
            });

            // Register Pages
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<PersonFolderPage>();
            builder.Services.AddTransient<AddRecordsPage>();
            builder.Services.AddTransient<AddVisitPage>();
            builder.Services.AddTransient<AddAppointmentPage>();
            builder.Services.AddTransient<MyProfilePage>();
            builder.Services.AddTransient<SettingsPage>();
            
            // Register Detail Pages
            builder.Services.AddTransient<eHealthFolderDev.Views.Details.ProfileDetailsPage>();
            builder.Services.AddTransient<VisitsDetailsPage>();
            builder.Services.AddTransient<AppointmentsDetailsPage>();

            return builder.Build();
        }
    }
}
