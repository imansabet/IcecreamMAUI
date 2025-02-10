using IcecreamMAUI.Shared.Dtos;
using System.Text.Json;

namespace IcecreamMAUI.Services;

public class AuthService
{
    private const string AuthKey = "AuthKey";
    public LoggedInUser? User { get; private set; }
    public string? Token { get; private set; }
    public void Signin(AuthResponseDto dto) 
    {
        var serializedDto = JsonSerializer.Serialize(dto);
        Preferences.Default.Set(AuthKey, serializedDto);
        
        (User, Token) = dto;
    }

    public void Initialize() 
    {
        if (Preferences.Default.ContainsKey(AuthKey)) 
        {
            var serialzed = Preferences.Default.Get<string?>(AuthKey,null);
            if(string.IsNullOrWhiteSpace(serialzed)) 
            {
                Preferences.Default.Remove(AuthKey);
            }
            else
            {
                (User, Token) = JsonSerializer.Deserialize<AuthResponseDto>(serialzed)!;
            }
        }
    }

    public void Signout()
    {
        Preferences.Default.Remove(AuthKey);
        (User, Token) = (null, null);

    }
}
