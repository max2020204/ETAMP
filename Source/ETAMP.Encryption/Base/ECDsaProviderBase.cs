#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.Base;

/// <summary>
///     Represents a base class for managing ECDsa instances. This class provides foundational
///     functionalities for storing and accessing ECDsa cryptographic providers using a designated storage mechanism.
/// </summary>
public abstract class ECDsaProviderBase : IECDsaProvider
{
    /// <summary>
    ///     Represents the storage mechanism for managing and retrieving
    ///     elliptic curve digital signature algorithm (ECDSA) provider instances.
    ///     Used as a dependency for ECDsaProviderBase.
    /// </summary>
    protected readonly IECDsaStore Store;

    /// Represents the base class for managing ECDsa instances.
    /// Provides common functionality for storing, retrieving, and setting ECDsa objects.
    protected ECDsaProviderBase(IECDsaStore store)
    {
        Store = store;
    }

    /// <summary>
    ///     Represents the currently active ECDsa instance managed by the provider.
    /// </summary>
    /// <remarks>
    ///     This property holds a reference to the active <see cref="ECDsa" /> instance
    ///     associated with the implementing provider. It can be set internally within
    ///     the class or by using the <see cref="SetECDsa(ECDsa)" /> method. The value
    ///     may be null if no ECDsa instance is currently assigned.
    /// </remarks>
    /// <value>
    ///     The current <see cref="ECDsa" /> instance being used by the provider,
    ///     or null if one has not been set.
    /// </value>
    public ECDsa? CurrentEcdsa { get; protected set; }

    /// <summary>
    ///     Retrieves the ECDsa instance associated with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the ECDsa instance to retrieve.</param>
    /// <returns>The ECDsa instance if found, otherwise null.</returns>
    public abstract ECDsa? GetECDsa(Guid id);

    /// <summary>
    ///     Retrieves an ECDsa instance based on the provided name.
    /// </summary>
    /// <param name="name">The name identifier used to locate the ECDsa instance.</param>
    /// <returns>The ECDsa instance associated with the specified name, or null if not found.</returns>
    public abstract ECDsa? GetECDsa(string name);

    /// <summary>
    ///     Sets the current ECDsa object.
    /// </summary>
    /// <param name="ecdsa">The ECDsa object to be set as the current instance.</param>
    public void SetECDsa(ECDsa ecdsa)
    {
        CurrentEcdsa = ecdsa;
    }
}