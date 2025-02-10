using IcecreamMAUI.ViewModels;

namespace IcecreamMAUI.Pages;

public partial class SigninPage : ContentPage
{
	public SigninPage(AuthViewModel authViewModel)
	{
		InitializeComponent();
		BindingContext = authViewModel;
	}

   
}