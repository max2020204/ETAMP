using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Provides an implementation for managing an ECDsa instance.
/// </summary>
public class ECDsaProvider : IECDsaProvider, IECDsaRegistrar
{
    private ECDsa? _ecdsa;

    /// <summary>
    ///     Provides an implementation for managing an ECDsa instance.
    /// </summary>
    public ECDsaProvider()
    {
        _ecdsa = ECDsa.Create();
    }

    /// <summary>
    /// Provides an implementation for managing an ECDsa instance.
    /// </summary>
    public ECDsaProvider(ECDsa ecdsa) 
    {
        _ecdsa = ecdsa;
    }

    /// <summary>
    ///     Retrieves the registered ECDsa instance.
    /// </summary>
    /// <returns>The currently registered ECDsa instance.</returns>
    public ECDsa? GetECDsa()
    {
        return _ecdsa;
    }

    /// <summary>
    ///     Registers an ECDsa instance with the provider.
    /// </summary>
    /// <param name="ecdsa">The ECDsa instance to register.</param>
    /// <returns>The provider itself, allowing for method chaining.</returns>
    public IECDsaProvider RegisterECDsa(ECDsa ecdsa)
    {
        _ecdsa = ecdsa;
        return this;
    }
}