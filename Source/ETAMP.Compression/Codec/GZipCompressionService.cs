using System.IO.Compression;
using System.IO.Pipelines;
using ETAMP.Compression.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides functionality for compressing and decompressing string data using GZip compression.
///     This class implements the <c>ICompressionService</c> interface and provides methods
///     to compress a string into a compressed Base64-encoded format and to decompress
///     a Base64-encoded compressed string back to its original format.
/// </summary>
public sealed class GZipCompressionService : ICompressionService
{
    private readonly ILogger<GZipCompressionService> _logger;

    public GZipCompressionService(ILogger<GZipCompressionService> logger)
    {
        _logger = logger;
    }


    public async Task CompressAsync(PipeReader inputReader, PipeWriter outputWriter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor =
                new GZipStream(outputWriter.AsStream(), CompressionMode.Compress, leaveOpen: true);

            while (true)
            {
                var readResult = await inputReader.ReadAsync(cancellationToken);
                var buffer = readResult.Buffer;

                foreach (var segment in buffer)
                {
                    await compressor.WriteAsync(segment, cancellationToken);
                }

                inputReader.AdvanceTo(buffer.End);

                if (readResult.IsCompleted)
                    break;
            }

            await compressor.FlushAsync(cancellationToken);
            await outputWriter.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
            await outputWriter.CompleteAsync(ex);
            throw;
        }
        finally
        {
            await inputReader.CompleteAsync();
        }
    }


    public async Task DecompressAsync(PipeReader inputData, PipeWriter outputData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var compressor =
                new GZipStream(outputData.AsStream(), CompressionMode.Compress, leaveOpen: true);

            while (true)
            {
                var readResult = await inputData.ReadAsync(cancellationToken);
                var buffer = readResult.Buffer;

                foreach (var segment in buffer)
                {
                    await compressor.WriteAsync(segment, cancellationToken);
                }

                inputData.AdvanceTo(buffer.End);

                if (readResult.IsCompleted)
                    break;
            }

            await compressor.FlushAsync(cancellationToken);
            await outputData.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compression failed.");
            await outputData.CompleteAsync(ex);
            throw;
        }
        finally
        {
            await inputData.CompleteAsync();
        }
    }
}