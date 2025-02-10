using IcecreamMAUI.Api.Data;
using IcecreamMAUI.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IcecreamMAUI.Api.Services;

public class IcecreamService(DataContext context )
{
    private readonly DataContext _context = context;
    public async Task<IcecreameDto[]> GetIcecreamsAsync() =>
        await _context.Icecreams.AsNoTracking()
        .Select(
            i => new IcecreameDto
            (   i.Id,
                i.Name,
                i.Image,
                i.Price,
                i.Options.Select(o => new IcecreameOptionDto(o.Flavor,o.Topping)).ToArray()))
        .ToArrayAsync();
}
