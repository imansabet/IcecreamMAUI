using Android.Service.QuickSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using IcecreamMAUI.Services;
using IcecreamMAUI.Shared.Dtos;
using Refit;

namespace IcecreamMAUI.ViewModels;


[QueryProperty(nameof(OrderId),nameof(OrderId))]
public partial class OrderDetailsViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    private readonly IOrderApi _orderApi;

    public OrderDetailsViewModel(AuthService authService , IOrderApi orderApi)
    {
        _authService = authService;
        _orderApi = orderApi;
    }

    [ObservableProperty]
    private long _orderId;

    [ObservableProperty]
    private OrderItemDto[] _orderItems = [];


    [ObservableProperty]
    private string _title = "Order Items";

    partial  void OnOrderIdChanged(long value)
    {
        Title = $"Order #{value}";
        LoadOrderItemsAsync(value);
    }

    private async Task LoadOrderItemsAsync(long orderId) 
    {
        IsBusy = true;
        try 
        {
            OrderItems = await _orderApi.GetMyOrderItemsAsync(orderId);
        }
        catch (ApiException ex) 
        {
            await HandleApiExceptionAsync(ex , () => _authService.Signout());
        }
        finally { IsBusy = false; }
    
    }
    
}
