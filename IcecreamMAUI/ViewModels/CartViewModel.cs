using IcecreamMAUI.Models;
using IcecreamMAUI.Services;
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
    private readonly DatabaseService _databaseService;

    public CartViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
    public ObservableCollection<CartItem> CartItems { get; set; } = [];

    public static int TotalCartCount { get; set; }
    // whenever we are modifying quantity we should raise this
    public static event EventHandler<int>? TotalCartCountChanged;
    
    public async void AddItemToCart(IcecreameDto icecream,int quantity, string flavor , string topping)
    {
        var existingItems = CartItems.FirstOrDefault(ci => ci.IcecreamId == icecream.Id);
        if (existingItems is not null) 
        {
            var dbCartItem = await _databaseService.GetCartItemAsync(existingItems.Id);
            if (quantity <= 0)
            {
                await _databaseService.DeleteCartItem(dbCartItem);

                CartItems.Remove(existingItems);
                await ShowToastAsync("Icecream Removed From The Cart");
            }
            else 
            {
                dbCartItem.Quantity = quantity;

                await _databaseService.UpdateCartItem(dbCartItem);

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

            var entity = new Data.CartItemEntity(cartItem);
            await _databaseService.AddCartItem(entity);
            
            cartItem.Id = entity.Id;

            CartItems.Add(cartItem);

            await ShowToastAsync("Such a Yummy Choice ! ");
        }

        TotalCartCount = CartItems.Sum(i => i.Quantity);
        TotalCartCountChanged?.Invoke(null,TotalCartCount);
    }

    public int GetItemCartCount(int icecreamId)
    {
        var existingItem = CartItems.FirstOrDefault(i => i.IcecreamId == icecreamId);
        return existingItem?.Quantity ?? 0;
    }


}
