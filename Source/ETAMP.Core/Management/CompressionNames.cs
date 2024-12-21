namespace ETAMP.Core.Management;

/// <summary>
///     Provides constants for identifying the compression algorithms used within the application.
///     These constants allow for consistent naming and usage of compression services throughout the application.
/// </summary>
public static class CompressionNames
{
    /// <summary>
    ///     Represents the Deflate compression algorithm.
    ///     This constant is used to identify and select the Deflate compression service.
    /// </summary>
    public const string Deflate = "Deflate";

    /// <summary>
    ///     Represents the GZip compression algorithm.
    ///     This constant is used to identify and select the GZip compression service.
    /// </summary>
    public const string GZip = "GZip";
}