using ETAMP.Compression.Interfaces;
using ETAMP.Compression.Interfaces.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Compression.Factory;

/// <summary>
///     Represents a factory for creating and retrieving instances of compression services
///     based on a specified compression type.
/// </summary>
/// <remarks>
///     This class utilizes an <see cref="IServiceProvider" /> to resolve keyed services
///     implementing the <see cref="ICompressionService" /> interface for different compression types.
/// </remarks>
public sealed record CompressionServiceFactory : ICompressionServiceFactory
{
    /// <summary>
    ///     Represents an instance of <see cref="IServiceProvider" /> used to resolve and provide
    ///     dependencies for creating or retrieving compression services within the factory.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     A factory implementation responsible for creating and retrieving instances of
    ///     <see cref="ICompressionService" /> based on the specified compression type.
    /// </summary>
    public CompressionServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Retrieves an instance of <see cref="ICompressionService" /> based on the specified compression type.
    /// </summary>
    /// <param name="compressionType">The type of compression service to retrieve (e.g., Deflate, GZip).</param>
    /// <returns>
    ///     An instance of <see cref="ICompressionService" /> corresponding to the given compression type, or null if no
    ///     matching service is found.
    /// </returns>
    public ICompressionService? Get(string compressionType)
    {
        return _serviceProvider.GetKeyedService<ICompressionService>(compressionType);
    }
}