﻿using IcecreamMAUI.Models;
using SQLite;

namespace IcecreamMAUI.Data;

public  class CartItemEntity
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public int IcecreamId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string FlavorName { get; set; }
    public string ToppingName { get; set; }
    public int Quantity { get; set; }

    public CartItemEntity(CartItem cartItemModel)
    {
        IcecreamId = cartItemModel.IcecreamId;  
        Name = cartItemModel.Name;
        Price = cartItemModel.Price;
        FlavorName = cartItemModel.FlavorName;
        ToppingName = cartItemModel.ToppingName;
        Quantity = cartItemModel.Quantity;
    }
    public CartItemEntity()
    {
        
    }
    public CartItem ToCartItemModel() => new ()
    {
        Id = Id,
        Name = Name,
        Price = Price,
        FlavorName = FlavorName,
        ToppingName = ToppingName,
        IcecreamId = IcecreamId,
        Quantity = Quantity
    };
}
