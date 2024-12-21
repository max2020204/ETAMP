namespace ETAMP.Encryption.Interfaces;

/// <summary>
///     Provides mechanisms for encrypting and decrypting data.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    ///     Gets the initialization vector (IV) used for the last encryption operation.
    /// </summary>
    byte[]? IV { get; }

    /// <summary>
    ///     Encrypts the specified data using the provided key and an optional initialization vector (IV).
    ///     If the IV is not provided, a new one will be generated and used.
    /// </summary>
    /// <param name="data">The data to encrypt, represented as a byte array.</param>
    /// <param name="key">The encryption key, represented as a byte array.</param>
    /// <param name="iv">The initialization vector (IV) for encryption, or null to generate a new IV.</param>
    /// <returns>The encrypted data as a byte array.</returns>
    byte[] Encrypt(byte[] data, byte[] key, byte[]? iv);

    /// <summary>
    ///     Decrypts the specified data using the provided key and the initialization vector (IV).
    /// </summary>
    /// <param name="data">The data to decrypt, represented as a byte array.</param>
    /// <param name="key">The decryption key, represented as a byte array.</param>
    /// <param name="iv">The initialization vector (IV) used for decryption.</param>
    /// <returns>The decrypted data as a byte array.</returns>
    byte[] Decrypt(byte[] data, byte[] key, byte[] iv);
}