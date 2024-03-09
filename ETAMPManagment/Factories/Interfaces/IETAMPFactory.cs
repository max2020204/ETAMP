using ETAMPManagment.ETAMP.Base.Interfaces;

namespace ETAMPManagment.Factories.Interfaces
{
    /// <summary>
    /// Defines a factory for registering and creating ETAMP generators based on a specified type.
    /// </summary>
    /// <typeparam name="Type">The type used to differentiate between ETAMP generators.</typeparam>
    public interface IETAMPFactory<Type>
    {
        /// <summary>
        /// Gets the dictionary mapping types to ETAMP generator functions.
        /// </summary>
        Dictionary<Type, Func<IETAMPBase>> Factory { get; }

        /// <summary>
        /// Registers a generator function for a specified type.
        /// </summary>
        /// <param name="type">The type identifier for the ETAMP generator.</param>
        /// <param name="generator">The function that creates an instance of an ETAMP generator.</param>
        void RegisterGenerator(Type type, Func<IETAMPBase> generator);

        /// <summary>
        /// Creates an ETAMP generator instance based on the specified type.
        /// </summary>
        /// <param name="type">The type identifier for the ETAMP generator to create.</param>
        /// <returns>An instance of <see cref="IETAMPBase"/> corresponding to the specified type.</returns>
        IETAMPBase CreateGenerator(Type type);

        /// <summary>
        /// Unregisters a previously registered ETAMP generator for a specified type.
        /// </summary>
        /// <param name="type">The type identifier for the ETAMP generator to unregister.</param>
        /// <returns><c>true</c> if the generator was successfully unregistered; otherwise, <c>false</c>.</returns>
        bool UnregisterETAMPGenerator(Type type);
    }
}