using System.IO.Compression;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
/// Provides GZip-based compression and decompression services by extending the functionality
/// of the <see cref="StreamCompressionService"/> class.
/// </summary>
/// <remarks>
/// This service uses <see cref="System.IO.Compression.GZipStream"/> for compressing and decompressing data streams.
/// It is designed to work with the ETAMP compression framework and implements the <see cref="ICompressionService"/> interface.
/// </remarks>
public sealed class GZipCompressionService : StreamCompressionService
{
    /// <summary>
    /// Provides GZip-based compression and decompression services for data streams.
    /// </summary>
    /// <remarks>
    /// This service uses the GZipStream class to implement the compression and decompression logic
    /// by creating streams with the specified compression mode.
    /// </remarks>
    public GZipCompressionService(ILogger<GZipCompressionService> logger)
        : base(logger)
    {
    }

    /// <summary>
    /// Creates a compression stream for handling data compression or decompression.
    /// </summary>
    /// <param name="output">The stream to which the compressed data will be written.</param>
    /// <param name="mode">The compression mode, either Compress or Decompress, indicating the operation.</param>
    /// <returns>A stream that performs compression or decompression based on the specified mode.</returns>
    protected override Stream CreateCompressionStream(Stream output, CompressionMode mode)
    {
        return new GZipStream(output, mode, true);
    }

    /// <summary>
    /// Creates a decompression stream for decompressing data using GZip compression.
    /// </summary>
    /// <param name="input">The input stream containing compressed data.</param>
    /// <param name="mode">
    /// The compression mode specifying whether the stream is used for compression or decompression.
    /// Should typically be set to <see cref="CompressionMode.Decompress"/>.
    /// </param>
    /// <returns>A <see cref="Stream"/> configured for decompressing data.</returns>
    protected override Stream CreateDecompressionStream(Stream input, CompressionMode mode)
    {
        return new GZipStream(input, mode, true);
    }
}