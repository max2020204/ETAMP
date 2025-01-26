#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Provides AES encryption and decryption services.
/// </summary>
public class AESEncryptionService : IEncryptionService
{
    private const int KeySize128 = 16;
    private const int KeySize192 = 24;
    private const int KeySize256 = 32;

    private readonly ILogger<AESEncryptionService> _logger;

    public AESEncryptionService(ILogger<AESEncryptionService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Encrypts the provided data asynchronously using the AES encryption algorithm and streams.
    /// </summary>
    /// <param name="inputStream">The input stream containing the data to be encrypted.</param>
    /// <param name="key">The encryption key. Must be 128, 192, or 256 bits long (16, 24, or 32 bytes).</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A stream containing the encrypted data, including the initialization vector (IV) at the beginning.</returns>
    public async Task<Stream> EncryptAsync(Stream inputStream, byte[] key,
        CancellationToken cancellationToken = default)
    {
        ValidateParameters(inputStream, key);

        var outputStream = new MemoryStream();
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();


        await WriteIVAsync(aes, outputStream, cancellationToken);

        await using var cryptoStream =
            new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write, true);
        await inputStream.CopyToAsync(cryptoStream, cancellationToken);
        FlushFinalBlock(cryptoStream);

        outputStream.Position = 0;
        return outputStream;
    }

    /// <summary>
    /// Decrypts the provided encrypted data asynchronously using the AES encryption algorithm and streams.
    /// </summary>
    /// <param name="inputStream">The input stream with encrypted data.</param>
    /// <param name="key">The decryption key.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A stream containing the decrypted data.</returns>
    public async Task<Stream> DecryptAsync(Stream inputStream, byte[] key,
        CancellationToken cancellationToken = default)
    {
        ValidateParameters(inputStream, key);

        var outputStream = new MemoryStream();
        using var aes = Aes.Create();

        var iv = await ReadIVAsync(inputStream, aes.BlockSize / 8, cancellationToken);
        aes.Key = key;
        aes.IV = iv;

        await using var cryptoStream =
            new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read, true);
        await cryptoStream.CopyToAsync(outputStream, cancellationToken);
        FlushFinalBlock(cryptoStream);

        outputStream.Position = 0;

        return outputStream;
    }

    private void ValidateParameters(Stream inputStream, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(inputStream, nameof(inputStream));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (key.Length is not (KeySize128 or KeySize192 or KeySize256))
        {
            _logger.LogError("Invalid key length: {KeyLength} bytes.", key.Length);
            throw new ArgumentException("Key must be 128, 192, or 256 bits long.", nameof(key));
        }

        if (!inputStream.CanSeek)
        {
            _logger.LogError("Input stream must support seeking.");
            throw new NotSupportedException("Input stream must support seeking.");
        }

        inputStream.Position = 0;
    }

    private async Task WriteIVAsync(Aes aes, Stream outputStream, CancellationToken cancellationToken)
    {
        await outputStream.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length), cancellationToken);
        _logger.LogInformation("IV written to output stream.");
    }

    private async Task<byte[]> ReadIVAsync(Stream inputStream, int ivLength, CancellationToken cancellationToken)
    {
        var iv = new byte[ivLength];
        var bytesRead = await inputStream.ReadAsync(iv.AsMemory(0, iv.Length), cancellationToken);

        if (bytesRead != iv.Length)
        {
            _logger.LogError("Failed to read the initialization vector (IV) from the input stream.");
            throw new CryptographicException("Failed to read the initialization vector (IV) from the input stream.");
        }

        return iv;
    }

    private void FlushFinalBlock(CryptoStream cryptoStream)
    {
        if (cryptoStream.HasFlushedFinalBlock)
            return;
        cryptoStream.FlushFinalBlock();
    }
}