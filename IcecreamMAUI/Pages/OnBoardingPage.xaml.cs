namespace IcecreamMAUI.Pages;

public partial class OnboardingPage : ContentPage
{
	public OnboardingPage()
	{
		InitializeComponent();
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