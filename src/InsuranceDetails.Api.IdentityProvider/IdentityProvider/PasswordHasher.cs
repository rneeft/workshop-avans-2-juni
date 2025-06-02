using System.Text;
using Konscious.Security.Cryptography;

namespace InsuranceDetails.Api.IdentityProvider;

public static class PasswordHasher
{
    public static string HashPassword(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
        argon2.Salt = salt;
        argon2.DegreeOfParallelism = 8;
        argon2.MemorySize = 65536;
        argon2.Iterations = 4;

        var hash = argon2.GetBytes(32); // Generate a 32-byte hash
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
    {
        var hashToVerify = HashPassword(password, salt);
        return hashToVerify == hashedPassword;
    }
}