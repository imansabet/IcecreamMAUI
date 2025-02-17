using CommunityToolkit.Maui;
using IcecreamMAUI.Pages;
using IcecreamMAUI.Services;
using IcecreamMAUI.ViewModels;
using Microsoft.Extensions.Logging;
using Refit;

#if ANDROID
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
#elif IOS
using Security;
using System.Net.Http;
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
                })
                .UseMauiCommunityToolkit()
                .ConfigureMauiHandlers(h =>
                {
#if ANDROID
                    h.AddHandler<Shell, Platforms.Android.TabbarBadgeRenderer>();
#elif IOS
                    h.AddHandler<Shell, TabbarBadgeRenderer>();
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddTransient<AuthViewModel>()
                .AddTransient<SignupPage>()
                .AddTransient<SigninPage>();

            builder.Services.AddSingleton<AuthService>();

            builder.Services.AddTransient<OnboardingPage>();

            builder.Services.AddSingleton<HomeViewModel>()
                .AddSingleton<HomePage>();

            builder.Services.AddTransient<DetailsViewModel>()
                .AddTransient<DetailsPage>();

            builder.Services.AddSingleton<CartViewModel>()
                .AddTransient<CartPage>();

            builder.Services.AddSingleton<ProfileViewModel>()
               .AddTransient<ProfilePage>();

            builder.Services.AddSingleton<OrdersViewModel>()
               .AddTransient<MyOrdersPage>();

            ConfigureRefit(builder.Services);

            return builder.Build();
        }

        private static void ConfigureRefit(IServiceCollection services)
        {

            services.AddRefitClient<IAuthApi>(GetRefitSettings)
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IIcecreamsApi>(GetRefitSettings)
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IOrderApi>(GetRefitSettings)
              .ConfigureHttpClient(SetHttpClient);


            static void SetHttpClient(HttpClient httpClient)
            {
                var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                    ? "https://10.0.2.2:7039"
                    : "https://localhost:7039";

                httpClient.BaseAddress = new Uri(baseUrl);
            }

            static RefitSettings GetRefitSettings(IServiceProvider serviceProvider)
            {
                var authService = serviceProvider.GetRequiredService<AuthService>();

                var refitSettings = new RefitSettings
                {
                    HttpMessageHandlerFactory = () =>
                    {
#if ANDROID
                    return new Xamarin.Android.Net.AndroidMessageHandler
                    {
                        ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
                        {
                            return certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;
                        }
                    };
#elif IOS
                    return new NSUrlSessionHandler
                    {
                        TrustOverrideForUrl = (NSUrlSessionHandler sender, string url, SecTrust trust) =>
                            url.StartsWith("https://localhost")
                    };
#endif
                        return null;
                    },
                    AuthorizationHeaderValueGetter = (_, __) =>
                        Task.FromResult(authService.Token ?? string.Empty)  // return Jwt
                    
                };
                return refitSettings;

            }
        }
    }
}
