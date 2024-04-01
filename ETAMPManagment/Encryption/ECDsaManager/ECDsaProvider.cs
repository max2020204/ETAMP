using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption.ECDsaManager
{
    /// <summary>
    /// Provides an implementation for managing an ECDsa instance.
    /// </summary>
    /// <remarks>
    /// This class serves as a provider for ECDsa instances, allowing for registration and retrieval of ECDsa objects.
    /// It implements both the provider and registrar interfaces to encapsulate the lifecycle management of ECDsa instances.
    /// </remarks>
    public class ECDsaProvider : IECDsaProvider, IECDsaRegistrar
    {
        private ECDsa _ecdsa;

        /// <summary>
        /// Retrieves the registered ECDsa instance.
        /// </summary>
        /// <returns>The currently registered ECDsa instance.</returns>
        public ECDsa GetECDsa()
        {
            return _ecdsa;
        }

        /// <summary>
        /// Registers an ECDsa instance with the provider.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance to register.</param>
        /// <returns>The provider itself, allowing for method chaining.</returns>
        public IECDsaProvider RegisterEcdsa(ECDsa ecdsa)
        {
            _ecdsa = ecdsa;
            return this;
        }
    }
}