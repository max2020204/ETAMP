#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Represents a service implementing the Elliptic Curve Integrated Encryption Scheme (ECIES)
///     for secure data encryption and decryption. This class integrates elliptic curve cryptography
///     with symmetric encryption to provide a reliable encryption mechanism.
/// </summary>
public sealed class ECIESEncryptionService : IECIESEncryptionService
{
    private readonly IEncryptionService? _encryptionService;

    /// <summary>
    ///     Provides encryption and decryption functionality using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    ///     Inherits functionality from ECIESEncryptionServiceBase and utilizes key exchange and symmetric encryption services.
    /// </summary>
    public ECIESEncryptionService(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    ///     Encrypts the provided message stream using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    ///     Derives a shared secret based on the provided Diffie-Hellman keys and encrypts the message with the derived secret.
    /// </summary>
    /// <param name="message">The input stream containing the message to be encrypted.</param>
    /// <param name="privateKey">The private Diffie-Hellman key used to derive the shared secret.</param>
    /// <param name="publicKey">The public Diffie-Hellman key used to derive the shared secret.</param>
    /// <returns>
    ///     A task that represents the asynchronous encryption operation. The task result contains the encrypted message
    ///     stream.
    /// </returns>
    /// <example>
    ///     var encryptedStream = await encryptionService.EncryptAsync(inputStream, privateKey, publicKey);
    /// </example>
    public async Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.EncryptAsync(message, sharedSecret);
    }


    /// <summary>
    ///     Asynchronously encrypts the provided message using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    ///     Derives a shared secret from the provided private and public keys and performs encryption on the message stream.
    /// </summary>
    /// <param name="message">The message stream to be encrypted.</param>
    /// <param name="privateKey">The ECDiffieHellman private key used for deriving the shared secret.</param>
    /// <param name="publicKey">The public key used for deriving the shared secret.</param>
    /// <returns>A task representing the asynchronous encryption operation, containing the encrypted message stream.</returns>
    /// <example>
    ///     var encryptedStream = await encryptionService.EncryptAsync(inputStream, privateKey, publicKey);
    /// </example>
    public async Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.EncryptAsync(message, sharedSecret);
    }

    /// Asynchronously decrypts an encrypted message using the provided public key.
    /// <param name="encryptedMessageBase64">
    ///     A stream containing the Base64-encoded encrypted message to be decrypted. This cannot be null.
    /// </param>
    /// <param name="publicKey">
    ///     The ECDiffieHellmanPublicKey used to derive the shared secret for decryption. This cannot be null.
    /// </param>
    /// <returns>
    ///     A task that resolves to a stream containing the decrypted data.
    /// </returns>
    public async Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(encryptedMessageBase64, nameof(encryptedMessageBase64));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.DecryptAsync(encryptedMessageBase64, sharedSecret);
    }

    /// <summary>
    ///     Asynchronously decrypts an encrypted message using the ECIES encryption scheme.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message encoded in Base64 format as a stream. Cannot be null.</param>
    /// <param name="publicKey">The public key used to derive the shared secret for decryption.</param>
    /// <returns>A stream containing the decrypted message.</returns>
    public async Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(encryptedMessageBase64, nameof(encryptedMessageBase64));
        var sharedSecret = DeriveSharedSecret(privateKey, publicKey);
        return await _encryptionService!.DecryptAsync(encryptedMessageBase64, sharedSecret);
    }

    /// <summary>
    ///     Derives a shared secret key using the specified public key.
    /// </summary>
    /// <param name="publicKey">The ECDiffieHellman public key used for deriving the shared secret.</param>
    /// <returns>A byte array representing the derived shared secret.</returns>
    private byte[] DeriveSharedSecret(ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        return privateKey.DeriveKeyMaterial(publicKey);
    }

    /// <summary>
    ///     Derives a shared secret key using the provided public key.
    /// </summary>
    /// <param name="publicKey">The raw byte array of the public key to use for deriving the shared secret.</param>
    /// <returns>A byte array representing the derived shared secret key.</returns>
    private byte[] DeriveSharedSecret(ECDiffieHellman privateKey, byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        using var ecdh = ECDiffieHellman.Create();
        ecdh.ImportSubjectPublicKeyInfo(publicKey, out _);
        return DeriveSharedSecret(privateKey, ecdh.PublicKey);
    }
}