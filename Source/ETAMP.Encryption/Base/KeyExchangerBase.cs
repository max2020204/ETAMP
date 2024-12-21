using System.Security.Cryptography;
using ETAMP.Encryption.Helper;
using ETAMP.Encryption.Interfaces;

namespace ETAMP.Encryption.Base;

/// <summary>
///     This is the base class for all key exchangers.
/// </summary>
public abstract class KeyExchangerBase : CheckInitialize, IKeyExchanger
{
    /// <summary>
    ///     Provides the functionality to exchange cryptographic keys using the Diffie-Hellman key exchange algorithm.
    /// </summary>
    protected KeyPairProviderBase? KeyProvider { get; private set; }

    /// <summary>
    ///     Represents a base class for key exchange operations.
    /// </summary>
    protected byte[]? SharedSecret { get; set; }

    /// <summary>
    ///     Derives a key using the hash-based method from the given public key and additional parameters.
    /// </summary>
    /// <param name="publicKey">The public key of the other party.</param>
    /// <param name="hash">The hash algorithm to use.</param>
    /// <param name="secretPrepend">Optional data to prepend before the derived secret.</param>
    /// <param name="secretAppend">Optional data to append after the derived secret.</param>
    /// <returns>A byte array representing the derived key.</returns>
    public abstract byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash,
        byte[]? secretPrepend,
        byte[]? secretAppend);

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
    public abstract byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey,
        byte[]? secretPrepend,
        byte[]? secretAppend);

    /// <summary>
    ///     Derives a key using the specified public key.
    /// </summary>
    /// <param name="publicKey">The public key used for key derivation.</param>
    /// <returns>The derived key.</returns>
    public abstract byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Derives a key material from the raw byte array of the other party's public key.
    /// </summary>
    /// <param name="otherPartyPublicKey">The raw byte array representing the other party's public key.</param>
    /// <returns>A byte array representing the derived key material.</returns>
    public abstract byte[] DeriveKey(byte[] otherPartyPublicKey);

    /// <summary>
    ///     Initializes the key exchanger with a key pair provider for subsequent cryptographic operations.
    /// </summary>
    /// <param name="keyPairProvider">The provider of ECDH key pairs used for deriving shared secrets.</param>
    public void Initialize(KeyPairProviderBase keyPairProvider)
    {
        Init = true;
        KeyProvider = keyPairProvider ?? throw new ArgumentNullException(nameof(keyPairProvider));
        SharedSecret = null;
    }

    /// <summary>
    ///     Retrieves the shared secret for key exchange.
    /// </summary>
    /// <returns>The shared secret as a byte array. Returns null if the shared secret is not available.</returns>
    public virtual byte[]? GetSharedSecret()
    {
        CheckInitialization();
        return SharedSecret;
    }
}