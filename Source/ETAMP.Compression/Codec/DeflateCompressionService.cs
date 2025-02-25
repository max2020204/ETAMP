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
    /// Compresses the input data stream and writes the compressed data to the output stream.
    /// </summary>
    /// <param name="inputData">The input stream of data to be compressed.</param>
    /// <param name="outputData">The output stream where the compressed data is written to.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous compression operation.</returns>
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
            throw new InvalidOperationException("Compression failed.", ex);
        }
        finally
        {
            await CompleteFlushAsync(inputData, outputData, cancellationToken);
        }
    }

    /// <summary>
    /// Asynchronously decompresses data from the specified input stream and writes the decompressed data
    /// to the specified output stream using the Deflate decompression algorithm.
    /// </summary>
    /// <param name="inputData">
    /// The <see cref="PipeReader" /> representing the compressed input data stream to decompress.
    /// </param>
    /// <param name="outputData">
    /// The <see cref="PipeWriter" /> where the decompressed data will be written.
    /// </param>
    /// <param name="cancellationToken">
    /// An optional <see cref="CancellationToken" /> to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous decompression operation.
    /// </returns>
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
            throw new InvalidOperationException("Decompression failed.", ex);
        }
        finally
        {
            await CompleteFlushAsync(inputData, outputData, cancellationToken);
        }
    }

    /// <summary>
    ///     Completes the flushing and finalization of both the input and output data pipelines.
    /// </summary>
    /// <param name="inputData">
    ///     The <see cref="PipeReader" /> representing the input data stream to be finalized.
    /// </param>
    /// <param name="outputData">
    ///     The <see cref="PipeWriter" /> representing the output data stream to be finalized and flushed.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous operation.
    /// </returns>
    private async Task CompleteFlushAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken)
    {
        await outputData.FlushAsync(cancellationToken);
        await outputData.CompleteAsync();
        await inputData.CompleteAsync();
    }
}