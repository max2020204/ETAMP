#region

using System.IO.Compression;
using ETAMP.Compression.Interfaces;
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


    public async Task<Stream> CompressStream(Stream data, CancellationToken cancellationToken = default)
    {
        if (data is not { CanRead: true })
        {
            _logger.LogError("The input stream must not be null and must be readable.");
            throw new ArgumentException("The input stream must not be null and must be readable.", nameof(data));
        }

        var outputStream = new MemoryStream();
        await using (var compressor = new GZipStream(outputStream, CompressionMode.Compress, true))
        {
            _logger.LogDebug("Compressing data stream...");
            await data.CopyToAsync(compressor, cancellationToken);
        }

        _logger.LogDebug("Data stream compressed.");
        outputStream.Position = 0;

        return outputStream;
    }


    public async Task<Stream> DecompressStream(Stream compressedStream, CancellationToken cancellationToken = default)
    {
        if (compressedStream is not { CanRead: true })
        {
            _logger.LogError("The input stream must not be null and must be readable.");
            throw new ArgumentException("The input stream must not be null and must be readable.",
                nameof(compressedStream));
        }

        var outputStream = new MemoryStream();

        try
        {
            await using var decompressor = new GZipStream(compressedStream, CompressionMode.Decompress);
            _logger.LogDebug("Decompressing data stream...");
            await decompressor.CopyToAsync(outputStream, cancellationToken);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex, "Failed to decompress the stream.");
            throw new InvalidDataException(
                "Failed to decompress the stream. The input data may be invalid or corrupted.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during decompression.");
            throw new InvalidOperationException("An unexpected error occurred during decompression.", ex);
        }

        if (outputStream.Length == 0)
        {
            _logger.LogError("The decompressed data is empty.");
            throw new InvalidOperationException(
                "The decompressed data is empty. This may indicate invalid or corrupted input data.");
        }

        outputStream.Position = 0;

        _logger.LogDebug("Data stream decompressed successfully.");
        return outputStream;
    }
}