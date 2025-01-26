namespace ETAMP.Compression.Interfaces;

/// <summary>
///     Defines a contract for services that can compress and decompress strings.
/// </summary>
public interface ICompressionService
{
    Task<Stream> CompressStream(Stream data, CancellationToken cancellationToken = default);
    Task<Stream> DecompressStream(Stream base64CompressedData, CancellationToken cancellationToken = default);
}