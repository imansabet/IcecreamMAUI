using IcecreamMAUI.Pages;
using IcecreamMAUI.Services;

namespace IcecreamMAUI;

public partial class AppShell : Shell
{
    private readonly AuthService _authService;
    public AppShell(AuthService authService)
    {
        InitializeComponent();
        RegisterRoutes();
        _authService = authService;
    }

    private readonly static Type[] _routablePageTypes = 
        [
            typeof(SigninPage),
            typeof(SignupPage),
            typeof(MyOrdersPage),
            typeof(OrderDetailsPage),
            typeof(DetailsPage)
        ];

    private static void RegisterRoutes() 
    {
        foreach (var pageType in _routablePageTypes) 
        {
            Routing.RegisterRoute(pageType.Name,pageType);
        }
    }

    private async void FlyoutFooter_Tapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://github.com/imansabet");
    }

    private async void SignOutMenuItem_Clicked(object sender, EventArgs e)
    {
        _authService.Signout();
        await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}");
    }
}
