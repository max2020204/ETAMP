using System.IO.Compression;
using System.Text;
using ETAMP.Compression.Interfaces;
using ETAMP.Core;
using ETAMP.Core.Utils;

namespace ETAMP.Compression.Codec;

/// <summary>
///     Provides methods for compressing and decompressing strings using Deflate algorithm and Base64 URL encoding.
/// </summary>
public sealed class DeflateCompressionService : ICompressionService
{
    /// <summary>
    ///     Compresses the given string using Deflate compression and then encodes the compressed bytes to a Base64 URL-encoded
    ///     string.
    /// </summary>
    /// <param name="data">The string data to compress.</param>
    /// <returns>A Base64 URL-encoded string representing the compressed input data.</returns>
    /// <remarks>
    ///     This method is useful for reducing the size of string data and making it safe for transmission over URL-friendly
    ///     environments.
    /// </remarks>
    public string? CompressString(string? data)
    {
        ArgumentException.ThrowIfNullOrEmpty(data);
        var inputBytes = Encoding.UTF8.GetBytes(data);

        using var output = new MemoryStream();
        using (var compressor = new DeflateStream(output, CompressionMode.Compress, true))
        {
            compressor.Write(inputBytes, 0, inputBytes.Length);
        }

        output.Position = 0;
        return Base64UrlEncoder.Encode(output.ToArray());
    }

    /// <summary>
    ///     Decompresses a Base64 URL-encoded string that was compressed using the Deflate algorithm.
    /// </summary>
    /// <param name="base64CompressedData">The Base64 URL-encoded string to decompress.</param>
    /// <returns>The original uncompressed string.</returns>
    /// <remarks>
    ///     This method reverses the compression and encoding done by <see cref="CompressString" />, restoring the original
    ///     string data.
    /// </remarks>
    public string DecompressString(string? base64CompressedData)
    {
        ArgumentException.ThrowIfNullOrEmpty(base64CompressedData);
        var inputBytes = Base64UrlEncoder.DecodeBytes(base64CompressedData);

        using var inputStream = new MemoryStream(inputBytes);
        using var outputStream = new MemoryStream();
        using (var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress))
        {
            decompressor.CopyTo(outputStream);
        }

        outputStream.Position = 0;
        var outputBytes = outputStream.ToArray();

        return Encoding.UTF8.GetString(outputBytes);
    }
}