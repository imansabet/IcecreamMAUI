using CommunityToolkit.Mvvm.ComponentModel;

namespace IcecreamMAUI.Models;

public partial class CartItem : ObservableObject
{
    public int Id { get; set; }
    public int IcecreamId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string FlavorName { get; set; }
    public string ToppingName { get; set; }



    [ObservableProperty,NotifyPropertyChangedFor(nameof(TotalPice))]
    private int _quantity;   // CommunityToolkit.Mvvm => Quantity

    public double TotalPice => Price * Quantity;

}
