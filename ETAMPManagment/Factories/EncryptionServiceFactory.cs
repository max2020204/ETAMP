using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Factories.Interfaces;

namespace ETAMPManagment.Factories
{
    /// <summary>
    /// Factory for creating encryption services based on specified names.
    /// This factory maintains a registry of encryption services which can be dynamically registered and created,
    /// allowing for flexible encryption service management and instantiation based on runtime requirements.
    /// </summary>
    public class EncryptionServiceFactory : IEncryptionServiceFactory
    {
        /// <summary>
        /// Gets the dictionary of registered encryption services.
        /// Each service is identified by a string name and associated with a function that creates an instance of the service.
        /// </summary>
        public Dictionary<string, Func<IEncryptionService>> Services { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionServiceFactory"/> class.
        /// </summary>
        public EncryptionServiceFactory() => Services = new Dictionary<string, Func<IEncryptionService>>();

        /// <summary>
        /// Registers an encryption service in the factory.
        /// </summary>
        /// <param name="name">The name of the encryption service to be registered.</param>
        /// <param name="serviceCreator">The function used to create an instance of the encryption service.</param>
        /// <exception cref="ArgumentException">Thrown when a service with the same name is already registered.</exception>
        public virtual void RegisterEncryptionService(string name, Func<IEncryptionService> serviceCreator)
        {
            if (Services.ContainsKey(name))
                throw new ArgumentException($"An encryption service with the name '{name}' is already registered.", nameof(name));

            Services[name] = serviceCreator;
        }

        /// <summary>
        /// Creates an instance of the encryption service based on the provided name.
        /// </summary>
        /// <param name="name">The name of the encryption service to create.</param>
        /// <returns>An instance of the encryption service.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported encryption service name is provided.</exception>
        public virtual IEncryptionService CreateEncryptionService(string name)
        {
            if (Services.TryGetValue(name, out var serviceCreator))
                return serviceCreator();

            throw new ArgumentException("Unsupported encryption service: " + name);
        }

        /// <summary>
        /// Unregisters an encryption service from the factory.
        /// </summary>
        /// <param name="name">The name of the encryption service to be unregistered.</param>
        /// <returns><c>true</c> if the service was successfully unregistered; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method removes the specified encryption service from the registry.
        /// If the service is not found, the method returns <c>false</c> and does not throw an exception.
        /// This approach allows for safe unregistration without the need for prior checks or exception handling
        /// by the caller.
        /// </remarks>
        public virtual bool UnregisterEncryptionService(string name)
        {
            return Services.Remove(name);
        }
    }
}