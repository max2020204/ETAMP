#region

using System.IO.Compression;
using System.Text;
using ETAMP.Compression.Interfaces;
using ETAMP.Core.Utils;
using Microsoft.Extensions.Logging;

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

    public GZipCompressionService(ILogger<GZipCompressionService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Compresses the provided string using GZip compression and encodes the result in Base64 URL format.
    /// </summary>
    /// <param name="data">The string to be compressed. Must not be null, empty, or whitespace.</param>
    /// <returns>A Base64 URL encoded string representing the compressed data.</returns>
    /// <exception cref="ArgumentException">Thrown when the input data is null, empty, or whitespace.</exception>
    public async Task<string> CompressString(string? data, CancellationToken cancellationToken = default)
    {
        ValidateInput(data);

        var bytes = Encoding.UTF8.GetBytes(data);

        await using var outputStream = new MemoryStream();
        await using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress, true))
        {
            await gzipStream.WriteAsync(bytes, cancellationToken);
        }


        outputStream.Position = 0;
        return Base64UrlEncoder.Encode(outputStream.ToArray());
    }

    /// <summary>
    /// Decompresses a Base64 encoded, GZIP-compressed string into its original uncompressed format.
    /// </summary>
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
    public async Task<string> DecompressString(string? base64CompressedData,
        CancellationToken cancellationToken = default)
    {
        ValidateInput(base64CompressedData);

        var compressedBytes = Base64UrlEncoder.DecodeBytes(base64CompressedData!);

        await using var inputStream = new MemoryStream(compressedBytes);
        await using var outputStream = new MemoryStream();

        try
        {
            await using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            await gzipStream.CopyToAsync(outputStream, cancellationToken);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex, "Failed to decompress the data");
            throw new InvalidDataException(
                "Failed to decompress the data. The input data may be in an unsupported or corrupted format.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during the decompression process.");
            throw new InvalidOperationException("The input data is invalid or corrupted.", ex);
        }

        if (outputStream.Length == 0)
        {
            _logger.LogError("The decompressed data is empty.");
            throw new InvalidOperationException(
                "The decompressed data is empty. The input might be invalid or corrupted.");
        }

        outputStream.Position = 0;
        var result = await new StreamReader(outputStream, Encoding.UTF8).ReadToEndAsync();

        return result;
    }

    private void ValidateInput(string? data)
    {
        if (!string.IsNullOrWhiteSpace(data))
            return;
        _logger.LogError("The input data must not be null, empty, or consist only of whitespace.");
        throw new ArgumentException("The input data must not be null, empty, or consist only of whitespace.",
            nameof(data));
    }
}