#region

using System.IO.Compression;
using System.Text;
using ETAMP.Compression.Interfaces;
using ETAMP.Core.Utils;

#endregion

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides compression and decompression functionality using the GZip algorithm.
/// </summary>
public sealed class GZipCompressionService : ICompressionService
{
    /// <summary>
    ///     Compresses and encodes a string for efficient storage or transmission.
    /// </summary>
    /// <param name="data">The string data to compress.</param>
    /// <returns>The compressed and encoded string.</returns>
    public string? CompressString(string? data)
    {
        ArgumentException.ThrowIfNullOrEmpty(data);
        var bytes = Encoding.UTF8.GetBytes(data);

        using var outputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress, true))
        {
            gzipStream.Write(bytes, 0, bytes.Length);
        }

        outputStream.Position = 0;

        return Base64UrlEncoder.Encode(outputStream.ToArray());
    }

    /// <summary>
    ///     Decompresses and decodes a string back to its original form.
    /// </summary>
    /// <param name="base64CompressedData">The compressed and encoded string to decompress.</param>
    /// <returns>The original uncompressed string.</returns>
    public string DecompressString(string? base64CompressedData)
    {
        ArgumentException.ThrowIfNullOrEmpty(base64CompressedData);
        var compressedBytes = Base64UrlEncoder.DecodeBytes(base64CompressedData);

        using var inputStream = new MemoryStream(compressedBytes);
        using var outputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
        {
            gzipStream.CopyTo(outputStream);
        }

        outputStream.Position = 0;
        var decompressedBytes = outputStream.ToArray();

        return Encoding.UTF8.GetString(decompressedBytes);
    }
}