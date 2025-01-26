#region

using System.Text;
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
    /// Asynchronously builds a serialized JSON representation of the ETAMP model
    /// with the token compressed using the specified compression service factory.
    /// </summary>
    /// <typeparam name="T">The type of the token, which must derive from the <see cref="Token"/> class.</typeparam>
    /// <param name="model">The ETAMP model containing data to be processed and serialized.</param>
    /// <param name="compressionServiceFactory">The factory used to create the compression service for the specified compression type.</param>
    /// <param name="cancellationToken">Optional cancellation token for asynchronous task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the serialized and compressed representation of the ETAMP model.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="model"/>, <paramref name="model.Token"/>, or <paramref name="compressionServiceFactory"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <see cref="ETAMPModel{T}.CompressionType"/> is null or whitespace.
    /// </exception>
    public static async Task<string> BuildAsync<T>(this ETAMPModel<T> model,
        ICompressionServiceFactory compressionServiceFactory, CancellationToken cancellationToken = default)
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
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, temp, cancellationToken: cancellationToken);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }


    /// <summary>
    /// Deconstructs an ETAMP JSON string into an <see cref="ETAMPModel{T}"/> object.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the token contained within the ETAMP model. Must inherit from <see cref="Token"/>.
    /// </typeparam>
    /// <param name="jsonEtamp">
    /// The JSON string representation of the ETAMP model to be deconstructed. It cannot be null, empty, or whitespace.
    /// </param>
    /// <param name="compressionServiceFactory">
    /// The compression service factory used to create the necessary decompression service.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the deconstructed <see cref="ETAMPModel{T}"/> object.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="compressionServiceFactory"/> parameter is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="jsonEtamp"/> parameter is null, empty, consists solely of whitespace, or is not valid JSON.
    /// </exception>
    public static async Task<ETAMPModel<T>> DeconstructETAMPAsync<T>(this string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory, CancellationToken cancellationToken = default)
        where T : Token
    {
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonEtamp);

        if (!jsonEtamp.Contains('{') && !jsonEtamp.Contains('}'))
            throw new ArgumentException("This string is not JSON");

        var tempModel = JsonSerializer.Deserialize<ETAMPModelBuilder>(jsonEtamp);
        var compressionService = compressionServiceFactory.Create(tempModel.CompressionType);
        var token = await compressionService.DecompressString(tempModel.Token, cancellationToken);

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