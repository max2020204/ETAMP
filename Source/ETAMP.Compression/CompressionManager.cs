using System.IO.Pipelines;
using System.Text.Json;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;

namespace ETAMP.Compression;

public record CompressionManager : ICompressionManager
{
    private readonly ICompressionServiceFactory _compressionServiceFactory;

    public CompressionManager(ICompressionServiceFactory compressionServiceFactory)
    {
        _compressionServiceFactory = compressionServiceFactory;
    }

    public async Task<ETAMPModelBuilder> CompressAsync<T>(ETAMPModel<T> model,
        CancellationToken cancellationToken = default) where T : Token
    {
        Pipe dataPipe = new();
        Pipe outputData = new();

        if (string.IsNullOrWhiteSpace(model.CompressionType))
            throw new ArgumentException("Compression type is required.");

        var compression = _compressionServiceFactory.Create(model.CompressionType);

        try
        {
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(model.Token);
            await dataPipe.Writer.WriteAsync(jsonBytes, cancellationToken);
            await dataPipe.Writer.FlushAsync(cancellationToken);
            await dataPipe.Writer.CompleteAsync();
        }
        catch (Exception ex)
        {
            await dataPipe.Writer.CompleteAsync(ex);
            throw new InvalidOperationException("Failed to serialize token.", ex);
        }

        await compression.CompressAsync(dataPipe.Reader, outputData.Writer, cancellationToken);

        var result = await outputData.Reader.ReadAsync(cancellationToken);

        var data = result.Buffer.FirstSpan;
        var token = Base64UrlEncoder.Encode(data);

        return new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            Token = token,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
    }


    public Task<ETAMPModel<T>> DecompressAsync<T>(ETAMPModelBuilder model,
        CancellationToken cancellationToken = default) where T : Token
    {
        throw new NotImplementedException();
    }
}