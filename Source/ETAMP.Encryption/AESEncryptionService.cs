using System.Buffers;
using System.IO.Pipelines;
using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Encryption;

/// <summary>
///     Provides AES encryption and decryption services using Pipe for streaming.
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
    /// Encrypts the provided data using AES encryption and Pipe.
    /// </summary>
    public async Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key,
        CancellationToken cancellationToken = default)
    {
        ValidateKey(key);
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        await outputWriter.WriteAsync(aes.IV, cancellationToken);

        try
        {
            await using var cryptoStream = new CryptoStream(outputWriter.AsStream(), aes.CreateEncryptor(),
                CryptoStreamMode.Write, true);

            var readResult = await inputReader.ReadAsync(cancellationToken);
            var buffer = readResult.Buffer;
            while (true)
            {
                foreach (var segment in buffer)
                {
                    await cryptoStream.WriteAsync(segment, cancellationToken);
                }

                inputReader.AdvanceTo(buffer.End);

                if (readResult.IsCompleted)
                    break;
            }

            await cryptoStream.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Encryption failed.");
            await outputWriter.CompleteAsync(ex);
            throw;
        }
        finally
        {
            await inputReader.CompleteAsync();
        }
    }

    /// <summary>
    /// Decrypts the provided encrypted data using AES and Pipe.
    /// </summary>
    public async Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key,
        CancellationToken cancellationToken = default)
    {
        ValidateKey(key);

        using var aes = Aes.Create();
        var iv = await ReadIVAsync(inputReader, aes.BlockSize / 8, cancellationToken);
        aes.Key = key;
        aes.IV = iv;

        try
        {
            await using var cryptoStream = new CryptoStream(outputWriter.AsStream(), aes.CreateDecryptor(),
                CryptoStreamMode.Write, true);
            var readResult = await inputReader.ReadAsync(cancellationToken);
            var buffer = readResult.Buffer;
            while (true)
            {
                foreach (var segment in buffer)
                {
                    await cryptoStream.WriteAsync(segment, cancellationToken);
                }

                inputReader.AdvanceTo(buffer.End);

                if (readResult.IsCompleted)
                    break;
            }

            await cryptoStream.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Decryption failed.");
            await outputWriter.CompleteAsync(ex);
            throw;
        }
        finally
        {
            await inputReader.CompleteAsync();
        }
    }

    /// <summary>
    /// Reads the IV (initialization vector) from the input Pipe.
    /// </summary>
    private async Task<byte[]> ReadIVAsync(PipeReader inputReader, int ivLength, CancellationToken cancellationToken)
    {
        var result = await inputReader.ReadAsync(cancellationToken);
        var buffer = result.Buffer;

        if (buffer.Length < ivLength)
            throw new CryptographicException("Failed to read the IV (initialization vector).");

        var iv = buffer.Slice(0, ivLength).ToArray();
        inputReader.AdvanceTo(buffer.GetPosition(ivLength));

        return iv;
    }

    /// <summary>
    /// Validates the encryption key size.
    /// </summary>
    private void ValidateKey(byte[] key)
    {
        if (key.Length is KeySize128 or KeySize192 or KeySize256)
            return;

        _logger.LogError("Invalid key length: {KeyLength} bytes.", key.Length);
        throw new ArgumentException("Key must be 128, 192, or 256 bits long.", nameof(key));
    }
}