#region

using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Models;
using Newtonsoft.Json;

#endregion

namespace ETAMPManagement.Extensions;

/// <summary>
///     Provides compression and decompression utilities for ETAMPModel objects.
/// </summary>
public static class Compression
{
    /// <summary>
    ///     Compresses the provided ETAMPModel as a string using the specified compression service factory and compression
    ///     type.
    /// </summary>
    /// <typeparam name="T">The type of Token in the ETAMPModel.</typeparam>
    /// <param name="model">The ETAMPModel to compress.</param>
    /// <param name="compressionServiceFactory">The compression service factory to use.</param>
    /// <param name="compressionType">The compression type to use.</param>
    /// <returns>The compressed ETAMPModel as a string.</returns>
    public static string? Compress<T>(this ETAMPModel<T> model, ICompressionServiceFactory compressionServiceFactory,
        string compressionType) where T : Token
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(compressionType);
        var compressionService = compressionServiceFactory.Create(compressionType);
        return compressionService.CompressString(model.ToString());
    }

    /// <summary>
    ///     Decompresses the provided string as an ETAMPModel using the specified compression service factory and compression
    ///     type.
    /// </summary>
    /// <typeparam name="T">The type of Token in the ETAMPModel.</typeparam>
    /// <param name="jsonEtamp">The compressed ETAMPModel as a string.</param>
    /// <param name="compressionServiceFactory">The compression service factory to use.</param>
    /// <param name="compressionType">The compression type to use.</param>
    /// <returns>The decompressed ETAMPModel.</returns>
    public static ETAMPModel<T> Decompress<T>(this string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory,
        string compressionType) where T : Token
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

        return JsonConvert.DeserializeObject<ETAMPModel<T>>(decompressedString)
               ?? throw new InvalidOperationException("Decompressed string resulted in a null ETAMPModel.");
    }
}