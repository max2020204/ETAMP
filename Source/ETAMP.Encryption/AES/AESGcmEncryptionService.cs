using System.Buffers;
using System.IO.Pipelines;
using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Encryption.AES;

/// <summary>
/// Provides encryption and decryption functionality using the AES-GCM (Galois/Counter Mode) algorithm.
/// </summary>
public class AESGcmEncryptionService : IEncryptionService
{
    private const int IvSize = 12;
    private const int TagSize = 16;
    private const int KeySize128 = 16;
    private const int KeySize192 = 24;
    private const int KeySize256 = 32;

    private readonly ILogger<AESGcmEncryptionService> _logger;

    public AESGcmEncryptionService(ILogger<AESGcmEncryptionService> logger)
    {
        _logger = logger;
    }

    #region Encrypt

    public async Task EncryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key,
        CancellationToken cancellationToken)
    {
        ValidateKey(key);
        using var owner = MemoryPool<byte>.Shared.Rent();
        var plainBuf = owner.Memory;
        var plainLen = 0;
        while (true)
        {
            var readResult = await inputReader.ReadAsync(cancellationToken);
            foreach (var seg in readResult.Buffer)
            {
                seg.CopyTo(plainBuf[plainLen..]);
                plainLen += seg.Length;
            }

            inputReader.AdvanceTo(readResult.Buffer.End);
            if (readResult.IsCompleted) break;
        }

        var iv = new byte[IvSize];
        RandomNumberGenerator.Fill(iv);

        var cipher = ArrayPool<byte>.Shared.Rent(plainLen);
        var tag = ArrayPool<byte>.Shared.Rent(TagSize);
        try
        {
            using var aes = new AesGcm(key, TagSize);
            aes.Encrypt(iv, plainBuf.Span[..plainLen], cipher.AsSpan(0, plainLen), tag);

            await outputWriter.WriteAsync(iv, cancellationToken);
            await outputWriter.WriteAsync(cipher, cancellationToken);
            await outputWriter.WriteAsync(tag, cancellationToken);

            await outputWriter.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AES‑GCM encryption failed.");
            await outputWriter.CompleteAsync(ex);
            throw;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(cipher, clearArray: true);
            ArrayPool<byte>.Shared.Return(tag, clearArray: true);
            await inputReader.CompleteAsync();
        }
    }

    #endregion

    #region Decrypt

    public async Task DecryptAsync(PipeReader inputReader, PipeWriter outputWriter, byte[] key,
        CancellationToken ct = default)
    {
        ValidateKey(key);

        // 1. Читаем IV
        byte[] iv = await ReadExactAsync(inputReader, IvSize, ct);

        // 2. Считываем остаток → cipher + tag
        ReadResult rr = await inputReader.ReadAsync(ct);
        var buf = rr.Buffer;
        if (buf.Length < TagSize)
            throw new CryptographicException("Payload too short.");

        int cipherLen = (int)buf.Length - TagSize;
        byte[] cipher = ArrayPool<byte>.Shared.Rent(cipherLen);
        byte[] tag = ArrayPool<byte>.Shared.Rent(TagSize);

        buf.Slice(0, cipherLen).CopyTo(cipher);
        buf.Slice(cipherLen, TagSize).CopyTo(tag);
        inputReader.AdvanceTo(buf.End);

        try
        {
            using var aes = new AesGcm(key, TagSize);
            byte[] plain = ArrayPool<byte>.Shared.Rent(cipherLen);
            aes.Decrypt(iv, cipher.AsSpan(0, cipherLen), tag, plain.AsSpan(0, cipherLen));

            await outputWriter.WriteAsync(plain.AsMemory(0, cipherLen), ct);
            await outputWriter.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AES‑GCM decryption failed.");
            await outputWriter.CompleteAsync(ex);
            throw;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(cipher, clearArray: true);
            ArrayPool<byte>.Shared.Return(tag, clearArray: true);
            await inputReader.CompleteAsync();
        }
    }

    #endregion

    #region Helpers

    private static async Task<byte[]> ReadExactAsync(PipeReader reader, int len, CancellationToken ct)
    {
        var readResult = await reader.ReadAsync(ct);
        if (readResult.Buffer.Length < len)
            throw new CryptographicException("Unexpected end of stream.");

        var dst = new byte[len];
        readResult.Buffer.Slice(0, len).CopyTo(dst);
        reader.AdvanceTo(readResult.Buffer.GetPosition(len));
        return dst;
    }

    private void ValidateKey(byte[] key)
    {
        if (key.Length is KeySize128 or KeySize192 or KeySize256)
            return;

        _logger.LogError("Invalid AES‑GCM key length: {Len} bytes.", key.Length);
        throw new ArgumentException("Key must be 128/192/256 bits.", nameof(key));
    }

    #endregion
}