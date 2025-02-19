using System.IO.Pipelines;
using System.Text.Json;
using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression;

/// <summary>
///     The CompressionManager class provides functionality for compressing and decompressing models using specified
///     compression algorithms. It acts as a manager for handling compression operations and coordinates between
///     compression services and the model data.
/// </summary>
public record CompressionManager : ICompressionManager
{
    /// <summary>
    ///     A private readonly instance of <see cref="ICompressionServiceFactory" /> used to create compression service
    ///     instances based on the required compression type.
    /// </summary>
    private readonly ICompressionServiceFactory _compressionServiceFactory;

    /// <summary>
    ///     Logger used to log messages and information related to compression and decompression processes
    ///     within the <see cref="CompressionManager" />. This instance of the logger provides structured
    ///     logging capabilities for debugging, monitoring, and tracking the behavior of the compression
    ///     manager during operations.
    /// </summary>
    private readonly ILogger<ICompressionManager> _logger;

    /// <summary>
    ///     The CompressionManager class provides functionality for compressing and decompressing models
    ///     using specified compression algorithms. It serves as the primary manager for handling the
    ///     coordination between compression operations, models, and associated services.
    /// </summary>
    public CompressionManager(ICompressionServiceFactory compressionServiceFactory, ILogger<ICompressionManager> logger)
    {
        _compressionServiceFactory = compressionServiceFactory;
        _logger = logger;
    }

    /// Compresses the data contained in the specified ETAMP model using the defined compression type
    /// and returns a builder object containing the compressed data and other model properties.
    /// <param name="model">
    ///     An instance of ETAMPModel containing the data and associated properties to compress.
    /// </param>
    /// <param name="cancellationToken">
    ///     A CancellationToken to observe while waiting for the task to complete.
    ///     Defaults to CancellationToken.None if not provided.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the Token contained within the provided ETAMPModel. Must inherit from the Token class.
    /// </typeparam>
    /// <returns>
    ///     An ETAMPModelBuilder containing the compressed data, model properties, and metadata.
    /// </returns>
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


    /// <summary>
    ///     Decompresses the compressed data from the given <see cref="ETAMPModelBuilder" /> and deserializes it into the
    ///     specified token type.
    /// </summary>
    /// <typeparam name="T">The type of the token to deserialize into. Must inherit from <see cref="Token" />.</typeparam>
    /// <param name="model">The builder model containing the data to be decompressed and deserialized.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An instance of <see cref="ETAMPModel{T}" /> containing the decompressed and deserialized data as the specified
    ///     token type.
    /// </returns>
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
            throw new ArgumentException($"Decompression failed: token is missing for model Id: {model.Id}");
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

    /// <summary>
    ///     Validates the compression type provided as input.
    /// </summary>
    /// <param name="compressionType">
    ///     The compression type to validate. Must not be null, empty, or whitespace.
    ///     Throws an <see cref="ArgumentException" /> if the input is invalid.
    /// </param>
    private void CheckCompressionType(string compressionType)
    {
        if (string.IsNullOrWhiteSpace(compressionType))
            throw new ArgumentException("Compression type is required.");
    }

    /// Reads all bytes from a specified pipe and returns them in a memory stream.
    /// <param name="reader">
    ///     The <see cref="PipeReader" /> instance from which data will be read.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="MemoryStream" /> containing the bytes read from the pipe.
    /// </returns>
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