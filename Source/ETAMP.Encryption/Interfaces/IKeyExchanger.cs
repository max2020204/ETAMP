#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Provides functionality for key exchange and derivation using Elliptic Curve Diffie-Hellman (ECDH) algorithm.
/// </summary>
public interface IKeyExchanger
{
    /// <summary>
    ///     Derives a key using the hash-based method from the given public key and additional parameters.
    /// </summary>
    /// <param name="publicKey">The public key of the other party.</param>
    /// <param name="hash">The hash algorithm to use.</param>
    /// <param name="secretPrepend">Optional data to prepend before the derived secret.</param>
    /// <param name="secretAppend">Optional data to append after the derived secret.</param>
    /// <returns>A byte array representing the derived key.</returns>
    byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? secretPrepend,
        byte[]? secretAppend);

    /// <summary>
    ///     Derives a key using the HMAC-based method from the given public key and additional parameters.
    /// </summary>
    /// <param name="publicKey">The public key of the other party.</param>
    /// <param name="hash">The hash algorithm to use for HMAC.</param>
    /// <param name="hmacKey">The key to use for HMAC.</param>
    /// <param name="secretPrepend">Optional data to prepend before the derived secret.</param>
    /// <param name="secretAppend">Optional data to append after the derived secret.</param>
    /// <returns>A byte array representing the derived key.</returns>
    byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey,
        byte[]? secretPrepend, byte[]? secretAppend);

    /// <summary>
    ///     Derives a key material directly from the given public key without additional hashing or HMAC.
    /// </summary>
    /// <param name="publicKey">The public key of the other party.</param>
    /// <returns>A byte array representing the derived key material.</returns>
    byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Derives a key material from the raw byte array of the other party's public key.
    /// </summary>
    /// <param name="otherPartyPublicKey">The raw byte array representing the other party's public key.</param>
    /// <returns>A byte array representing the derived key material.</returns>
    byte[] DeriveKey(byte[] otherPartyPublicKey);
}