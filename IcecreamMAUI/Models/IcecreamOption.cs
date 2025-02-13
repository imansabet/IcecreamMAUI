using CommunityToolkit.Mvvm.ComponentModel;

namespace IcecreamMAUI.Models;
 
public partial class IcecreamOption : ObservableObject
{
    public string Flavor { get; set; }
    public string Topping { get; set; }

    [ObservableProperty]
    private bool _isSelected; 
}
  