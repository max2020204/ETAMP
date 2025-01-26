#region

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

namespace ETAMP.Core.Models;

/// <summary>
///     Provides storage for public and private keys in a model, typically used in cryptographic operations.
/// </summary>
public class ECDKeyModelProvider
{
    private static ILogger<ECDKeyModelProvider> _logger;

    public ECDKeyModelProvider(ILogger<ECDKeyModelProvider>? logger = null)
    {
        _logger = logger ?? NullLogger<ECDKeyModelProvider>.Instance;
    }

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
        _logger.LogDebug("Clearing PEM formatting.");
        if (!string.IsNullOrWhiteSpace(key))
            return key.Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

        _logger.LogError("Key cannot be null or empty.");
        throw new ArgumentException("Key cannot be null or empty.", nameof(key));
    }
}