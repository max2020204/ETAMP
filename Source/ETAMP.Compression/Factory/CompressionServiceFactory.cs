using ETAMP.Compression.Codec;
using ETAMP.Compression.Interfaces.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Compression.Factory;

/// <summary>
/// Provides a factory implementation for creating and retrieving instances of
/// <see cref="StreamCompressionService"/> based on a specified compression type.
/// </summary>
/// <remarks>
/// This class is designed to work with the dependency injection container and leverages keyed service
/// resolution to retrieve the appropriate compression service.
/// </remarks>
public sealed record CompressionServiceFactory : ICompressionServiceFactory
{
    /// <summary>
    /// Represents the service provider used to resolve dependencies within the compression service factory.
    /// </summary>
    /// <remarks>
    /// This variable holds an instance of <see cref="IServiceProvider" />, which is used to dynamically retrieve
    /// keyed or non-keyed compression services based on the requested compression type.
    /// </remarks>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// A factory class to create and retrieve instances of StreamCompressionService based on the compression type.
    /// </summary>
    public CompressionServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Retrieves an instance of the StreamCompressionService based on the specified compression type.
    /// Returns null if the compression type is null, empty, or whitespace.
    /// </summary>
    /// <param name="compressionType">
    /// The type of compression for which the StreamCompressionService instance is required.
    /// </param>
    /// <returns>
    /// An instance of <see cref="StreamCompressionService"/> corresponding to the specified compression type,
    /// or null if the compression type is invalid.
    /// </returns>
    public StreamCompressionService? Get(string compressionType)
    {
        if (string.IsNullOrWhiteSpace(compressionType))
            return null;

        return _serviceProvider.GetKeyedService<StreamCompressionService>(compressionType);
    }
}