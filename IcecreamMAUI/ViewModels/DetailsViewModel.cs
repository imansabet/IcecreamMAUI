using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IcecreamMAUI.Models;
using IcecreamMAUI.Shared.Dtos;

namespace IcecreamMAUI.ViewModels;

// detailspage?icecream=Value
[QueryProperty(nameof(Icecream), nameof(Icecream))]
public partial class DetailsViewModel : BaseViewModel
{
    public DetailsViewModel(CartViewModel cartViewModel)
    {
        _cartViewModel = cartViewModel;
    }


    [ObservableProperty]
    private IcecreameDto? _icecream;


    [ObservableProperty]
    private int _quantity;

    [ObservableProperty]
    private IcecreamOption[] _options = [];
    private CartViewModel _cartViewModel;
    private readonly CartViewModel cartViewModel;

    partial void OnIcecreamChanged(IcecreameDto? value)
    {
        Options = [];
        if (value is null)
            return;
        Options = value.Options.Select(o => new IcecreamOption
        {
            Flavor = o.Flavor,
            Topping = o.Topping,
            IsSelected = false
        })
            .ToArray();
    }


    [RelayCommand]
    private void IncreaseQuantity() => Quantity++; 
    [RelayCommand]
    private void DecreaseQuantity() => Quantity--;

    [RelayCommand]
    private async Task GoBackAsync() => await GoToAsync("..",animate : true);

    [RelayCommand]
    private void SelectOption(IcecreamOption newOption)
    {
        var newIsSelected = !newOption.IsSelected;
        // Deselect All Options
        Options = [.. Options.Select(o => {o.IsSelected = false; return o; })];
        //Options = Options.Select(o =>
        //{
        //    o.IsSelected = false;
        //    return o;
        //}).ToList();
        newOption.IsSelected = newIsSelected;
    }

    [RelayCommand]
    private void AddToCart()
    {
        var selectedOption = Options.FirstOrDefault(o => o.IsSelected) ?? Options[0];
        _cartViewModel.AddItemToCart(Icecream!, Quantity, selectedOption.Flavor, selectedOption.Topping);
    }

}
