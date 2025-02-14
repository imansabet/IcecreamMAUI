using IcecreamMAUI.Models;
using IcecreamMAUI.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcecreamMAUI.ViewModels;

public partial class CartViewModel : BaseViewModel
{
    public ObservableCollection<CartItem> CartItems { get; set; } = [];

    public static int TotalCartCount { get; set; }
    // whenever we are modifying quantity we should raise this
    public static event EventHandler<int>? TotalCartCountChanged;
    
    public async void AddItemToCart(IcecreameDto icecream,int quantity, string flavor , string topping)
    {
        var existingItems = CartItems.FirstOrDefault(ci => ci.IcecreamId == icecream.Id);
        if (existingItems is not null) 
        {
            if (quantity <= 0)
            {
                CartItems.Remove(existingItems);
                await ShowToastAsync("Icecream Removed From The Cart");
            }
            else 
            {
                existingItems.Quantity = quantity;
                await ShowToastAsync("Quantity Updated .");
            }

        }
        else
        {
            var cartItem = new CartItem
            {
                FlavorName = flavor,
                IcecreamId = icecream.Id,
                Name = icecream.Name,
                Price = icecream.Price,
                Quantity = quantity,
                ToppingName = topping
            };
            CartItems.Add(cartItem);
            await ShowToastAsync("Such a Yummy Choice ! ");
        }

        TotalCartCount = CartItems.Sum(i => i.Quantity);
        TotalCartCountChanged?.Invoke(null,TotalCartCount);
    }


}
