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
        var compression = _compressionServiceFactory.Create(model.CompressionType);

        try
        {
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(model.Token);
            await dataPipe.Writer.WriteAsync(jsonBytes, cancellationToken);
            await dataPipe.Writer.CompleteAsync();
        }
        catch (Exception ex)
        {
            await dataPipe.Writer.CompleteAsync(ex);
            throw;
        }

        await compression.CompressAsync(dataPipe.Reader, outputData.Writer, cancellationToken);

        var read = await outputData.Reader.ReadAsync(cancellationToken);
        outputData.Reader.AdvanceTo(read.Buffer.End);


        return new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            Token = Base64UrlEncoder.Encode(read.Buffer),
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