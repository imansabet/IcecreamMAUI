namespace IcecreamMAUI.Shared.Dtos;

public record struct IcecreameOptionDto( string Flavor,string Topping );
public record IcecreameDto(int Id, string Name,string Image , double Price  , IcecreameOptionDto[] Options);
