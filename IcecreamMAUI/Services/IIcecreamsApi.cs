using IcecreamMAUI.Shared.Dtos;
using Refit;

namespace IcecreamMAUI.Services;

public interface IIcecreamsApi
{
    [Get("/api/icecreams")]
    Task<IcecreameDto[]> GetIcecreamsAync();
} 