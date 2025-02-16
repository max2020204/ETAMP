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


    public async Task CompressAsync(PipeReader inputReader, PipeWriter outputWriter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor = new DeflateStream(outputWriter.AsStream(), CompressionMode.Compress, true);
            _logger.LogDebug("Compressing data stream...");
            await inputReader.CopyToAsync(compressor, cancellationToken);
            await compressor.FlushAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
        }
        finally
        {
            await outputWriter.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
            await inputReader.CompleteAsync();
        }
    }

    public async Task DecompressAsync(PipeReader inputReader, PipeWriter outputWriter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var decompressor =
                new DeflateStream(inputReader.AsStream(), CompressionMode.Decompress, true);
            _logger.LogDebug("Decompressing data stream...");
            await decompressor.CopyToAsync(outputWriter, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Decompression failed.");
        }
        finally
        {
            await outputWriter.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
            await inputReader.CompleteAsync();
        }
    }
}