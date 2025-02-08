using System.Security.Cryptography;
using System.Text;

namespace IcecreamMAUI.Api.Services;

public class PasswordService
{
    private const int SaltSize = 10;
    public (string salt,string hashedPasssword) GenerateSaltAndHash(string plainPassword) 
    {
        if(string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentNullException(nameof(plainPassword));

        var buffer = RandomNumberGenerator.GetBytes(SaltSize);
        var salt = Convert.ToBase64String(buffer);    

        var hashedPasssword = GenerateHashedPassword(plainPassword,salt);

        return (salt, hashedPasssword);
    }

    public bool Compare(string plainPassword, string hashedPasssword, string salt) 
    {
        var newHashedPassword = GenerateHashedPassword(plainPassword, salt);
        return newHashedPassword == hashedPasssword;
    }

    private static string GenerateHashedPassword(string plainPassword, string salt) 
    {
        var bytes = Encoding.UTF8.GetBytes(plainPassword + salt);

        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
