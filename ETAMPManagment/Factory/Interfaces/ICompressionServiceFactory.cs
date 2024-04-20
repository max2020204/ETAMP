using ETAMPManagment.Services.Interfaces;

namespace ETAMPManagment.Factory.Interfaces
{
    /// <summary>
    /// Defines a factory interface for creating instances of compression services based on a specified type.
    /// </summary>
    public interface ICompressionServiceFactory
    {
        IDictionary<string, ICompressionService> Factory { get; }

        /// <summary>
        /// Creates an instance of a compression service based on the specified compression type.
        /// </summary>
        /// <param name="compressionType">The type of compression to create. This type should correspond to one of the supported compression algorithms.</param>
        /// <returns>An instance of <see cref="ICompressionService"/> that corresponds to the specified compression type.</returns>
        ICompressionService Create(string compressionType);

        void RegisterCompressionService(string compressionType, ICompressionService serviceFactory);

        bool UnregisterCompressionService(string compressionType);
    }
}