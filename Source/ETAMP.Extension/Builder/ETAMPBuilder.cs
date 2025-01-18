#region

using System.Text.Json;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;

#endregion

namespace ETAMP.Extension.Builder;

/// <summary>
///     Provides utility methods to build and parse ETAMP (Enhanced Tokenized Application Message Protocol) models
///     with support for compression.
/// </summary>
public static class ETAMPBuilder
{
    /// <summary>
    ///     Builds a serialized representation of the provided ETAMPModel instance, compressing its Token property and
    ///     constructing a new ETAMPModelBuilder instance for serialization.
    /// </summary>
    /// <typeparam name="T">The type of the Token property, which must derive from the Token class.</typeparam>
    /// <param name="model">The ETAMPModel instance to be processed and serialized.</param>
    /// <param name="compressionServiceFactory">
    ///     The factory used to retrieve the appropriate compression service based on the
    ///     CompressionType property of the model.
    /// </param>
    /// <returns>A serialized string representation of the ETAMPModelBuilder instance, or null if the input is invalid.</returns>
    public static async Task<string> BuildAsync<T>(this ETAMPModel<T> model,
        ICompressionServiceFactory compressionServiceFactory)
        where T : Token
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(model.Token);
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(model.CompressionType);
        var compressionService = compressionServiceFactory.Create(model.CompressionType);
        var temp = new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            Token = await compressionService.CompressString(model.Token.ToJson())!,
            UpdateType = model.UpdateType,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
        return JsonSerializer.Serialize(temp);
    }

    /// <summary>
    ///     Deserializes a JSON representation of an ETAMP object into a structured ETAMPModel object,
    ///     decompressing the token data using the specified compression service.
    /// </summary>
    /// <typeparam name="T">The type of token contained in the ETAMPModel. Must derive from Token.</typeparam>
    /// <param name="jsonEtamp">The JSON string representation of the ETAMP object to be deconstructed.</param>
    /// <param name="compressionServiceFactory">
    ///     The factory responsible for creating the appropriate compression service for
    ///     decompression.
    /// </param>
    /// <returns>An instance of ETAMPModel containing the deserialized and decompressed data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the compressionServiceFactory is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the jsonEtamp is null, empty, or invalid JSON.</exception>
    public static async Task<ETAMPModel<T>> DeconstructETAMPAsync<T>(this string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory) where T : Token
    {
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonEtamp);

        if (!jsonEtamp.Contains('{') && !jsonEtamp.Contains('}'))
            throw new ArgumentException("This string is not JSON");

        var tempModel = JsonSerializer.Deserialize<ETAMPModelBuilder>(jsonEtamp);
        var compressionService = compressionServiceFactory.Create(tempModel.CompressionType);
        var token = await compressionService.DecompressString(tempModel.Token);

        return new ETAMPModel<T>
        {
            Id = tempModel.Id,
            Version = tempModel.Version,
            Token = JsonSerializer.Deserialize<T>(token),
            UpdateType = tempModel.UpdateType,
            CompressionType = tempModel.CompressionType,
            SignatureMessage = tempModel.SignatureMessage
        };
    }
}