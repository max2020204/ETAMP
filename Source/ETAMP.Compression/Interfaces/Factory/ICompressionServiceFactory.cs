namespace ETAMP.Compression.Interfaces.Factory;

/// <summary>
/// Defines a factory interface for creating and retrieving instances of
/// compression services based on the specified compression type.
/// </summary>
public interface ICompressionServiceFactory
{
    /// <summary>
    ///     Retrieves an instance of <see cref="ICompressionService" /> based on the specified compression type.
    /// </summary>
    /// <param name="compressionType">The type of compression for which the corresponding service is required.</param>
    /// <returns>
    ///     An instance of <see cref="ICompressionService" /> that matches the specified compression type, or null if no
    ///     matching service is found.
    /// </returns>
    ICompressionService? Get(string compressionType);
}