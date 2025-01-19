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
    public async Task<Stream> EncryptAsync(Stream inputStream, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(inputStream, nameof(inputStream));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("Invalid key length. Key must be 128, 192, or 256 bits long.", nameof(key));

        if (inputStream.CanSeek)
            inputStream.Position = 0;

        var outputStream = new MemoryStream();

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        await outputStream.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length));

        await using var cryptoStream =
            new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write, true);

        await inputStream.CopyToAsync(cryptoStream);
        if (cryptoStream.HasFlushedFinalBlock == false)
            await cryptoStream.FlushFinalBlockAsync();


        outputStream.Position = 0;
        return outputStream;
    }

    /// <summary>
    ///     Decrypts the provided encrypted data asynchronously using AES encryption algorithm and streams.
    /// </summary>
    /// <param name="inputStream">The input stream with encrypted data.</param>
    /// <param name="key">The decryption key.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    public async Task<Stream> DecryptAsync(Stream inputStream, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(inputStream, nameof(inputStream));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("Invalid key length. Key must be 128, 192, or 256 bits long.", nameof(key));

        if (inputStream.CanSeek)
            inputStream.Position = 0;

        var outputStream = new MemoryStream();

        using var aes = Aes.Create();
        var iv = new byte[aes.BlockSize / 8];

        var bytesRead = await inputStream.ReadAsync(iv.AsMemory(0, iv.Length));
        if (bytesRead != iv.Length)
            throw new CryptographicException("Failed to read the initialization vector (IV) from the input stream.");

        aes.Key = key;
        aes.IV = iv;
        await using var cryptoStream =
            new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read, true);

        await cryptoStream.CopyToAsync(outputStream);
        if (cryptoStream.HasFlushedFinalBlock == false)
            await cryptoStream.FlushFinalBlockAsync();


        outputStream.Position = 0;
        return outputStream;
    }
}