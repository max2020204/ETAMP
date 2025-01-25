#region

using System.IO.Compression;
using System.Text;
using ETAMP.Compression.Interfaces;
using ETAMP.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides functionality for compressing and decompressing string data using GZip compression.
///     This class implements the <c>ICompressionService</c> interface and provides methods
///     to compress a string into a compressed Base64-encoded format and to decompress
///     a Base64-encoded compressed string back to its original format.
/// </summary>
public sealed class GZipCompressionService : ICompressionService
{
    private readonly ILogger<GZipCompressionService> _logger;

    public GZipCompressionService(ILogger<GZipCompressionService>? logger = null)
    {
        _logger = logger ?? NullLogger<GZipCompressionService>.Instance;
    }

    /// <summary>
    ///     Compresses the provided string using GZip compression and encodes the result in Base64 URL format.
    /// </summary>
    /// <param name="data">The string to be compressed. Must not be null, empty, or whitespace.</param>
    /// <returns>A Base64 URL encoded string representing the compressed data.</returns>
    /// <exception cref="ArgumentException">Thrown when the input data is null, empty, or whitespace.</exception>
    public async Task<string> CompressString(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            _logger.LogError("The input data must not be null, empty, or consist only of whitespace.");
            throw new ArgumentException("The input data must not be null, empty, or consist only of whitespace.",
                nameof(data));
        }

        _logger.LogDebug("Input data length: {0}", data.Length);
        var bytes = Encoding.UTF8.GetBytes(data);

        using var outputStream = new MemoryStream();
        await using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress, true))
        {
            _logger.LogDebug("Compressing data");
            await gzipStream.WriteAsync(bytes);
            _logger.LogDebug("Data compressed");
            _logger.LogDebug("Output data length: {0}", outputStream.Length);
        }

        outputStream.Position = 0;
        _logger.LogDebug("Encoding data");
        return Base64UrlEncoder.Encode(outputStream.ToArray());
    }

    /// Decompresses a Base64 encoded, GZIP-compressed string into its original uncompressed format.
    /// <param name="base64CompressedData">
    ///     The Base64 encoded string that represents GZIP-compressed data.
    ///     Must not be null, empty, or whitespace.
    /// </param>
    /// <returns>
    ///     The decompressed string in its original format.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the input string is null, empty, or contains only whitespace.
    /// </exception>
    /// <exception cref="InvalidDataException">
    ///     Thrown if the input data is not in a valid GZIP-compressed format or is corrupted.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the decompressed data is empty or an error occurs during the decompression process.
    /// </exception>
    public async Task<string> DecompressString(string? base64CompressedData)
    {
        if (string.IsNullOrWhiteSpace(base64CompressedData))
        {
            _logger.LogError("The input data must not be null, empty, or consist only of whitespace.");
            throw new ArgumentException("The input data must not be null, empty, or consist only of whitespace.",
                nameof(base64CompressedData));
        }

        var compressedBytes = Base64UrlEncoder.DecodeBytes(base64CompressedData);
        _logger.LogDebug("Decompressing data");

        using var inputStream = new MemoryStream(compressedBytes);
        using var outputStream = new MemoryStream();
        try
        {
            await using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            await gzipStream.CopyToAsync(outputStream);
            _logger.LogDebug("Data decompressed");
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex, "Failed to decompress the data");
            throw new InvalidDataException(
                "Failed to decompress the data. The input data may be in an unsupported or corrupted format.", ex);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred during the decompression process.");
            throw new InvalidOperationException("The input data is invalid or corrupted.", e);
        }

        _logger.LogDebug("Output data length: {0}", outputStream.Length);

        outputStream.Position = 0;
        var decompressedBytes = outputStream.ToArray();
        _logger.LogDebug("Decompressed data length: {0}", decompressedBytes.Length);

        if (decompressedBytes.Length != 0)
            return Encoding.UTF8.GetString(decompressedBytes);

        _logger.LogError("The decompressed data is empty.");
        throw new InvalidOperationException(
            "The decompressed data is empty. The input might be invalid or corrupted.");
    }
}