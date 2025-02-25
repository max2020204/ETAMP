using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides GZip compression and decompression functionality for data streams.
/// </summary>
/// <remarks>
///     Implements the <see cref="ICompressionService" /> interface to handle GZip-based
///     compression and decompression of streams using the System.IO.Compression.GZipStream.
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
        catch (ArgumentNullException argNullException)
        {
            var errorMsg = "One of the streams (input or output) is null. Please verify the provided parameters.";
            _logger.LogError(argNullException, errorMsg);
            throw new ArgumentNullException(errorMsg, argNullException);
        }
        catch (OperationCanceledException canceledException)
        {
            var warningMsg = "Compression operation was canceled.";
            _logger.LogWarning(canceledException, warningMsg);
            throw; // Rethrow to allow the cancellation to be handled by the caller.
        }
        catch (Exception ex)
        {
            var errorMsg = "An error occurred during the compression process.";
            _logger.LogError(ex, errorMsg);
            throw new InvalidOperationException(errorMsg, ex);
        }
        finally
        {
            await CompleteFlushAsync(inputData, outputData, cancellationToken);
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
        catch (ArgumentNullException argNullException)
        {
            var errorMsg = "One of the streams (input or output) is null. Please verify the provided parameters.";
            _logger.LogError(argNullException, errorMsg);
            throw new ArgumentNullException(errorMsg, argNullException);
        }
        catch (OperationCanceledException canceledException)
        {
            _logger.LogWarning(canceledException, "Decompression operation was canceled.");
            throw;
        }
        catch (Exception ex)
        {
            var errorMsg = "An error occurred during the decompression process.";
            _logger.LogError(ex, errorMsg);
            throw new InvalidOperationException(errorMsg, ex);
        }
        finally
        {
            await CompleteFlushAsync(inputData, outputData, cancellationToken);
        }
    }

    /// <summary>
    ///     Completes flushing and finalization of the PipeWriter and PipeReader to ensure proper
    ///     handling of the data stream and resource cleanup.
    /// </summary>
    /// <param name="inputData">
    ///     The <see cref="PipeReader" /> representing the input data that has been read and processed.
    /// </param>
    /// <param name="outputData">
    ///     The <see cref="PipeWriter" /> representing the output data where the processed content is written.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> which can be used to signal cancellation of the operation.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation of finalizing and completing both the reader and
    ///     writer.
    /// </returns>
    private async Task CompleteFlushAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken)
    {
        await outputData.FlushAsync(cancellationToken);
        await outputData.CompleteAsync();
        await inputData.CompleteAsync();
    }
}