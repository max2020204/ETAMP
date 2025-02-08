using System.Collections.Concurrent;

namespace ETAMP.Compression.Interfaces.Factory;

/// <summary>
///     Defines a factory interface for creating instances of compression services based on a specified type.
/// </summary>
public interface ICompressionServiceFactory
{
    /// <summary>
    ///     Represents a factory for creating instances of compression services based on a specified type.
    /// </summary>
    ConcurrentDictionary<string, ICompressionService> Factory { get; }

    /// <summary>
    ///     Creates an instance of a compression service based on the specified compression type.
    /// </summary>
    /// <param name="compressionType">
    ///     The type of compression to create. This type should correspond to one of the supported
    ///     compression algorithms.
    /// </param>
    /// <returns>An instance of <see cref="ICompressionService" /> that corresponds to the specified compression type.</returns>
    ICompressionService Create(string compressionType);


    /// <summary>
    ///     Registers a new compression service under a specified compression type.
    /// </summary>
    /// <param name="compressionType">The type of compression to register.</param>
    /// <param name="serviceFactory">The compression service instance to be registered.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either <paramref name="compressionType" /> or
    ///     <paramref name="serviceFactory" /> is null.
    /// </exception>
    void RegisterCompressionService(string compressionType, ICompressionService serviceFactory);

    /// <summary>
    ///     Unregisters an existing compression service for a specified compression type.
    /// </summary>
    /// <param name="compressionType">The type of compression to unregister.</param>
    /// <returns>True if the service was successfully unregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="compressionType" /> is null.</exception>
    bool UnregisterCompressionService(string compressionType);
}