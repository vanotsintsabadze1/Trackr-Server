using System.Security.Cryptography;
using Trackr.Application.Interfaces;

namespace Trackr.Infrastructure.Utility;

internal class PasswordHasher : IPasswordHasher
{
    private const int saltSize = 16;
    private const int hashSize = 32;
    private const int iterations = 100000;
    private readonly HashAlgorithmName alg = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, alg, hashSize);
        string hashToString = Convert.ToHexString(hash);
        string saltToString = Convert.ToHexString(salt);

        return $"{hashToString}-{saltToString}";
    }

    public bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split("-");
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, alg, hashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
