using IcecreamMAUI.ViewModels;

namespace IcecreamMAUI.Pages;

public partial class CartPage : ContentPage
{

    public CartPage(CartViewModel cartViewModel)
	{
		InitializeComponent();
        BindingContext = cartViewModel;
    }
}