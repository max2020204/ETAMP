namespace ETAMP.Compression.Interfaces;

/// <summary>
///     Defines a contract for services that can compress and decompress strings.
/// </summary>
public interface ICompressionService
{
    /// <summary>
    ///     Compresses and encodes a string for efficient storage or transmission.
    /// </summary>
    /// <param name="data">The string data to compress.</param>
    /// <returns>The compressed and encoded string.</returns>
    string? CompressString(string? data);

    /// <summary>
    ///     Decompresses and decodes a string back to its original form.
    /// </summary>
    /// <param name="base64CompressedData">The compressed and encoded string to decompress.</param>
    /// <returns>The original uncompressed string.</returns>
    string DecompressString(string? base64CompressedData);
}