using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

public sealed record DeflateCompressionService : ICompressionService
{
    private readonly ILogger<DeflateCompressionService> _logger;

    public DeflateCompressionService(ILogger<DeflateCompressionService> logger)
    {
        _logger = logger;
    }


    public async Task CompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor =
                new DeflateStream(outputData.AsStream(), CompressionMode.Compress, true);
            _logger.LogDebug("Compressing data stream...");
            await inputData.CopyToAsync(compressor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
            await outputData.CompleteAsync(ex);
        }
        finally
        {
            await outputData.FlushAsync(cancellationToken);
            await outputData.CompleteAsync();
            await inputData.CompleteAsync();
        }
    }


    public async Task DecompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor =
                new DeflateStream(outputData.AsStream(), CompressionMode.Decompress, true);
            _logger.LogDebug("Compressing data stream...");
            await inputData.CopyToAsync(compressor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
            await outputData.CompleteAsync(ex);
        }
        finally
        {
            await outputData.FlushAsync(cancellationToken);
            await outputData.CompleteAsync();
            await inputData.CompleteAsync();
        }
    }
}