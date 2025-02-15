﻿using IcecreamMAUI.Api.Data;
using IcecreamMAUI.Api.Data.Entities;
using IcecreamMAUI.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IcecreamMAUI.Api.Services;

public class OrderService(DataContext context )
{
    private readonly DataContext _context = context;

    public async Task<ResultDto> PlaceOrderAsync(OrderPlaceDto dto , Guid customerId)
    {
        var customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == customerId);
        if (customer is null)
            return ResultDto.Failure("Customer Does Not Exist .");

        var orderItems = dto.Items.Select(i =>
            new OrderItem
        {
            Flavor = i.Flavor,
            IcecreamId = i.IcecreamId,
            Name = i.Name,
            Price = i.Price,
            Quantity = i.Quantity,
            Topping = i.Topping,
            TotalPice = i.TotalPrice
        });

        var order = new Order
        {
            CustomerId = customerId,
            CustomerAddress = customer.Address,
            CustomerEmail = customer.Email,
            CustomerName = customer.Name,
            OrderedAt = DateTime.Now,
            TotalPrice = orderItems.Sum(i => i.TotalPice),
            Items = orderItems.ToArray()

        };
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return ResultDto.Success();

        }
        catch (Exception ex) 
        {
            return ResultDto.Failure(ex.Message);
        }


    }
}
