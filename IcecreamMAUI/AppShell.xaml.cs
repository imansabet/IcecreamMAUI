using IcecreamMAUI.Pages;

namespace IcecreamMAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
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
        await Shell.Current.DisplayAlert("Alert", "  Sign Out ? ", "Ok");
    }
}
