using ETAMPManagment.ETAMP.Base.Interfaces;

namespace ETAMPManagment.Factories.Interfaces
{
    /// <summary>
    /// Defines a factory for creating ETAMP data generators.
    /// </summary>
    /// <typeparam name="Type">The type used as a key to register and retrieve ETAMP data generators.</typeparam>
    public interface IETAMPFactory<Type>
    {
        /// <summary>
        /// Gets the dictionary containing registered ETAMP data generators.
        /// </summary>
        Dictionary<Type, Func<IETAMPData>> Factory { get; }

        /// <summary>
        /// Registers a generator function for creating ETAMP data instances.
        /// </summary>
        /// <param name="type">The key used to register the generator.</param>
        /// <param name="generator">The function that creates an instance of ETAMP data.</param>
        void RegisterGenerator(Type type, Func<IETAMPData> generator);

        /// <summary>
        /// Creates an ETAMP data instance using the registered generator function associated with the specified key.
        /// </summary>
        /// <param name="type">The key associated with the generator to create an ETAMP data instance.</param>
        /// <returns>An instance of ETAMP data created by the generator function.</returns>
        IETAMPData CreateGenerator(Type type);

        /// <summary>
        /// Unregisters the ETAMP data generator associated with the specified key.
        /// </summary>
        /// <param name="type">The key associated with the generator to be unregistered.</param>
        /// <returns>True if the generator was successfully unregistered; otherwise, false.</returns>
        bool UnregisterETAMPGenerator(Type type);
    }
}