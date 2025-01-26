#region

using System.Text;
using System.Text.Json;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;

#endregion

namespace ETAMP.Extension.Builder;

/// <summary>
///     Provides utility methods to build and parse ETAMP (Enhanced Tokenized Application Message Protocol) models
///     with support for compression.
/// </summary>
/// <summary>
/// Provides utility methods to build and parse ETAMP (Enhanced Tokenized Application Message Protocol) models
/// with support for compression.
/// </summary>
public static class ETAMPBuilder
{
    /// <summary>
    /// Asynchronously builds a serialized JSON representation of the ETAMP model
    /// with the token compressed using the specified compression service factory.
    /// </summary>
    public static async Task<string> BuildAsync<T>(
        this ETAMPModel<T> model,
        ICompressionServiceFactory compressionServiceFactory,
        CancellationToken cancellationToken = default
    ) where T : Token
    {
        ValidateBuildInputs(model, compressionServiceFactory);

        var compressionService = compressionServiceFactory.Create(model.CompressionType);

        await using var compressedStream = await compressionService.CompressStream(
            await model.Token.ToJsonStreamAsync(cancellationToken),
            cancellationToken
        );

        // Prepare the ETAMP model for serialization
        var tempModel = await CreateModelBuilder(model, compressedStream, cancellationToken);
        return await tempModel.ToJsonAsync();
    }

    /// <summary>
    /// Deconstructs an ETAMP JSON string into an <see cref="ETAMPModel{T}"/> object.
    /// </summary>
    public static async Task<ETAMPModel<T>> DeconstructETAMPAsync<T>(
        this string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory,
        CancellationToken cancellationToken = default
    ) where T : Token
    {
        ValidateDeconstructInputs(jsonEtamp, compressionServiceFactory);

        var tempModel = await DeserializeJsonAsync<ETAMPModelBuilder>(jsonEtamp, cancellationToken);

        var compressionService = compressionServiceFactory.Create(tempModel.CompressionType);

        // Decompress the token
        await using var tokenStream = new MemoryStream(Encoding.UTF8.GetBytes(tempModel.Token));
        await using var decompressedStream = await compressionService.DecompressStream(tokenStream, cancellationToken);

        var token = await DeserializeJsonAsync<T>(decompressedStream, cancellationToken);

        return ReconstructETAMPModel(tempModel, token);
    }

    /// <summary>
    /// Converts a stream to a Base64 string asynchronously.
    /// </summary>
    private static async Task<string> EncodeStreamToBase64Async(Stream stream, CancellationToken cancellationToken)
    {
        stream.Position = 0;
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        return Base64UrlEncoder.Encode(memoryStream.ToArray());
    }

    /// <summary>
    /// Creates an ETAMP model builder based on the provided inputs.
    /// </summary>
    private static async Task<ETAMPModelBuilder> CreateModelBuilder<T>(
        ETAMPModel<T> model,
        Stream compressedStream,
        CancellationToken cancellationToken
    ) where T : Token
    {
        return new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            Token = await EncodeStreamToBase64Async(compressedStream, cancellationToken),
            UpdateType = model.UpdateType,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
    }

    /// <summary>
    /// Reconstructs an ETAMP model from its builder and token.
    /// </summary>
    private static ETAMPModel<T> ReconstructETAMPModel<T>(
        ETAMPModelBuilder builder,
        T token
    ) where T : Token
    {
        return new ETAMPModel<T>
        {
            Id = builder.Id,
            Version = builder.Version,
            Token = token,
            UpdateType = builder.UpdateType,
            CompressionType = builder.CompressionType,
            SignatureMessage = builder.SignatureMessage
        };
    }

    /// <summary>
    /// Validates the inputs for the BuildAsync method.
    /// </summary>
    private static void ValidateBuildInputs<T>(
        ETAMPModel<T> model,
        ICompressionServiceFactory compressionServiceFactory
    ) where T : Token
    {
        ArgumentNullException.ThrowIfNull(model.Token, nameof(model.Token));
        ArgumentNullException.ThrowIfNull(compressionServiceFactory, nameof(compressionServiceFactory));
        ArgumentException.ThrowIfNullOrWhiteSpace(model.CompressionType, nameof(model.CompressionType));
    }

    /// <summary>
    /// Validates the inputs for the DeconstructETAMPAsync method.
    /// </summary>
    private static void ValidateDeconstructInputs(
        string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory
    )
    {
        ArgumentNullException.ThrowIfNull(compressionServiceFactory, nameof(compressionServiceFactory));
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonEtamp, nameof(jsonEtamp));

        if (!jsonEtamp!.Contains('{') || !jsonEtamp.Contains('}'))
        {
            throw new ArgumentException("The provided string is not valid JSON.", nameof(jsonEtamp));
        }
    }

    /// <summary>
    /// Deserializes a JSON string or stream into an object.
    /// </summary>
    private static async Task<T> DeserializeJsonAsync<T>(Stream stream, CancellationToken cancellationToken)
    {
        stream.Position = 0;
        return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException($"Failed to deserialize JSON into {typeof(T).Name}.");
    }

    private static async Task<T> DeserializeJsonAsync<T>(string json, CancellationToken cancellationToken)
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        return await DeserializeJsonAsync<T>(stream, cancellationToken);
    }
}