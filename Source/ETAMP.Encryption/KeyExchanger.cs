#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     The KeyExchanger class is responsible for exchanging and deriving key materials using the ECDiffieHellmanBase
///     algorithm.
/// </summary>
public class KeyExchanger : KeyExchangerBase
{
    /// <summary>
    ///     Derives a key using the hash-based method from the given public key and additional parameters.
    /// </summary>
    /// <param name="publicKey">The public key of the other party.</param>
    /// <param name="hash">The hash algorithm to use.</param>
    /// <param name="secretPrepend">Optional data to prepend before the derived secret.</param>
    /// <param name="secretAppend">Optional data to append after the derived secret.</param>
    /// <returns>A byte array representing the derived key.</returns>
    public override byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash,
        byte[]? secretPrepend,
        byte[]? secretAppend)
    {
        ArgumentNullException.ThrowIfNull(publicKey);
        ArgumentNullException.ThrowIfNull(KeyProvider);
        CheckInitialization();
        return SharedSecret = KeyProvider!.GetECDiffieHellman()
            .DeriveKeyFromHash(publicKey, hash, secretPrepend, secretAppend);
    }

    /// <summary>
    ///     Derives a key using the specified public key.
    /// </summary>
    /// <param name="publicKey">The public key used for key derivation.</param>
    /// <returns>The derived key.</returns>
    public override byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey);
        ArgumentNullException.ThrowIfNull(KeyProvider);

        return SharedSecret = KeyProvider!.GetECDiffieHellman().DeriveKeyMaterial(publicKey);
    }

    /// <summary>
    ///     Derives a key material from the raw byte array of the other party's public key.
    /// </summary>
    /// <param name="otherPartyPublicKey">The raw byte array representing the other party's public key.</param>
    /// <returns>A byte array representing the derived key material.</returns>
    public override byte[] DeriveKey(byte[] otherPartyPublicKey)
    {
        ArgumentNullException.ThrowIfNull(otherPartyPublicKey);
        ArgumentNullException.ThrowIfNull(KeyProvider);
        CheckInitialization();
        using var eCDiffieHellman = ECDiffieHellman.Create();
        eCDiffieHellman.ImportSubjectPublicKeyInfo(otherPartyPublicKey, out _);

        return SharedSecret = KeyProvider!.GetECDiffieHellman().DeriveKeyMaterial(eCDiffieHellman.PublicKey);
    }

    /// <summary>
    ///     Derives a key using HMAC from the specified public key, hash algorithm, HMAC key, secret prepend, and secret
    ///     append.
    /// </summary>
    /// <param name="publicKey">The public key used for key derivation.</param>
    /// <param name="hash">The hash algorithm used for key derivation.</param>
    /// <param name="hmacKey">The HMAC key used for key derivation. Can be null.</param>
    /// <param name="secretPrepend">Data to prepend to the shared secret. Can be null.</param>
    /// <param name="secretAppend">Data to append to the shared secret. Can be null.</param>
    /// <returns>The derived shared secret key.</returns>
    public override byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey,
        byte[]? secretPrepend, byte[]? secretAppend)
    {
        ArgumentNullException.ThrowIfNull(publicKey);
        ArgumentNullException.ThrowIfNull(KeyProvider);
        CheckInitialization();
        return SharedSecret = KeyProvider!.GetECDiffieHellman()
            .DeriveKeyFromHmac(publicKey, hash, hmacKey, secretPrepend, secretAppend);
    }
}