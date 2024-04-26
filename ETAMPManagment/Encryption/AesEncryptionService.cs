using System.Security.Cryptography;
using ETAMPManagment.Encryption.Interfaces;

namespace ETAMPManagment.Encryption;

/// <summary>
///     Provides AES encryption and decryption services.
/// </summary>
public class AesEncryptionService : IEncryptionService
{
    public byte[]? IV { get; private set; }

    /// <summary>
    ///     Encrypts the specified data using the AES algorithm.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <param name="key">The encryption key.</param>
    /// <returns>The encrypted data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when data or key is null.</exception>
    public virtual byte[] Encrypt(byte[] data, byte[] key, byte[]? iv)
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
    ///     Decrypts the specified data using the AES algorithm.
    /// </summary>
    /// <param name="data">The data to decrypt.</param>
    /// <param name="key">The decryption key.</param>
    /// <returns>The decrypted data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when data or key is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the IV is not set.</exception>
    public virtual byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(iv, nameof(iv));

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(data, 0, data.Length);
    }
}