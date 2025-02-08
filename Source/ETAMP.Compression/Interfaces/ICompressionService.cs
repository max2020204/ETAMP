using System.IO.Pipelines;

namespace ETAMP.Compression.Interfaces;

/// <summary>
///     Defines a contract for services that can compress and decompress strings.
/// </summary>
public interface ICompressionService
{
    Task CompressAsync(PipeReader inputData, PipeWriter outputData, CancellationToken cancellationToken = default);
    Task DecompressAsync(PipeReader inputData, PipeWriter outputData, CancellationToken cancellationToken = default);
}