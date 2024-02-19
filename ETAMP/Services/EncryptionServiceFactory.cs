using ETAMP.Services.Interfaces;

namespace ETAMP.Services
{
    /// <summary>
    /// Factory for creating encryption services based on specified names.
    /// This factory maintains a registry of encryption services which can be dynamically registered and created.
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
        public EncryptionServiceFactory()
        {
            Services = new Dictionary<string, Func<IEncryptionService>>();
        }

        /// <summary>
        /// Registers an encryption service in the factory.
        /// </summary>
        /// <param name="name">The name of the encryption service to be registered.</param>
        /// <param name="serviceCreator">The function used to create an instance of the encryption service.</param>
        public void RegisterEncryptionService(string name, Func<IEncryptionService> serviceCreator)
        {
            Services[name] = serviceCreator;
        }

        /// <summary>
        /// Creates an instance of the encryption service based on the provided name.
        /// </summary>
        /// <param name="name">The name of the encryption service to create.</param>
        /// <returns>An instance of the encryption service.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported encryption service name is provided.</exception>
        public IEncryptionService CreateEncryptionService(string name)
        {
            if (Services.TryGetValue(name, out var serviceCreator))
            {
                return serviceCreator();
            }

            throw new ArgumentException("Unsupported encryption service: " + name);
        }
    }
}