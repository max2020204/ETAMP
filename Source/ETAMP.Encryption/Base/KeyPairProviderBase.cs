#region

using System.Security.Cryptography;
using ETAMP.Core.Models;
using ETAMP.Encryption.Helper;
using ETAMP.Encryption.Interfaces;

#endregion

namespace ETAMP.Encryption.Base;

public abstract class KeyPairProviderBase : CheckInitialize, IKeyPairProvider
{
    protected ECDiffieHellman? ECDiffieHellmanBase { get; private set; }

    /// <summary>
    ///     Represents a provider for managing key models.
    /// </summary>
    public ECDKeyModelProvider? KeyModelProvider { get; protected set; }

    /// <summary>
    ///     Represents the public key of the Elliptic Curve Diffie-Hellman (ECDH) key pair.
    /// </summary>
    public ECDiffieHellmanPublicKey? HellmanPublicKey => ECDiffieHellmanBase?.PublicKey;

    /// <summary>
    ///     Releases all resources used by the KeyPairProvider.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    ///     Imports a private key into the ECDH key pair provider.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array to import into the provider.</param>
    public abstract void ImportPrivateKey(byte[] privateKey);

    /// <summary>
    ///     Imports a public key into the ECDH key pair provider.
    /// </summary>
    /// <param name="publicKey">The public key as a byte array to import into the provider.</param>
    public abstract void ImportPublicKey(byte[] publicKey);

    /// <summary>
    ///     Initializes the provider with a specific <see cref="System.Security.Cryptography.ECDiffieHellman" /> instance,
    ///     allowing for custom configuration
    ///     and use of an existing ECDiffieHellmanBase instance for cryptographic operations.
    /// </summary>
    /// <param name="eCDiffieHellman">An existing instance of ECDiffieHellmanBase to be used by the provider.</param>
    public void Initialize(ECDiffieHellman eCDiffieHellman)
    {
        ECDiffieHellmanBase = eCDiffieHellman ?? throw new ArgumentNullException(nameof(eCDiffieHellman));
        InitializeKeys();
    }

    /// <summary>
    ///     Retrieves the instance of <see cref="System.Security.Cryptography.ECDiffieHellman" /> used by the
    ///     <see cref="KeyPairProviderBase" /> provider for cryptographic operations.
    /// </summary>
    /// <returns>The instance of <see cref="System.Security.Cryptography.ECDiffieHellman" /> used by the provider.</returns>
    public virtual ECDiffieHellman? GetECDiffieHellman()
    {
        return ECDiffieHellmanBase;
    }

    /// <summary>
    ///     Initializes the key pair provider by generating the public and private keys using the ECDiffieHellmanBase
    ///     algorithm.
    /// </summary>
    protected void InitializeKeys()
    {
        if (ECDiffieHellmanBase != null)
            KeyModelProvider = new ECDKeyModelProvider
            {
                PrivateKey = ECDiffieHellmanBase.ExportPkcs8PrivateKeyPem(),
                PublicKey = ECDiffieHellmanBase.ExportSubjectPublicKeyInfoPem()
            };
    }
}