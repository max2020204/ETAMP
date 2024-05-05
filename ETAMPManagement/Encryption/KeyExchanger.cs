#region

using System.Security.Cryptography;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.Helper;

#endregion

namespace ETAMPManagement.Encryption;

/// <summary>
///     The KeyExchanger class is responsible for exchanging and deriving key materials using the ECDiffieHellman
///     algorithm.
/// </summary>
public class KeyExchanger : InitializeBase, IKeyExchanger
{
    private IKeyPairProvider? _keyProvider;
    private byte[]? _sharedSecret;
    /// <summary>
    ///     Initializes the key exchanger with a key pair provider for subsequent cryptographic operations.
    /// </summary>
    /// <param name="keyPairProvider">The provider of ECDH key pairs used for deriving shared secrets.</param>
    public void Initialize(IKeyPairProvider keyPairProvider)
    {
        _init = true;
        _keyProvider = keyPairProvider ?? throw new ArgumentNullException(nameof(keyPairProvider));
        _sharedSecret = null;
    }

    /// <summary>
    ///     Derives a key using the hash-based method from the given public key and additional parameters.
    /// </summary>
    /// <param name="publicKey">The public key of the other party.</param>
    /// <param name="hash">The hash algorithm to use.</param>
    /// <param name="secretPrepend">Optional data to prepend before the derived secret.</param>
    /// <param name="secretAppend">Optional data to append after the derived secret.</param>
    /// <returns>A byte array representing the derived key.</returns>
    public byte[] DeriveKeyHash(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? secretPrepend,
        byte[]? secretAppend)
    {
        ArgumentNullException.ThrowIfNull(publicKey);
        ArgumentNullException.ThrowIfNull(_keyProvider);
        CheckInitialization();
        return _sharedSecret = _keyProvider!.GetECDiffieHellman()
            .DeriveKeyFromHash(publicKey, hash, secretPrepend, secretAppend);
    }

    /// <summary>
    ///     Derives a key using the specified public key.
    /// </summary>
    /// <param name="publicKey">The public key used for key derivation.</param>
    /// <returns>The derived key.</returns>
    public byte[] DeriveKey(ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey);
        ArgumentNullException.ThrowIfNull(_keyProvider);

        return _sharedSecret = _keyProvider!.GetECDiffieHellman().DeriveKeyMaterial(publicKey);
    }

    /// <summary>
    ///     Derives a key material from the raw byte array of the other party's public key.
    /// </summary>
    /// <param name="otherPartyPublicKey">The raw byte array representing the other party's public key.</param>
    /// <returns>A byte array representing the derived key material.</returns>
    public byte[] DeriveKey(byte[] otherPartyPublicKey)
    {
        ArgumentNullException.ThrowIfNull(otherPartyPublicKey);
        ArgumentNullException.ThrowIfNull(_keyProvider);
        CheckInitialization();
        using var eCDiffieHellman = ECDiffieHellman.Create();
        eCDiffieHellman.ImportSubjectPublicKeyInfo(otherPartyPublicKey, out _);

        return _sharedSecret = _keyProvider!.GetECDiffieHellman().DeriveKeyMaterial(eCDiffieHellman.PublicKey);
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
    public byte[] DeriveKeyHmac(ECDiffieHellmanPublicKey publicKey, HashAlgorithmName hash, byte[]? hmacKey,
        byte[]? secretPrepend, byte[]? secretAppend)
    {
        ArgumentNullException.ThrowIfNull(publicKey);
        ArgumentNullException.ThrowIfNull(_keyProvider);
        CheckInitialization();
        return _sharedSecret = _keyProvider!.GetECDiffieHellman()
            .DeriveKeyFromHmac(publicKey, hash, hmacKey, secretPrepend, secretAppend);
    }

    /// <summary>
    ///     Retrieves the shared secret for key exchange.
    /// </summary>
    /// <returns>The shared secret as a byte array. Returns null if the shared secret is not available.</returns>
    public byte[]? GetSharedSecret()
    {
        CheckInitialization();
        return _sharedSecret;
    }
}