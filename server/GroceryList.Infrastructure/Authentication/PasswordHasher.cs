using GroceryList.Application.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace GroceryList.Infrastructure.Authentication;
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 350000;

    private static readonly KeyDerivationPrf _keyDerivationPrf = KeyDerivationPrf.HMACSHA512;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] subkey = KeyDerivation.Pbkdf2(password, salt, _keyDerivationPrf, Iterations, KeySize);

        var outputBytes = new byte[13 + salt.Length + subkey.Length];
        outputBytes[0] = 0x01; // format marker
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 13 + SaltSize, subkey.Length);
        return Convert.ToBase64String(outputBytes);
    }

    public bool Verify(string passwordHash, string password)
    {
        var savedHash = Convert.FromBase64String(passwordHash);

        try
        {
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(savedHash, 13, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 128 bits
            int subkeyLength = savedHash.Length - 13 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            byte[] expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(savedHash, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, _keyDerivationPrf, Iterations, subkeyLength);

            return CryptographicOperations.FixedTimeEquals(expectedSubkey, actualSubkey);
        }
        catch
        {
            return false;
        }
    }
}
