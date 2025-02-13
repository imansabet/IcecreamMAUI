using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IcecreamMAUI.Shared.Dtos;

namespace IcecreamMAUI.ViewModels;

// detailspage?icecream=Value
[QueryProperty(nameof(Icecream), nameof(Icecream))]
public partial class DetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private IcecreameDto? _icecream;


    [ObservableProperty]
    private int _quantity;

    [RelayCommand]
    private void IncreaseQuantity() => Quantity++; 
    [RelayCommand]
    private void DecreaseQuantity() => Quantity--;

    [RelayCommand]
    private async Task GoBackAsync() => await GoToAsync("..",animate : true);

}
