using Microsoft.Extensions.Logging;
using RazorClassLib.Services;
using TicketsAreUs.Data;
using TicketsAreUs.Services;
using ZXing.Net.Maui.Controls;

namespace TicketsAreUs
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {


            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<IOccasionService, OccasionService>();
            builder.Services.AddSingleton<ITicketService, TicketService>();
            builder.Services.AddSingleton<IEnvironmentService, EnvironmentService>();
            builder.Services.AddScoped<SyncDatabase>();
            builder.Services.AddSingleton<HttpClient>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
