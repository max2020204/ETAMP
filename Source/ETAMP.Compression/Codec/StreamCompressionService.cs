using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
/// Abstract base class that provides foundational methods for stream-based data compression and decompression.
/// </summary>
/// <remarks>
/// This class defines the structure for creating compression and decompression streams and managing
/// asynchronous compression and decompression workflows. Specific implementations for different compression
/// algorithms need to inherit from this class and provide concrete implementations of the required methods.
/// </remarks>
public abstract class StreamCompressionService : ICompressionService
{
    /// <summary>
    /// Represents a logger instance used to log messages and exceptions during compression
    /// and decompression operations in the <see cref="StreamCompressionService"/>.
    /// </summary>
    /// <remarks>
    /// This logger is utilized to record debug information, warnings, and errors to aid in
    /// diagnostics and monitoring of the compression service.
    /// </remarks>
    private readonly ILogger _logger;

    /// <summary>
    /// An abstract base class for stream compression and decompression services.
    /// </summary>
    /// <remarks>
    /// This class provides the core framework for implementing stream-based compression and decompression
    /// functionality. It defines abstract methods for creating compression and decompression streams
    /// that are implemented by derived classes, as well as asynchronous methods for performing compression
    /// and decompression operations.
    /// </remarks>
    protected StreamCompressionService(ILogger logger)
    {
        _logger = logger;
    }

    /// Compresses the data from the specified input pipe reader and writes the compressed result to the specified output pipe writer.
    /// This method utilizes a compression stream and handles potential exceptions during the compression process.
    /// <param name="inputData">The <see cref="PipeReader"/> providing input data to be compressed.</param>
    /// <param name="outputData">The <see cref="PipeWriter"/> for writing the compressed data.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous compression operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either the input or output stream is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the compression operation is canceled.</exception>
    /// <exception cref="InvalidOperationException">Thrown when an unexpected error occurs during compression.</exception>
    public async Task CompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor = CreateCompressionStream(outputData.AsStream(), CompressionMode.Compress);
            _logger.LogDebug("Compressing data stream...");
            await inputData.CopyToAsync(compressor, cancellationToken);
            await compressor.FlushAsync(cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "One of the streams (input or output) is null.");
            throw new ArgumentNullException("One of the streams (input or output) is null.", ex);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Compression operation was canceled.");
            throw;
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
    /// Decompresses a stream of input data and writes the decompressed data to the specified output stream.
    /// </summary>
    /// <param name="inputData">
    /// The <see cref="PipeReader"/> representing the input stream containing compressed data.
    /// </param>
    /// <param name="outputData">
    /// The <see cref="PipeWriter"/> representing the output stream where the decompressed data will be written.
    /// </param>
    /// <param name="cancellationToken">
    /// Optional. A <see cref="CancellationToken"/> to monitor for cancellation requests during the decompression operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous decompression operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when either the input or output stream is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown if the decompression operation is canceled via the provided <paramref name="cancellationToken"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an error occurs during decompression.
    /// </exception>
    public async Task DecompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var decompressor = CreateDecompressionStream(inputData.AsStream(), CompressionMode.Decompress);
            _logger.LogDebug("Decompressing data stream...");
            await decompressor.CopyToAsync(outputData.AsStream(), cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "One of the streams (input or output) is null.");
            throw new ArgumentNullException("One of the streams (input or output) is null.", ex);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Decompression operation was canceled.");
            throw;
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
    /// Creates a compression stream to compress or decompress data.
    /// </summary>
    /// <param name="output">The output stream where data will be written.</param>
    /// <param name="mode">The <see cref="CompressionMode"/> specifying whether to compress or decompress the data.</param>
    /// <returns>A <see cref="Stream"/> configured for compression or decompression based on the specified mode.</returns>
    protected abstract Stream CreateCompressionStream(Stream output, CompressionMode mode);

    /// <summary>
    /// Creates a decompression stream for decompressing data from the specified input stream.
    /// </summary>
    /// <param name="input">The input stream containing the compressed data to be decompressed.</param>
    /// <param name="mode">
    /// The compression mode specifying the operation to perform.
    /// For decompression, this should typically be set to <see cref="CompressionMode.Decompress"/>.
    /// </param>
    /// <returns>A stream configured for decompressing data.</returns>
    protected abstract Stream CreateDecompressionStream(Stream input, CompressionMode mode);

    /// <summary>
    /// Completes the flushing of the provided PipeReader and PipeWriter streams and ensures that resources are finalized appropriately.
    /// </summary>
    /// <param name="inputData">The <see cref="PipeReader"/> representing the input data stream.</param>
    /// <param name="outputData">The <see cref="PipeWriter"/> representing the output data stream.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the operation to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private async Task CompleteFlushAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken)
    {
        await outputData.FlushAsync(cancellationToken);
        await outputData.CompleteAsync();
        await inputData.CompleteAsync();
    }
}