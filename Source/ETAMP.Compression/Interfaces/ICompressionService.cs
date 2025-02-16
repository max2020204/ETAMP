using System.IO.Pipelines;

namespace ETAMP.Compression.Interfaces;

/// <summary>
/// Interface for services that provide methods to compress and decompress data streams.
/// </summary>
public interface ICompressionService
{
    /// <summary>
    ///     Compresses the input data stream and writes the compressed data to the output stream asynchronously.
    /// </summary>
    /// <param name="inputData">The <see cref="PipeReader" /> instance for reading the data to be compressed.</param>
    /// <param name="outputData">The <see cref="PipeWriter" /> instance for writing the compressed data.</param>
    /// <param name="cancellationToken">
    ///     An optional <see cref="CancellationToken" /> to observe while waiting for the task to
    ///     complete.
    /// </param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
    Task CompressAsync(PipeReader inputData, PipeWriter outputData, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Decompresses data from an input PipeReader and writes the decompressed data to an output PipeWriter.
    /// </summary>
    /// <param name="inputData">The PipeReader containing the compressed input data to be decompressed.</param>
    /// <param name="outputData">The PipeWriter where the decompressed output data will be written.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous decompression operation.</returns>
    Task DecompressAsync(PipeReader inputData, PipeWriter outputData, CancellationToken cancellationToken = default);
}