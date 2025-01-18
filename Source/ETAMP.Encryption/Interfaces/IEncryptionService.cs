namespace ETAMP.Encryption.Interfaces;

/// <summary>
/// Defines an interface for encryption and decryption services.
/// Provides methods for transforming data streams securely using a given cryptographic key.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Encrypts the provided data stream asynchronously using AES encryption.
    /// </summary>
    /// <param name="inputStream">The input stream containing the data to be encrypted.</param>
    /// <param name="key">The encryption key used for encrypting the data.</param>
    /// <returns>A stream containing the encrypted data.</returns>
    Task<Stream> EncryptAsync(Stream inputStream, byte[] key);

    /// <summary>
    /// Decrypts the provided encrypted data stream asynchronously using AES encryption.
    /// </summary>
    /// <param name="inputStream">The input stream containing the encrypted data to be decrypted.</param>
    /// <param name="key">The decryption key used to decrypt the data.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    Task<Stream> DecryptAsync(Stream inputStream, byte[] key);
}