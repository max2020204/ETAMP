#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Provides AES encryption and decryption services.
/// </summary>
public class AESEncryptionService : IEncryptionService
{
    /// <summary>
    /// Encrypts the provided data asynchronously using AES encryption algorithm and streams.
    /// </summary>
    /// <param name="inputStream">The input stream with data to be encrypted.</param>
    /// <param name="key">The encryption key.</param>
    /// <param name="iv">The initialization vector. If null, a new one will be generated.</param>
    /// <returns>A stream containing the encrypted data.</returns>
    public async Task<Stream> EncryptAsync(Stream inputStream, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(inputStream, nameof(inputStream));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        var outputStream = new MemoryStream(); // Output will be written here

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        await outputStream.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length));

        await using var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

        await inputStream.CopyToAsync(cryptoStream);
        await cryptoStream.FlushAsync();

        outputStream.Position = 0;
        return outputStream;
    }

    /// <summary>
    /// Decrypts the provided encrypted data asynchronously using AES encryption algorithm and streams.
    /// </summary>
    /// <param name="inputStream">The input stream with encrypted data.</param>
    /// <param name="key">The decryption key.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    public async Task<Stream> DecryptAsync(Stream inputStream, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(inputStream, nameof(inputStream));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        var outputStream = new MemoryStream(); // Decrypted data will be written here

        using var aes = Aes.Create();
        var iv = new byte[aes.BlockSize / 8];

        // Read the IV from the input stream
        var bytesRead = await inputStream.ReadAsync(iv);
        if (bytesRead != iv.Length)
            throw new CryptographicException("Failed to read the initialization vector (IV) from the input stream.");

        aes.Key = key;
        aes.IV = iv;

        // Create the CryptoStream for decryption
        await using var cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);

        // Copy decrypted data from CryptoStream to the output stream
        await cryptoStream.CopyToAsync(outputStream);

        // Reset the output stream position for further reading
        outputStream.Position = 0;
        return outputStream;
    }
}