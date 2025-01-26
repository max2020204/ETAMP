#region

#endregion

namespace ETAMP.Core.Models;

/// <summary>
///     Provides storage for public and private keys in a model, typically used in cryptographic operations.
/// </summary>
public class ECDKeyModelProvider
{
    /// <summary>
    ///     Gets or sets the public key in a format suitable for the cryptographic operation.
    /// </summary>
    public string? PublicKey { get; set; }

    /// <summary>
    ///     Gets or sets the private key in a format suitable for the cryptographic operation.
    /// </summary>
    public string? PrivateKey { get; set; }

    public static string ClearPemFormatting(string key)
    {
        if (!string.IsNullOrWhiteSpace(key))
            return key.Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

        throw new ArgumentException("Key cannot be null or empty.", nameof(key));
    }
}