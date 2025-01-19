#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Provides functionality for managing ECDsa cryptographic operations within a secure storage context.
/// </summary>
/// <remarks>
///     ECDsaProvider is a concrete implementation of the ECDsaProviderBase class.
///     It utilizes an underlying store to retrieve ECDsa instances by unique identifier or name.
///     It ensures seamless integration into the system's cryptographic infrastructure.
/// </remarks>
public class ECDSAProvider : ECDSAProviderBase
{
    /// <summary>
    ///     Provides functionality for managing ECDsa (Elliptic Curve Digital Signature Algorithm) instances.
    ///     Extends the <see cref="ECDSAProviderBase" /> class to implement methods for retrieving ECDsa objects
    ///     by unique identifiers such as GUID or name.
    /// </summary>
    public ECDSAProvider(IECDSAStore store) : base(store)
    {
    }


    /// <summary>
    ///     Retrieves the ECDsa (elliptic curve digital signature algorithm) instance associated with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier used to locate the relevant ECDsa instance.</param>
    /// <returns>An <see cref="ECDsa" /> instance if found; otherwise, null.</returns>
    public override ECDsa? GetECDsa(Guid id)
    {
        var provider = Store.Get(id);
        return provider?.CurrentEcdsa;
    }

    /// <summary>
    ///     Retrieves an ECDsa instance based on a specified name from the provider store.
    /// </summary>
    /// <param name="name">The name of the ECDsa instance to retrieve from the store.</param>
    /// <returns>An ECDsa instance if found, or null if no matching provider is available.</returns>
    public override ECDsa? GetECDsa(string name)
    {
        var provider = Store.Get(name);
        return provider?.CurrentEcdsa;
    }
}