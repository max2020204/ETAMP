using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.ECDsaManager.Interfaces
{
    /// <summary>
    /// Defines a contract for registering an ECDsa instance within a provider.
    /// This interface is intended for internal use to manage the lifecycle of ECDsa instances.
    /// </summary>
    internal interface IECDsaRegistrar
    {
        /// <summary>
        /// Registers an ECDsa instance with the provider and returns an associated provider instance.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance to be registered.</param>
        /// <returns>A provider that manages the registered ECDsa instance.</returns>
        IECDsaProvider RegisterEcdsa(ECDsa ecdsa);
    }
}