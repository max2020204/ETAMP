using ETAMPManagment.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;

namespace ETAMPManagment.Services
{
    /// <summary>
    /// Provides methods for compressing and decompressing strings using Deflate algorithm and Base64 URL encoding.
    /// </summary>
    public class DeflateCompressionService : ICompressionService
    {
        /// <summary>
        /// Compresses the given string using Deflate compression and then encodes the compressed bytes to a Base64 URL-encoded string.
        /// </summary>
        /// <param name="data">The string data to compress.</param>
        /// <returns>A Base64 URL-encoded string representing the compressed input data.</returns>
        /// <remarks>
        /// This method is useful for reducing the size of string data and making it safe for transmission over URL-friendly environments.
        /// </remarks>
        public virtual string CompressString(string data)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(data);

            using var output = new MemoryStream();
            using (DeflateStream compressor = new(output, CompressionMode.Compress))
            {
                compressor.Write(inputBytes, 0, inputBytes.Length);
            }

            return Base64UrlEncoder.Encode(output.ToArray());
        }

        /// <summary>
        /// Decompresses a Base64 URL-encoded string that was compressed using the Deflate algorithm.
        /// </summary>
        /// <param name="base64CompressedData">The Base64 URL-encoded string to decompress.</param>
        /// <returns>The original uncompressed string.</returns>
        /// <remarks>
        /// This method reverses the compression and encoding done by <see cref="CompressString"/>, restoring the original string data.
        /// </remarks>
        public virtual string DecompressString(string base64CompressedData)
        {
            byte[] inputBytes = Base64UrlEncoder.DecodeBytes(base64CompressedData);

            using var inputStream = new MemoryStream(inputBytes);
            using var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress);
            using var outputStream = new MemoryStream();
            decompressor.CopyTo(outputStream);
            byte[] outputBytes = outputStream.ToArray();

            return Encoding.UTF8.GetString(outputBytes);
        }
    }
}