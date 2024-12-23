using System.Security.Cryptography;
using System.Text;

namespace SimpleBlog.Application.Users;

public static class PasswordManager
{
    private const int KEY_SIZE = 64;
    private const int HASH_ITERATIONS = 350000;
    private static HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    internal static (string hash, string salt) GeneratePaswordHashAndSalt(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(KEY_SIZE);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                                salt,
                                                HASH_ITERATIONS,
                                                _hashAlgorithm,
                                                KEY_SIZE);

        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    internal static bool VerifyPasword(string password, string passwordHash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        ReadOnlySpan<byte> oldHash = Convert.FromBase64String(passwordHash);
        ReadOnlySpan<byte> newHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                                saltBytes,
                                                HASH_ITERATIONS,
                                                _hashAlgorithm,
                                                KEY_SIZE);

        return CryptographicOperations.FixedTimeEquals(oldHash, newHash);
    }
}
