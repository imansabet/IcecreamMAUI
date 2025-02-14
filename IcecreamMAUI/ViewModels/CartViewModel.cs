using CommunityToolkit.Mvvm.Input;
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

        NotifyCartCountChange();
    }

    private void NotifyCartCountChange()
    {
        TotalCartCount = CartItems.Sum(i => i.Quantity);
        TotalCartCountChanged?.Invoke(null, TotalCartCount);
    }

    public int GetItemCartCount(int icecreamId)
    {
        var existingItem = CartItems.FirstOrDefault(i => i.IcecreamId == icecreamId);
        return existingItem?.Quantity ?? 0;
    }

    public async Task InitialzeCartAsync()
    {
        var dbItems = await _databaseService.GetAllCartItemsAsync();
        foreach(var dbItem in dbItems)
        {
            CartItems.Add(dbItem.ToCartItemModel());
        }
        NotifyCartCountChange();
    }

    [RelayCommand]
    private async Task ClearCartAsync()
    {
        if(CartItems.Count == 0)
        {
            await ShowAlertAsync("Empty Cart","There Are Now items In The Cart .");
            return;
        }
        if(await ConfirmAsync("Remove Shopping Cart", "Remove All Items ??"))
        {
            await _databaseService.ClearCartAsync();
            CartItems.Clear();
            await ShowToastAsync("Cart Removed .");
            NotifyCartCountChange();
        };
    }

    [RelayCommand]
    private async Task RemoveCartItemAsync(int cartItemId)
    {
        if (await ConfirmAsync("Remove This Item ? ", "Remove This Item From Cart ??"))
        {
            var existingItem = CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (existingItem is null)
                return;

            CartItems.Remove(existingItem);

            var dbCartItem = await _databaseService.GetCartItemAsync(existingItem.Id);
            if (dbCartItem is null)
                return;

            await _databaseService.DeleteCartItem(dbCartItem);

            await ShowToastAsync("Item Removed .");
            NotifyCartCountChange();
        };
    }




}
