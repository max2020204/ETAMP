using System.IO.Compression;
using System.Text;
using ETAMPManagment.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Services;

/// <summary>
///     Provides methods for compressing and decompressing strings using GZip algorithm and Base64 URL encoding.
/// </summary>
public class GZipCompressionService : ICompressionService
{
    /// <summary>
    ///     Compresses the specified string using GZip compression and then encodes the compressed bytes to a Base64
    ///     URL-encoded string.
    /// </summary>
    /// <param name="data">The string data to compress.</param>
    /// <returns>A Base64 URL-encoded string representing the compressed input data.</returns>
    /// <remarks>
    ///     This method compresses the input string data using the GZip algorithm, which is useful for reducing the size of the
    ///     data
    ///     and making it safe for transmission over URL-friendly environments. The compressed data is then encoded to a Base64
    ///     URL-encoded string
    ///     to ensure safe transmission and storage in environments that support only text data.
    /// </remarks>
    public virtual string CompressString(string data)
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
    ///     Decompresses a Base64 URL-encoded string that was compressed using the GZip algorithm.
    /// </summary>
    /// <param name="base64CompressedData">The Base64 URL-encoded string to decompress.</param>
    /// <returns>The original uncompressed string.</returns>
    /// <remarks>
    ///     This method reverses the process done by <see cref="CompressString" />, decoding the Base64 URL-encoded string
    ///     and then decompressing the data using the GZip algorithm. It restores the original string data,
    ///     making it suitable for use where the original data needs to be recovered from a compressed format.
    /// </remarks>
    public virtual string DecompressString(string base64CompressedData)
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