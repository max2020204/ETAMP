namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines a service for encrypting and decrypting messages using Elliptic Curve Integrated ETAMPEncryption Scheme
///     (ECIES).
/// </summary>
public interface IECIESEncryptionService
{
    /// <summary>
    /// Encrypts the specified message using ECIES encryption algorithm.
    /// </summary>
    /// <param name="message">The plain text message to be encrypted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encrypted message as a Base64-encoded string.</returns>
    Task<string> EncryptAsync(MemoryStream message);

    /// <summary>
    /// Asynchronously decrypts a message that was encrypted using ECIES encryption.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message in Base64-encoded format. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous decryption operation. The task result contains the decrypted plain text.
    /// </returns>
    Task<string> DecryptAsync(MemoryStream? encryptedMessageBase64);
}