using System.IO.Compression;
using Microsoft.Extensions.Logging;

namespace ETAMP.Compression.Codec;

/// <summary>
/// Provides data compression and decompression services using the Deflate algorithm.
/// </summary>
/// <remarks>
/// This class inherits from <see cref="StreamCompressionService"/> and implements the methods
/// required to create compression and decompression streams based on the `DeflateStream` class.
/// </remarks>
public sealed class DeflateCompressionService : StreamCompressionService
{
    /// <summary>
    /// A compression service implementing Deflate-based stream compression and decompression.
    /// </summary>
    /// <remarks>
    /// This class utilizes the DeflateStream implementation provided by .NET for compression and decompression of streams.
    /// It inherits from the <see cref="StreamCompressionService" />, which provides the core capabilities for handling
    /// input and output streams, and compressing or decompressing data asynchronously.
    /// </remarks>
    public DeflateCompressionService(ILogger<DeflateCompressionService> logger)
        : base(logger)
    {
    }


    /// <summary>
    /// Creates a compression stream using the Deflate algorithm.
    /// </summary>
    /// <param name="output">The output stream where compressed data will be written.</param>
    /// <param name="mode">The <see cref="CompressionMode"/> indicating whether to compress or decompress the data.</param>
    /// <returns>A <see cref="Stream"/> instance configured for compression or decompression based on the specified mode.</returns>
    protected override Stream CreateCompressionStream(Stream output, CompressionMode mode)
    {
        return new DeflateStream(output, mode, true);
    }

    /// <summary>
    /// Creates a decompression stream that uses the Deflate algorithm for decompressing data.
    /// </summary>
    /// <param name="input">The input stream to be decompressed.</param>
    /// <param name="mode">The compression mode specifying that the stream is to be used for decompression.</param>
    /// <returns>A decompression stream using the Deflate algorithm for the given input stream.</returns>
    protected override Stream CreateDecompressionStream(Stream input, CompressionMode mode)
    {
        return new DeflateStream(input, mode, true);
    }
}