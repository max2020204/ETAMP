#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;

#endregion

namespace ETAMP.Encryption.Base;

/// <summary>
///     This is an abstract base class for ECIES (Elliptic Curve Integrated Encryption Scheme)
///     encryption service. It provides the basic functionality for encrypting and decrypting messages.
/// </summary>
public abstract class ECIESEncryptionServiceBase : IECIESEncryptionService
{
    public ECIESEncryptionServiceBase(IEncryptionService encryptionService)
    {
        EncryptionService = encryptionService ??
                            throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    ///     Provides encryption and decryption services.
    /// </summary>
    protected IEncryptionService? EncryptionService { get; private set; }


    /// <summary>
    ///     Encrypts the given stream using ECIES (Elliptic Curve Integrated Encryption Scheme) and the provided public key.
    /// </summary>
    /// <param name="message">The input stream to be encrypted.</param>
    /// <param name="publicKey">The public key used for encryption, provided as <see cref="ECDiffieHellmanPublicKey" />.</param>
    /// <returns>A stream containing the encrypted data.</returns>
    public abstract Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Encrypts the provided message asynchronously using the specified public key.
    /// </summary>
    /// <param name="message">The input data stream to be encrypted.</param>
    /// <param name="publicKey">The public key used for encryption.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the encrypted data as a stream.</returns>
    public abstract Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, byte[] publicKey);

    /// <summary>
    ///     Decrypts the given stream that contains encrypted data encoded in Base64 format using ECIES (Elliptic Curve
    ///     Integrated Encryption Scheme) and the provided public key.
    /// </summary>
    /// <param name="encryptedMessageBase64">The input stream containing the encrypted data in Base64 format to be decrypted.</param>
    /// <param name="publicKey">The public key used for decryption, provided as <see cref="ECDiffieHellmanPublicKey" />.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    public abstract Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Decrypts the given stream encrypted using ECIES (Elliptic Curve Integrated Encryption Scheme) with the provided
    ///     public key.
    /// </summary>
    /// <param name="encryptedMessageBase64">A stream containing the encrypted data in Base64 format.</param>
    /// <param name="publicKey">The public key used for decryption, provided as a byte array.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    public abstract Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        byte[] publicKey);
}