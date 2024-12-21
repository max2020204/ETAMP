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
}