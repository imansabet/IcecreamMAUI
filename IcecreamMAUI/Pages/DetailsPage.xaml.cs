using IcecreamMAUI.ViewModels;

namespace IcecreamMAUI.Pages;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel detailsViewModel)
    {
		InitializeComponent();
		BindingContext = detailsViewModel;
	}
}