using System.IO.Pipelines;
using System.Text.Json;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression;

public record CompressionManager : ICompressionManager
{
    private readonly ICompressionServiceFactory _compressionServiceFactory;
    private readonly ILogger<ICompressionManager> _logger;

    public CompressionManager(ICompressionServiceFactory compressionServiceFactory, ILogger<ICompressionManager> logger)
    {
        _compressionServiceFactory = compressionServiceFactory;
        _logger = logger;
    }

    public async Task<ETAMPModelBuilder> CompressAsync<T>(ETAMPModel<T> model,
        CancellationToken cancellationToken = default) where T : Token
    {
        _logger.LogInformation("Starting compression for model Id: {ModelId} with compression type: {CompressionType}",
            model.Id, model.CompressionType);

        Pipe dataPipe = new();
        Pipe outputData = new();

        CheckCompressionType(model.CompressionType);

        var compression = _compressionServiceFactory.Get(model.CompressionType);
        _logger.LogDebug("Compression service created for type: {CompressionType}", model.CompressionType);

        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(model.Token);
        _logger.LogDebug("Serialized model.Token to JSON bytes, size: {Size} bytes", jsonBytes.Length);

        await dataPipe.Writer.WriteAsync(jsonBytes, cancellationToken);
        _logger.LogDebug("Written JSON bytes to data pipe writer.");

        await dataPipe.Writer.FlushAsync(cancellationToken);
        await dataPipe.Writer.CompleteAsync();
        _logger.LogDebug("Data pipe writer completed.");

        await compression.CompressAsync(dataPipe.Reader, outputData.Writer, cancellationToken);
        _logger.LogDebug("Compression service finished compressing data.");

        using var ms = await ReadAllBytesFromPipeAsync(outputData.Reader, cancellationToken);
        var compressedBytes = ms.ToArray();

        var token = Base64UrlEncoder.Encode(compressedBytes);
        _logger.LogInformation("Compression successful for model Id: {ModelId}, resulting token length: {TokenLength}",
            model.Id, token.Length);

        return new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            UpdateType = model.UpdateType,
            Token = token,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
    }


    public async Task<ETAMPModel<T>> DecompressAsync<T>(ETAMPModelBuilder model,
        CancellationToken cancellationToken = default) where T : Token
    {
        _logger.LogInformation(
            "Starting decompression for model Id: {ModelId} with compression type: {CompressionType}",
            model.Id, model.CompressionType);

        Pipe dataPipe = new();
        Pipe outputData = new();

        CheckCompressionType(model.CompressionType);

        var compression = _compressionServiceFactory.Get(model.CompressionType);
        _logger.LogDebug("Decompression service created for type: {CompressionType}", model.CompressionType);

        if (string.IsNullOrWhiteSpace(model.Token))
        {
            _logger.LogError("Decompression failed: token is missing for model Id: {ModelId}", model.Id);
            throw new ArgumentException("Token is required.");
        }

        var tokenBytes = Base64UrlEncoder.DecodeBytes(model.Token);
        await dataPipe.Writer.WriteAsync(tokenBytes, cancellationToken);
        await dataPipe.Writer.CompleteAsync();

        await compression.DecompressAsync(dataPipe.Reader, outputData.Writer, cancellationToken);
        _logger.LogDebug("Decompression service finished decompressing data.");

        await using var ms = await ReadAllBytesFromPipeAsync(outputData.Reader, cancellationToken);
        _logger.LogDebug("Starting JSON deserialization from decompressed stream.");

        var token = await JsonSerializer.DeserializeAsync<T>(ms, cancellationToken: cancellationToken);
        _logger.LogInformation("Decompression and deserialization successful for model Id: {ModelId}", model.Id);

        return new ETAMPModel<T>
        {
            Id = model.Id,
            Version = model.Version,
            UpdateType = model.UpdateType,
            Token = token,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
    }

    private void CheckCompressionType(string compressionType)
    {
        if (string.IsNullOrWhiteSpace(compressionType)) throw new ArgumentException("Compression type is required.");
    }

    private async Task<MemoryStream> ReadAllBytesFromPipeAsync(PipeReader reader, CancellationToken cancellationToken)
    {
        var ms = new MemoryStream();

        while (true)
        {
            var result = await reader.ReadAsync(cancellationToken);
            var buffer = result.Buffer;

            foreach (var segment in buffer) ms.Write(segment.Span);

            reader.AdvanceTo(buffer.End);

            if (!result.IsCompleted)
                continue;

            _logger.LogDebug("Completed reading from output data pipe.");
            break;
        }

        ms.Position = 0;
        await reader.CompleteAsync();
        _logger.LogDebug("Compression complete, total compressed size: {Size} bytes", ms.Length);
        return ms;
    }
}