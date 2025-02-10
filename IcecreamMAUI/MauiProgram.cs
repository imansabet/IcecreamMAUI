using CommunityToolkit.Maui;
using IcecreamMAUI.Services;
using IcecreamMAUI.ViewModels;
using Microsoft.Extensions.Logging;
using Refit;

#if ANDROID
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
#elif IOS
using Security;
#endif

namespace IcecreamMAUI
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
                }).UseMauiCommunityToolkit();
                


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<AuthViewMode>();


            ConfigureRefit(builder.Services);

            return builder.Build();
        }

        private static void ConfigureRefit(IServiceCollection services) 
        {
            var refitSettings = new RefitSettings
            {
                HttpMessageHandlerFactory = () =>
                {
                    // Return HttpMessageHandler
#if ANDROID
                    return new Xamarin.Android.Net.AndroidMessageHandler
                    {
                        ServerCertificateCustomValidationCallback = (httpRequestMessage ,certificate , Chain , sslPolicyErrors) =>
                        {
                           return certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;
                        }
                    };
#elif IOS
                    return new NSUrlSessionHandler
                    {
                        TrustOverrideForUrl = (NSUrlSessionHandler sender , string url , SecTrust  trust)=>
                            url.StartsWith("htttps://loclhost")
                    };

#endif
                    return null;
                }
            };

            services.AddRefitClient<IAuthApi>(refitSettings)
                .ConfigureHttpClient(httpclient =>
                {
                    var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                        ? "https://10.0.2.2:7039"
                        : "https://localhost:7039";

                    httpclient.BaseAddress = new Uri(baseUrl);
                });
        }

    }
}
