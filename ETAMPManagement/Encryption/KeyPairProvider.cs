#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.Helper;
using ETAMPManagement.Models;

#endregion

namespace ETAMPManagement.Encryption;

/// <summary>
///     Provides key pair generation and management for ECDiffieHellman encryption.
/// </summary>
public sealed class KeyPairProvider : InitializeBase, IKeyPairProvider
{
    private ECDiffieHellman _eCDiffieHellman;

    /// <summary>
    ///     Represents a provider that manages asymmetric key pairs using the ECDiffieHellman algorithm.
    /// </summary>
    public KeyPairProvider()
    {
        _eCDiffieHellman = ECDiffieHellman.Create();
        InitializeKeys();
    }

    /// <summary>
    ///     Represents a provider for managing key models.
    /// </summary>
    public ECDKeyModelProvider KeyModelProvider { get; private set; } = null!;

    /// <summary>
    ///     Represents the public key of the Elliptic Curve Diffie-Hellman (ECDH) key pair.
    /// </summary>
    public ECDiffieHellmanPublicKey HellmanPublicKey => _eCDiffieHellman.PublicKey;

    /// <summary>
    ///     Initializes the provider with a specific <see cref="ECDiffieHellman" /> instance, allowing for custom configuration
    ///     and use of an existing ECDiffieHellman instance for cryptographic operations.
    /// </summary>
    /// <param name="eCDiffieHellman">An existing instance of ECDiffieHellman to be used by the provider.</param>
    public void Initialize(ECDiffieHellman eCDiffieHellman)
    {
        _eCDiffieHellman = eCDiffieHellman ?? throw new ArgumentNullException(nameof(eCDiffieHellman));
        InitializeKeys();
    }

    /// <summary>
    ///     Gets the <see cref="ECDiffieHellman" /> instance used for key exchange.
    /// </summary>
    /// <returns>The <see cref="ECDiffieHellman" /> instance.</returns>
    public ECDiffieHellman GetECDiffieHellman()
    {
        return _eCDiffieHellman;
    }

    /// <summary>
    ///     Imports a private key into the ECDH key pair provider.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array to import into the provider.</param>
    public void ImportPrivateKey(byte[] privateKey)
    {
        ArgumentNullException.ThrowIfNull(privateKey);
        _eCDiffieHellman.ImportPkcs8PrivateKey(privateKey, out _);
        InitializeKeys();
    }

    /// <summary>
    ///     Imports a public key into the ECDH key pair provider.
    /// </summary>
    /// <param name="publicKey">The public key as a byte array to import into the provider.</param>
    public void ImportPublicKey(byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey);

        _eCDiffieHellman.ImportSubjectPublicKeyInfo(publicKey, out _);
        KeyModelProvider = new ECDKeyModelProvider
        {
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
        };
    }

    /// <summary>
    ///     Releases all resources used by the KeyPairProvider.
    /// </summary>
    public void Dispose()
    {
        _eCDiffieHellman.Dispose();
    }

    /// <summary>
    ///     Initializes the key pair provider by generating the public and private keys using the ECDiffieHellman algorithm.
    /// </summary>
    private void InitializeKeys()
    {
        KeyModelProvider = new ECDKeyModelProvider
        {
            PrivateKey = _eCDiffieHellman.ExportPkcs8PrivateKeyPem(),
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
        };
    }
}