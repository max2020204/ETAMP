#region

using System.Security.Cryptography;
using ETAMP.Encryption.Interfaces.ECDSAManager;

#endregion

namespace ETAMP.Encryption.Base;

/// <summary>
///     Represents an abstract base class for registering ECDsa cryptographic objects.
/// </summary>
/// <remarks>
///     This class is designed to provide a foundational implementation for handling
///     registration of ECDsa objects and parameterized configurations.
///     It includes methods to register ECDsa instances with optional naming support.
/// </remarks>
public abstract class ECDSARegistrationBase : IECDSARegistrar
{
    /// <summary>
    ///     Represents the base implementation for an ECDSA (Elliptic Curve Digital Signature Algorithm) registrar.
    ///     This abstract class defines a common structure for registering ECDSA instances in a backing store.
    /// </summary>
    protected ECDSARegistrationBase(IECDSAStore store)
    {
        Store = store;
    }

    /// <summary>
    ///     Gets the instance of the store that provides functionality to manage and
    ///     interact with cryptographic providers associated with ECDSA registrations.
    /// </summary>
    protected IECDSAStore Store { get; }

    /// <summary>
    ///     Registers an ECDsa instance with an associated store by assigning it a unique identifier
    ///     and returns the registration status.
    ///     <param name="ecdsa">The ECDsa instance to be registered.</param>
    ///     <returns>
    ///         A tuple containing the unique identifier (Guid) for the registered ECDsa and a bool indicating the status
    ///         of the registration.
    ///     </returns>
    /// </summary>
    public abstract (Guid Id, bool status) Registrar(ECDsa ecdsa);

    /// <summary>
    ///     Registers an ECDsa instance and associates it with a specified name.
    /// </summary>
    /// <param name="ecdsa">The ECDsa instance to register.</param>
    /// <param name="name">The name to associate with the ECDsa instance.</param>
    /// <returns>Returns a boolean value indicating whether the registration was successful.</returns>
    public abstract bool Registrar(ECDsa ecdsa, string name);
}