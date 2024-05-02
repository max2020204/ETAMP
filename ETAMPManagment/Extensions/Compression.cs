#region

using ETAMPManagment.Factory.Interfaces;
using ETAMPManagment.Models;
using Newtonsoft.Json;

#endregion

namespace ETAMPManagment.Extensions;

/// <summary>
///     Provides compression and decompression utilities for ETAMPModel objects.
/// </summary>
public static class Compression
{
    /// <summary>
    ///     Compresses the ETAMPModel instance into a compressed string format.
    /// </summary>
    /// <param name="model">The ETAMPModel instance to be compressed.</param>
    /// <param name="compressionServiceFactory">A factory method that creates an instance of ICompressionService.</param>
    /// <param name="compressionType">The type of compression to be used.</param>
    /// <returns>A compressed string representation of the ETAMPModel.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the model, compressionServiceFactory, or compressionType is null.</exception>
    /// <exception cref="ArgumentException">Thrown if compressionType is whitespace.</exception>
    public static string Compress(this ETAMPModel model, ICompressionServiceFactory compressionServiceFactory,
        string compressionType)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(compressionType);
        var compressionService = compressionServiceFactory.Create(compressionType);
        return compressionService.CompressString(model.ToString());
    }

    /// <summary>
    ///     Decompresses a compressed string into an ETAMPModel instance.
    /// </summary>
    /// <param name="jsonEtamp">The compressed string representation of the ETAMPModel.</param>
    /// <param name="compressionServiceFactory">A factory method that creates an instance of ICompressionService.</param>
    /// <param name="compressionType">The type of compression used for the string.</param>
    /// <returns>The decompressed ETAMPModel instance.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if jsonEtamp, compressionServiceFactory, or compressionType is null or
    ///     empty.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown if jsonEtamp or compressionType is whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if decompression fails.</exception>
    public static ETAMPModel Decompress(this string jsonEtamp, ICompressionServiceFactory compressionServiceFactory,
        string compressionType)
    {
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonEtamp);
        ArgumentException.ThrowIfNullOrWhiteSpace(compressionType);

        var compressionService = compressionServiceFactory.Create(compressionType);
        string decompressedString;
        try
        {
            decompressedString = compressionService.DecompressString(jsonEtamp);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Decompression of the ETAMP string failed.", ex);
        }

        try
        {
            return JsonConvert.DeserializeObject<ETAMPModel>(decompressedString)
                   ?? throw new InvalidOperationException("Decompressed string resulted in a null ETAMPModel.");
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("The decompressed string is not in a valid JSON format.", jsonEtamp, ex);
        }
    }
}