namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines a contract for managing Elliptic Curve Diffie-Hellman (ECDH) key pairs and facilitates access to their
///     components,
///     including the public and private keys. This interface abstracts the functionality necessary to work with ECDH keys
///     in various cryptographic operations.
/// </summary>
public interface IKeyPairProvider : IDisposable
{
    /// <summary>
    ///     Imports a private key into the ECDH key pair provider.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array to import into the provider.</param>
    void ImportPrivateKey(byte[] privateKey);

    /// <summary>
    ///     Imports a public key into the ECDH key pair provider.
    /// </summary>
    /// <param name="publicKey">The public key as a byte array to import into the provider.</param>
    void ImportPublicKey(byte[] publicKey);
}