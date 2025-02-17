using IcecreamMAUI.ViewModels;

namespace IcecreamMAUI.Pages;

public partial class OrderDetailsPage : ContentPage
{

    public OrderDetailsPage(OrderDetailsViewModel orderDetailsViewModel)
	{
		InitializeComponent();
        BindingContext = orderDetailsViewModel;
    }
}