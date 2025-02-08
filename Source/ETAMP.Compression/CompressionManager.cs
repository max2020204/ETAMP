using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;

namespace ETAMP.Compression;

public class CompressionManager : ICompressionManager
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

        using var tokenBuffer = new MemoryStream();
        await outputData.Reader.CopyToAsync(tokenBuffer, cancellationToken);
        tokenBuffer.Position = 0;

        return new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            Token = Encoding.UTF8.GetString(tokenBuffer.GetBuffer(), 0, (int)tokenBuffer.Length),
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