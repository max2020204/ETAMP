#region

using System.Security.Cryptography;

#endregion

namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines a service for encrypting and decrypting messages using Elliptic Curve Integrated ETAMPEncryption Scheme
///     (ECIES).
/// </summary>
public interface IECIESEncryptionService
{
    /// <summary>
    ///     Encrypts the specified message using ECIES encryption algorithm.
    /// </summary>
    /// <param name="message">The plain text message to be encrypted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the encrypted message as a
    ///     Base64-encoded string.
    /// </returns>
    Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Encrypts the specified message using ECIES encryption algorithm.
    /// </summary>
    /// <param name="message">The input stream containing the plain text message to be encrypted.</param>
    /// <param name="publicKey">The public key used for encryption.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the encrypted message as a stream.
    /// </returns>
    Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, byte[] publicKey);

    /// <summary>
    ///     Asynchronously decrypts a message that was encrypted using ECIES encryption.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message in Base64-encoded format. Can be null.</param>
    /// <returns>
    ///     A task that represents the asynchronous decryption operation. The task result contains the decrypted plain text.
    /// </returns>
    Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey);

    /// <summary>
    ///     Decrypts an encrypted message that was encrypted using the ECIES encryption algorithm.
    /// </summary>
    /// <param name="encryptedMessageBase64">The input stream containing the encrypted message in Base64-encoded format.</param>
    /// <param name="publicKey">The public key used for decryption.</param>
    /// <returns>
    ///     A task that represents the asynchronous decryption operation. The task result contains the decrypted message as a
    ///     stream.
    /// </returns>
    Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey, byte[] publicKey);
}