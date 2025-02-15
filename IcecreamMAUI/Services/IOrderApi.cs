using IcecreamMAUI.Shared.Dtos;
using Refit;

namespace IcecreamMAUI.Services;

[Headers("Authorization: Bearer")]
public  interface IOrderApi
{
    [Post("/api/order/place-order")]
    Task<ResultDto> PlaceOrderAsync(OrderPlaceDto dto);
}
