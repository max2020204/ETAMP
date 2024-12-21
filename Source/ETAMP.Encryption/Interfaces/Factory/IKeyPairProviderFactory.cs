namespace ETAMP.Encryption.Interfaces.Factory;

/// <summary>
///     Defines a factory interface for creating instances of <see cref="IKeyPairProvider" />.
///     This interface allows for abstracting the creation process of key pair providers,
///     enabling dependency injection and creation of different implementations as needed.
/// </summary>
public interface IKeyPairProviderFactory
{
    /// <summary>
    ///     Creates and returns a new instance of an object that implements <see cref="IKeyPairProvider" />.
    /// </summary>
    /// <returns>A new instance of an object that implements <see cref="IKeyPairProvider" />.</returns>
    IKeyPairProvider CreateInstance();
}