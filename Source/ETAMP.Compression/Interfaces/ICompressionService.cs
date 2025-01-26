namespace ETAMP.Compression.Interfaces;

/// <summary>
///     Defines a contract for services that can compress and decompress strings.
/// </summary>
public interface ICompressionService
{
    /// <summary>
    /// Compresses the provided string data using the implemented compression algorithm.
    /// </summary>
    /// <param name="data">The data to be compressed. Can be null.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the compressed string, or null if the input data is null.</returns>
    Task<string>? CompressString(string? data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Decompresses a string that has been compressed and encoded in Base64 format.
    /// </summary>
    /// <param name="base64CompressedData">The Base64 encoded compressed string to be decompressed. Can be null.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation, containing the decompressed string.</returns>
    Task<string> DecompressString(string? base64CompressedData, CancellationToken cancellationToken = default);
}