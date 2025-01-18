#region

using System.IO.Compression;
using System.Text;
using ETAMP.Compression.Interfaces;
using ETAMP.Core.Utils;

#endregion

namespace ETAMP.Compression.Codec;

/// <summary>
/// Provides functionality for compressing and decompressing string data using Deflate compression.
/// </summary>
public sealed class DeflateCompressionService : ICompressionService
{
    /// <summary>
    /// Compresses the provided string using Deflate compression and encodes the result in Base64 URL format.
    /// </summary>
    /// <param name="data">The string to be compressed. Must not be null, empty, or consist only of whitespace.</param>
    /// <returns>A Base64 URL encoded string representing the compressed data.</returns>
    /// <exception cref="ArgumentException">Thrown when the input data is null, empty, or consists only of whitespace.</exception>
    public async Task<string> CompressString(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
            throw new ArgumentException("The input data must not be null, empty, or consist only of whitespace.",
                nameof(data));

        var inputBytes = Encoding.UTF8.GetBytes(data);

        using var outputStream = new MemoryStream();
        await using (var compressor = new DeflateStream(outputStream, CompressionMode.Compress, true))
        {
            await compressor.WriteAsync(inputBytes);
        }

        outputStream.Position = 0;

        return Base64UrlEncoder.Encode(outputStream.ToArray());
    }

    /// <summary>
    /// Decompresses a Base64-encoded, Deflate-compressed string into its original uncompressed format.
    /// </summary>
    /// <param name="base64CompressedData">
    /// The Base64-encoded string that represents Deflate-compressed data. Must not be null, empty, or consist only of whitespace.
    /// </param>
    /// <returns>The decompressed string in its original format.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the input string is null, empty, or consists only of whitespace.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Thrown if the input data is not in a valid Deflate-compressed format or is corrupted.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the decompressed data is empty or an unexpected error occurs during the decompression process.
    /// </exception>
    public async Task<string> DecompressString(string? base64CompressedData)
    {
        if (string.IsNullOrWhiteSpace(base64CompressedData))
            throw new ArgumentException("The input data must not be null, empty, or consist only of whitespace.",
                nameof(base64CompressedData));

        var compressedBytes = Base64UrlEncoder.DecodeBytes(base64CompressedData);

        using var inputStream = new MemoryStream(compressedBytes);
        using var outputStream = new MemoryStream();
        try
        {
            await using var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress);
            await decompressor.CopyToAsync(outputStream);
        }
        catch (InvalidDataException ex)
        {
            throw new InvalidDataException(
                "Failed to decompress the data. The input data may be in an unsupported or corrupted format.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred during the decompression process.", ex);
        }

        outputStream.Position = 0;
        var decompressedBytes = outputStream.ToArray();

        if (decompressedBytes.Length == 0)
            throw new InvalidOperationException(
                "The decompressed data is empty. This may indicate invalid or corrupted input data.");

        return Encoding.UTF8.GetString(decompressedBytes);
    }
}