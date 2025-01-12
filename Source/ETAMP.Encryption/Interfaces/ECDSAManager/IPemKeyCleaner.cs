#region

using ETAMP.Core.Models;

#endregion

namespace ETAMP.Encryption.Interfaces.ECDSAManager;

/// <summary>
///     Defines methods for cleaning PEM-formatted keys to remove headers, footers, and whitespace,
///     making them suitable for cryptographic operations.
/// </summary>
public interface IPemKeyCleaner
{
    /// <summary>
    ///     Gets the key model provider that contains the cleaned private and public keys.
    /// </summary>
    ECDKeyModelProvider KeyModelProvider { get; }

    /// <summary>
    ///     Removes PEM formatting from a private key, converting it into a format suitable for cryptographic operations.
    /// </summary>
    /// <param name="privateKey">The private key in PEM format to be cleaned.</param>
    /// <returns>An instance of <see cref="IPemKeyCleaner" /> to allow method chaining.</returns>
    IPemKeyCleaner ClearPemPrivateKey(string privateKey);

    /// <summary>
    ///     Removes PEM formatting from a public key, converting it into a format suitable for cryptographic operations.
    /// </summary>
    /// <param name="publicKey">The public key in PEM format to be cleaned.</param>
    /// <returns>An instance of <see cref="IPemKeyCleaner" /> to allow method chaining.</returns>
    IPemKeyCleaner ClearPemPublicKey(string publicKey);
}