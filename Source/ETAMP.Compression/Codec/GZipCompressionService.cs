using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides functionality for compressing and decompressing string data using GZip compression.
///     This class implements the <c>ICompressionService</c> interface and provides methods
///     to compress a string into a compressed Base64-encoded format and to decompress
///     a Base64-encoded compressed string back to its original format.
/// </summary>
public sealed record GZipCompressionService : ICompressionService
{
    private readonly ILogger<GZipCompressionService> _logger;

    public GZipCompressionService(ILogger<GZipCompressionService> logger)
    {
        _logger = logger;
    }


    public async Task CompressAsync(PipeReader inputReader, PipeWriter outputWriter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor = new GZipStream(outputWriter.AsStream(), CompressionMode.Compress, true);
            _logger.LogDebug("Compressing data stream...");
            await inputReader.CopyToAsync(compressor, cancellationToken);
            await compressor.FlushAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
        }
        finally
        {
            await outputWriter.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
            await inputReader.CompleteAsync();
        }
    }

    public async Task DecompressAsync(PipeReader inputReader, PipeWriter outputWriter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var decompressor = new GZipStream(inputReader.AsStream(), CompressionMode.Decompress, true);
            _logger.LogDebug("Decompressing data stream...");
            await decompressor.CopyToAsync(outputWriter.AsStream(), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Decompression failed.");
        }
        finally
        {
            await outputWriter.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
            await inputReader.CompleteAsync();
        }
    }
}