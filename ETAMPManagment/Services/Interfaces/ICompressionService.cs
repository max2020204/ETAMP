namespace ETAMPManagment.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for compression services that can compress and decompress strings.
    /// </summary>
    public interface ICompressionService
    {
        /// <summary>
        /// Compresses the specified string data and encodes it to a format suitable for transmission or storage.
        /// </summary>
        /// <param name="data">The string data to be compressed.</param>
        /// <returns>A string representing the compressed data, encoded in a format suitable for transmission or storage.</returns>
        /// <remarks>
        /// Implementations of this method should focus on reducing the size of the input data string,
        /// potentially using various compression algorithms and then encoding the compressed byte array to a string.
        /// The encoded string format (e.g., Base64) should be chosen to ensure safe transmission or storage in text-based systems.
        /// </remarks>
        string CompressString(string data);

        /// <summary>
        /// Decompresses the given string, which was previously compressed and encoded, back to its original string form.
        /// </summary>
        /// <param name="base64CompressedData">The compressed and encoded string to be decompressed.</param>
        /// <returns>The original uncompressed string.</returns>
        /// <remarks>
        /// Implementations of this method are expected to reverse the compression process,
        /// decoding the input string to a byte array and then decompressing it to retrieve the original string data.
        /// This method should be compatible with the output of <see cref="CompressString"/>.
        /// </remarks>
        string DecompressString(string base64CompressedData);
    }
}