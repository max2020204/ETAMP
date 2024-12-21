using ETAMP.Encryption.Interfaces;
using ETAMP.Encryption.Interfaces.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Encryption.Factory;

/// <summary>
///     Factory class for creating instances of <see cref="KeyPairProvider" />.
///     This factory utilizes the ActivatorUtilities class to create new instances of KeyPairProvider,
///     allowing for dependency injection in the created instances.
/// </summary>
public sealed class KeyPairProviderFactory : IKeyPairProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="KeyPairProviderFactory" /> class with the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use for creating instances of KeyPairProvider.</param>
    public KeyPairProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider
                           ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    ///     Creates a new instance of <see cref="KeyPairProvider" /> using the registered services in the service provider.
    /// </summary>
    /// <returns>A new instance of <see cref="KeyPairProvider" />.</returns>
    public IKeyPairProvider CreateInstance()
    {
        return ActivatorUtilities.CreateInstance<KeyPairProvider>(_serviceProvider);
    }
}