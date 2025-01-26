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
    /// Encrypts a given message stream using the specified private and public keys.
    /// </summary>
    /// <param name="message">The stream containing the message to be encrypted.</param>
    /// <param name="privateKey">The private ECDiffieHellman key used for encryption.</param>
    /// <param name="publicKey">The public key used for encryption, represented either as an ECDiffieHellmanPublicKey or a byte array.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous encryption operation, with a stream containing the encrypted message as its result.</returns>
    Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, ECDiffieHellmanPublicKey publicKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Encrypts a message using the ECIES (Elliptic Curve Integrated Encryption Scheme).
    /// </summary>
    /// <param name="message">The input stream containing the message to be encrypted.</param>
    /// <param name="privateKey">The sender's private ECDiffieHellman key used in the encryption process.</param>
    /// <param name="publicKey">The receiver's public key used in the encryption process.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A stream containing the encrypted message data.</returns>
    Task<Stream> EncryptAsync(Stream message, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrypts an encrypted message using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    /// <param name="encryptedMessageBase64">
    /// A stream containing the base64-encoded encrypted message to be decrypted.
    /// </param>
    /// <param name="privateKey">
    /// The ECDiffieHellman private key used for decryption.
    /// </param>
    /// <param name="publicKey">
    /// The ECDiffieHellmanPublicKey corresponding to the sender's public key.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token to cancel the operation, if necessary. Optional.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous decryption operation. The task result contains a stream with the decrypted message.
    /// </returns>
    Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey,
        ECDiffieHellmanPublicKey publicKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrypts an encrypted message represented as a base64 stream using the provided private key and a public key.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a base64-encoded stream to decrypt.</param>
    /// <param name="privateKey">The ECDiffieHellman private key used for the decryption process.</param>
    /// <param name="publicKey">The public key in byte array format associated with the encryption process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous decryption operation. The task result is a stream containing the decrypted message.
    /// </returns>
    Task<Stream> DecryptAsync(Stream encryptedMessageBase64, ECDiffieHellman privateKey, byte[] publicKey,
        CancellationToken cancellationToken = default);
}