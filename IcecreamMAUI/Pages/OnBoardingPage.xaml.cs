using IcecreamMAUI.Services;

namespace IcecreamMAUI.Pages;

public partial class OnboardingPage : ContentPage
{
    private readonly AuthService _authService;

    public OnboardingPage(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
    }

    protected override async void OnAppearing()
    {
        if (_authService.User is not null
            && _authService.User.Id != default(Guid)
            && !string.IsNullOrWhiteSpace(_authService.Token))
        {
            // user is logged in =>>>> so navigate to home page
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
 
        }
    }


    private async void Signin_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SigninPage));

    }

    private async void Signup_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignupPage));

    }

    //private async void Button_Clicked_1(object sender, EventArgs e)
    //{
     //   await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    //}
}