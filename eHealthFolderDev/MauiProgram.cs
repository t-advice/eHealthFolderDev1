using Microsoft.Extensions.Logging;
using eHealthFolderDev.Services;

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

            builder.Services.AddSingleton<eHealthFolderDatabase>(s =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "eHealthFolder.db3");
                return new eHealthFolderDatabase(dbPath);
            });



            return builder.Build();
        }
    }
}
