#region

using ETAMP.Core.Models;
using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Provides key pair generation and management for ECDiffieHellmanBase encryption.
/// </summary>
public sealed class KeyPairProvider : KeyPairProviderBase
{
    /// <summary>
    ///     Imports a private key into the ECDH key pair provider.
    /// </summary>
    /// <param name="privateKey">The private key as a byte array to import into the provider.</param>
    public override void ImportPrivateKey(byte[] privateKey)
    {
        ArgumentNullException.ThrowIfNull(privateKey);
        ECDiffieHellmanBase?.ImportPkcs8PrivateKey(privateKey, out _);
        InitializeKeys();
    }

    /// <summary>
    ///     Imports a public key into the ECDH key pair provider.
    /// </summary>
    /// <param name="publicKey">The public key as a byte array to import into the provider.</param>
    public override void ImportPublicKey(byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey);

        ECDiffieHellmanBase?.ImportSubjectPublicKeyInfo(publicKey, out _);
        KeyModelProvider = new ECDKeyModelProvider
        {
            PublicKey = ECDiffieHellmanBase?.ExportSubjectPublicKeyInfoPem()
        };
    }

    /// <summary>
    ///     Releases all resources used by the KeyPairProvider.
    /// </summary>
    public override void Dispose()
    {
        ECDiffieHellmanBase?.Dispose();
    }
}