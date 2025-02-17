using IcecreamMAUI.ViewModels;

namespace IcecreamMAUI.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;

    public ProfilePage(ProfileViewModel profileViewModel)
	{
		InitializeComponent();
		BindingContext = profileViewModel;
        _profileViewModel = profileViewModel;
    }
    protected override void OnAppearing()
    {
        _profileViewModel.Initialize();
    }
}