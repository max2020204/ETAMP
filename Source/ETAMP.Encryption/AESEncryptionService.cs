using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;

namespace ETAMP.Encryption;

/// <summary>
///     Provides AES encryption and decryption services.
/// </summary>
public class AESEncryptionService : IEncryptionService
{
    public byte[]? IV { get; private set; }

    /// <summary>
    ///     Encrypts the provided data using AES encryption algorithm.
    /// </summary>
    /// <param name="data">The data to be encrypted.</param>
    /// <param name="key">The encryption key.</param>
    /// <param name="iv">The initialization vector.</param>
    /// <returns>The encrypted data.</returns>
    public byte[] Encrypt(byte[] data, byte[] key, byte[]? iv)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv ?? aes.IV;
        IV = aes.IV;
        using var encryptor = aes.CreateEncryptor();
        return encryptor.TransformFinalBlock(data, 0, data.Length);
    }

    /// <summary>
    ///     Decrypts the provided data using the given key and IV.
    /// </summary>
    /// <param name="data">The data to be decrypted.</param>
    /// <param name="key">The key used for decryption.</param>
    /// <param name="iv">The initialization vector used for decryption.</param>
    /// <returns>The decrypted data.</returns>
    public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(iv, nameof(iv));

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decrypt = aes.CreateDecryptor();
        return decrypt.TransformFinalBlock(data, 0, data.Length);
    }
}