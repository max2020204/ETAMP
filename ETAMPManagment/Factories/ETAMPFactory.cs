using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using ETAMPManagment.Utils;

namespace ETAMPManagment.Factories
{
    /// <summary>
    /// Implements a factory for creating instances of different types of ETAMP generators based on the ETAMPType enumeration.
    /// </summary>
    public class ETAMPFactory : IETAMPFactory<ETAMPType>
    {
        /// <summary>
        /// Gets the dictionary mapping ETAMP types to their corresponding generator functions.
        /// </summary>
        public Dictionary<ETAMPType, Func<IETAMPBase>> Factory { get; }

        /// <summary>
        /// Initializes a new instance of the ETAMPFactory class.
        /// </summary>
        public ETAMPFactory()
        {
            Factory = new Dictionary<ETAMPType, Func<IETAMPBase>>();
        }

        /// <summary>
        /// Creates an ETAMP generator instance based on the specified ETAMP type.
        /// </summary>
        /// <param name="type">The ETAMP type for which to create a generator.</param>
        /// <returns>An instance of IETAMPBase corresponding to the specified ETAMP type.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported ETAMP generator type is requested.</exception>
        public virtual IETAMPBase CreateGenerator(ETAMPType type)
        {
            if (Factory.TryGetValue(type, out var func))
            {
                return func();
            }
            throw new ArgumentException($"Unsupported ETAMP generator type: {type}", nameof(type));
        }

        /// <summary>
        /// Registers a new ETAMP generator function under a specified ETAMP type.
        /// </summary>
        /// <param name="type">The ETAMP type under which to register the generator.</param>
        /// <param name="generator">The generator function to be registered.</param>
        public virtual void RegisterGenerator(ETAMPType type, Func<IETAMPBase> generator)
        {
            Factory.Add(type, generator);
        }

        /// <summary>
        /// Unregisters an existing ETAMP generator for a specified ETAMP type.
        /// </summary>
        /// <param name="type">The ETAMP type whose generator is to be unregistered.</param>
        /// <returns>True if the generator was successfully unregistered; otherwise, false.</returns>
        public virtual bool UnregisterETAMPGenerator(ETAMPType type)
        {
            return Factory.Remove(type);
        }
    }
}