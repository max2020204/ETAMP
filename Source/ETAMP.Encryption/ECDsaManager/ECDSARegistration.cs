#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.ECDsaManager;

/// <summary>
///     Represents a class for handling the registration of EC-DSA keys and associated providers.
/// </summary>
/// <remarks>
///     ECDsaRegistration extends the functionality defined in ECDsaRegistrationBase to register
///     EC-DSA keys with a storage system and associate them with a provider.
/// </remarks>
public class ECDSARegistration : ECDSARegistrationBase
{
    /// <summary>
    ///     Represents the cryptographic provider used for managing Elliptic Curve Digital Signature Algorithm (ECDSA)
    ///     instances.
    ///     This variable holds the implementation of the provider, which facilitates ECDSA operations like associating
    ///     instances to identifiers or names.
    /// </summary>
    private readonly ECDSAProviderBase _provider;

    /// <summary>
    ///     Represents the ECDSA registration process, which acts as an extension
    ///     of the ECDsaRegistrationBase class for handling cryptographic components.
    ///     It provides methods for performing registration operations on ECDsa instances.
    /// </summary>
    public ECDSARegistration(IECDSAStore store, ECDSAProviderBase provider) : base(store)
    {
        _provider = provider;
    }

    /// <summary>
    ///     Registers an ECDsa instance and stores it after generating a unique identifier.
    /// </summary>
    /// <param name="ecdsa">An instance of ECDsa to be registered and stored.</param>
    /// <returns>
    ///     A tuple containing a unique Guid identifier and a boolean status indicating success or failure of storing the
    ///     ECDsa instance.
    /// </returns>
    public override (Guid Id, bool status) Registrar(ECDsa ecdsa)
    {
        _provider.SetECDsa(ecdsa);
        var guid = Guid.NewGuid();
        var status = Store.Add(guid, _provider);
        return (guid, status);
    }

    /// <summary>
    ///     Registers an ECDsa instance with the specified name in the ECDsa provider and store.
    /// </summary>
    /// <param name="ecdsa">The ECDsa instance to be registered.</param>
    /// <param name="name">The name or key with which the ECDsa instance will be associated.</param>
    /// <return>Returns true if the ECDsa instance is successfully registered; otherwise, false.</return>
    public override bool Registrar(ECDsa ecdsa, string name)
    {
        _provider.SetECDsa(ecdsa);
        return Store.Add(name, _provider);
    }
}