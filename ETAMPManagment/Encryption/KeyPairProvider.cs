using System.Security.Cryptography;
using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;

namespace ETAMPManagment.Encryption;

/// <summary>
///     Provides an implementation for managing Elliptic Curve Diffie-Hellman (ECDH) key pairs and facilitating
///     cryptographic operations.
/// </summary>
public sealed class KeyPairProvider : IKeyPairProvider
{
    private ECDiffieHellman _eCDiffieHellman;

    /// <summary>
    ///     Initializes a new instance of the KeyPairProvider class, creating a new ECDiffieHellman key pair.
    /// </summary>
    public KeyPairProvider()
    {
        _eCDiffieHellman = ECDiffieHellman.Create();
        InitializeKeys();
    }

    /// <summary>
    ///     Provides access to the model provider which contains the public and private keys.
    /// </summary>
    public ECDKeyModelProvider KeyModelProvider { get; private set; }

    /// <summary>
    ///     Gets the public key component of the ECDH key pair, allowing for public key exchange operations.
    /// </summary>
    public ECDiffieHellmanPublicKey HellmanPublicKey => _eCDiffieHellman.PublicKey;

    /// <summary>
    ///     Initializes the provider with a specific ECDiffieHellman instance, allowing for custom configuration
    ///     and use of an existing ECDiffieHellman instance for cryptographic operations.
    /// </summary>
    /// <param name="eCDiffieHellman">An existing instance of ECDiffieHellman for cryptographic operations.</param>
    public void Initialize(ECDiffieHellman eCDiffieHellman)
    {
        _eCDiffieHellman = eCDiffieHellman ?? throw new ArgumentNullException(nameof(eCDiffieHellman));
        InitializeKeys();
    }

    /// <summary>
    ///     Provides access to the underlying ECDiffieHellman instance for advanced cryptographic operations.
    /// </summary>
    /// <returns>The ECDiffieHellman instance used by this provider.</returns>
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

    private void InitializeKeys()
    {
        KeyModelProvider = new ECDKeyModelProvider
        {
            PrivateKey = _eCDiffieHellman.ExportPkcs8PrivateKeyPem(),
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
        };
    }
}