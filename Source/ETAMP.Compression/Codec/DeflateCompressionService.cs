using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides methods for compressing and decompressing data streams using the Deflate compression algorithm.
///     This class implements the <see cref="ICompressionService" /> interface.
/// </summary>
public sealed record DeflateCompressionService : ICompressionService
{
    /// <summary>
    ///     Represents the logger instance used for logging informational messages, debugging details,
    ///     and error reports related to compression and decompression operations.
    /// </summary>
    private readonly ILogger<DeflateCompressionService> _logger;

    /// <summary>
    ///     Provides a service for compressing and decompressing data streams using the Deflate algorithm.
    /// </summary>
    public DeflateCompressionService(ILogger<DeflateCompressionService> logger)
    {
        _logger = logger;
    }


    /// <summary>
    ///     Asynchronously compresses data from a <see cref="PipeReader" /> to a <see cref="PipeWriter" /> using Deflate
    ///     compression.
    /// </summary>
    /// <param name="inputData">The <see cref="PipeReader" /> from which to read the uncompressed input data.</param>
    /// <param name="outputData">The <see cref="PipeWriter" /> to which the compressed data will be written.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous compression operation.</returns>
    public async Task CompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor = new DeflateStream(outputData.AsStream(), CompressionMode.Compress, true);
            _logger.LogDebug("Compressing data stream...");
            await inputData.CopyToAsync(compressor, cancellationToken);
            await compressor.FlushAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
        }
        finally
        {
            await outputData.FlushAsync(cancellationToken);
            await outputData.CompleteAsync();
            await inputData.CompleteAsync();
        }
    }

    /// <summary>
    ///     Asynchronously decompresses data from the input stream and writes the decompressed data to the output stream.
    /// </summary>
    /// <param name="inputData">The <see cref="PipeReader" /> representing the compressed input stream.</param>
    /// <param name="outputData">The <see cref="PipeWriter" /> to which the decompressed data will be written.</param>
    /// <param name="cancellationToken">
    ///     An optional <see cref="CancellationToken" /> to observe while waiting for the operation
    ///     to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous decompression operation.</returns>
    public async Task DecompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var decompressor =
                new DeflateStream(inputData.AsStream(), CompressionMode.Decompress, true);
            _logger.LogDebug("Decompressing data stream...");
            await decompressor.CopyToAsync(outputData, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Decompression failed.");
        }
        finally
        {
            await outputData.FlushAsync(cancellationToken);
            await outputData.CompleteAsync();
            await inputData.CompleteAsync();
        }
    }
}