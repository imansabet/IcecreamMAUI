﻿using IcecreamMAUI.Api.Services;
using IcecreamMAUI.Shared.Dtos;
using System.Security.Claims;

namespace IcecreamMAUI.Api.Endpoints;

public static class Endpoints
{
    private static Guid GetUserId(this ClaimsPrincipal principal) =>
       Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!); 

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapPost("api/auth/signup",
           async (SignupRequestDto dto, AuthService authService) =>
               TypedResults.Ok(await authService.SignupAsync(dto)));



        app.MapPost("api/auth/signin",
            async (SigninRequestDto dto, AuthService authService) =>
                TypedResults.Ok(await authService.SigninAsync(dto)));

        app.MapGet("/api/icecreams", async (IcecreamService icecreamService) =>
            TypedResults.Ok(await icecreamService.GetIcecreamsAsync()));


        app.MapPost("/api/auth/change-password", 
            async (ChangePasswordDto passwordDto ,ClaimsPrincipal principal ,AuthService authService ) =>
                TypedResults.Ok(await authService.ChangePasswordAsync(passwordDto,principal.GetUserId()))
        ).RequireAuthorization();


        var orderGroup =  app.MapGroup("/api/orders").RequireAuthorization();

        orderGroup.MapPost("/place-order",
            async (OrderPlaceDto dto, ClaimsPrincipal principal, OrderService orderService) =>
                await orderService.PlaceOrderAsync(dto, principal.GetUserId()));

        orderGroup.MapGet("",
            async (ClaimsPrincipal principal, OrderService orderService) =>
                TypedResults.Ok(await orderService.GetUserOrdersAsync(principal.GetUserId())));

        orderGroup.MapGet("/{orderId:long}/items",
            async (long orderId,ClaimsPrincipal principal, OrderService orderService) =>
                TypedResults.Ok(await orderService.GetUserOrderItemsAsync(orderId, principal.GetUserId())));

        return app;
    }
}
