using ETAMPManagment.Encryption.Interfaces;

namespace ETAMPManagment.Factories.Interfaces
{
    /// <summary>
    /// Defines a factory for managing and creating instances of encryption services.
    /// </summary>
    public interface IEncryptionServiceFactory
    {
        /// <summary>
        /// Gets a dictionary mapping service names to their respective factory methods.
        /// </summary>
        Dictionary<string, Func<IEncryptionService>> Services { get; }

        /// <summary>
        /// Registers an encryption service with the factory.
        /// </summary>
        /// <param name="name">The name of the encryption service to register.</param>
        /// <param name="serviceCreator">A function that creates an instance of the encryption service.</param>
        void RegisterEncryptionService(string name, Func<IEncryptionService> serviceCreator);

        /// <summary>
        /// Creates an instance of the specified encryption service.
        /// </summary>
        /// <param name="name">The name of the encryption service to create.</param>
        /// <returns>An instance of the specified encryption service.</returns>
        IEncryptionService CreateEncryptionService(string name);

        /// <summary>
        /// Unregisters an encryption service from the factory.
        /// </summary>
        /// <param name="name">The name of the encryption service to unregister.</param>
        /// <returns><c>true</c> if the service was successfully unregistered; otherwise, <c>false</c>.</returns>
        bool UnregisterEncryptionService(string name);
    }
}