namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Defines an interface for encryption and decryption services.
///     Provides methods for transforming data streams securely using a given cryptographic key.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Encrypts the provided data stream asynchronously using a specific encryption mechanism.
    /// </summary>
    /// <param name="inputStream">The input stream containing the data to be encrypted.</param>
    /// <param name="key">The encryption key used for encrypting the data.</param>
    /// <param name="cancellationToken">A token for propagating cancellation signals during the asynchronous operation.</param>
    /// <returns>A stream containing the encrypted data.</returns>
    Task<Stream> EncryptAsync(Stream inputStream, byte[] key, CancellationToken cancellationToken);

    /// <summary>
    /// Decrypts the provided encrypted data stream asynchronously using a specific decryption mechanism.
    /// </summary>
    /// <param name="inputStream">The input stream containing the encrypted data to be decrypted.</param>
    /// <param name="key">The decryption key used for decrypting the data.</param>
    /// <param name="cancellationToken">A token for propagating cancellation signals during the asynchronous operation.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    Task<Stream> DecryptAsync(Stream inputStream, byte[] key, CancellationToken cancellationToken);
}