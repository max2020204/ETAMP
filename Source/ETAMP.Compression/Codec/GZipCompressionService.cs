using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
/// Provides GZip compression and decompression functionality for data streams.
/// </summary>
/// <remarks>
/// Implements the <see cref="ICompressionService"/> interface to handle GZip-based
/// compression and decompression of streams using the System.IO.Compression.GZipStream.
/// </remarks>
public sealed record GZipCompressionService : ICompressionService
{
    /// <summary>
    ///     Represents a logging instance used to capture and record diagnostic information,
    ///     warnings, errors, or other logging details for the <see cref="GZipCompressionService" />.
    /// </summary>
    /// <remarks>
    ///     This logger is utilized for debugging, error handling, and providing runtime information
    ///     during the compression and decompression operations performed by the service.
    /// </remarks>
    private readonly ILogger<GZipCompressionService> _logger;

    /// <summary>
    ///     Provides methods to compress and decompress data using GZip compression.
    ///     Implements the <see cref="ICompressionService" /> interface.
    /// </summary>
    public GZipCompressionService(ILogger<GZipCompressionService> logger)
    {
        _logger = logger;
    }


    /// <summary>
    ///     Compresses the data from the input pipe reader and writes the compressed output to the output pipe writer using
    ///     GZip compression.
    /// </summary>
    /// <param name="inputData">The pipe reader supplying the data to be compressed.</param>
    /// <param name="outputData">The pipe writer to which the compressed data will be written.</param>
    /// <param name="cancellationToken">An optional token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous compression operation.</returns>
    public async Task CompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor = new GZipStream(outputData.AsStream(), CompressionMode.Compress, true);
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
    ///     Decompresses data from a specified input reader and writes the decompressed data to a specified output writer.
    /// </summary>
    /// <param name="inputData">The <see cref="PipeReader" /> containing the compressed data to be decompressed.</param>
    /// <param name="outputData">The <see cref="PipeWriter" /> where the decompressed data will be written.</param>
    /// <param name="cancellationToken">
    ///     The <see cref="CancellationToken" /> to observe for cancellation requests, defaulting
    ///     to <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task DecompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var decompressor = new GZipStream(inputData.AsStream(), CompressionMode.Decompress, true);
            _logger.LogDebug("Decompressing data stream...");
            await decompressor.CopyToAsync(outputData.AsStream(), cancellationToken);
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