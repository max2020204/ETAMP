#region

using System.Security.Cryptography;
using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Helper;
using ETAMPManagment.Models;

#endregion

namespace ETAMPManagment.Encryption;

/// <summary>
///     Provides key pair generation and management for ECDiffieHellman encryption.
/// </summary>
public sealed class KeyPairProvider : InitializeBase, IKeyPairProvider
{
    /// <summary>
    ///     The variable _eCDiffieHellman is an instance of the ECDiffieHellman class used for managing Elliptic Curve
    ///     Diffie-Hellman (ECDH) key pairs and facilitating access to their components, including the public and private keys.
    ///     This variable is part of the KeyPairProvider class that implements the IKeyPairProvider interface.
    /// </summary>
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
    public ECDKeyModelProvider? KeyModelProvider { get; private set; }

    /// <summary>
    ///     Represents the public key of the Elliptic Curve Diffie-Hellman (ECDH) key pair.
    /// </summary>
    /// <remarks>
    ///     This property provides access to the public key component of an ECDH key pair.
    ///     The public key is used for key exchange with another party in a cryptographic operation.
    /// </remarks>
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
    ///     Initializes the key pair provider by generating new key pair or importing existing keys.
    /// </summary>
    /// <remarks>
    ///     This method initializes the KeyPairProvider by generating a new key pair using the ECDiffieHellman algorithm
    ///     if no existing ECDiffieHellman instance is provided. If an existing ECDiffieHellman instance is provided,
    ///     it initializes the KeyPairProvider by importing the provided ECDiffieHellman instance.
    /// </remarks>
    /// <param name="eCDiffieHellman">
    ///     The existing ECDiffieHellman instance to be imported. If null, a new key pair will be
    ///     generated.
    /// </param>
    private void InitializeKeys()
    {
        KeyModelProvider = new ECDKeyModelProvider
        {
            PrivateKey = _eCDiffieHellman.ExportPkcs8PrivateKeyPem(),
            PublicKey = _eCDiffieHellman.ExportSubjectPublicKeyInfoPem()
        };
    }
}