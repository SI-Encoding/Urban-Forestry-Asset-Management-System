using System.Security.Cryptography;
using System.Text;

namespace UFAMS.Infrastructure.ArcGIS;

public static class PkceGenerator
{
    public static string GenerateCodeVerifier()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Base64UrlEncode(bytes);
    }

    public static string GenerateCodeChallenge(string verifier)
    {
        using var sha = SHA256.Create();

        var hash = sha.ComputeHash(
            Encoding.UTF8.GetBytes(verifier));

        return Base64UrlEncode(hash);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}