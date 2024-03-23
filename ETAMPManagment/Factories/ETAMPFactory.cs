using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using ETAMPManagment.Utils;

namespace ETAMPManagment.Factories
{
    /// <summary>
    /// A factory class for creating ETAMP (Encrypted Token And Message Protocol) data generators.
    /// This class manages a registry of ETAMP generators, allowing dynamic creation of ETAMP data based on specific types.
    /// </summary>
    public class ETAMPFactory : IETAMPFactory<ETAMPType>
    {
        /// <summary>
        /// Gets the mapping of ETAMP types to their corresponding data generator functions.
        /// </summary>
        public Dictionary<ETAMPType, Func<IETAMPData>> Factory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPFactory"/> class.
        /// The constructor sets up an empty dictionary ready to register ETAMP data generators.
        /// </summary>
        public ETAMPFactory() => Factory = new Dictionary<ETAMPType, Func<IETAMPData>>();

        /// <summary>
        /// Creates an ETAMP data generator based on the specified ETAMP type.
        /// </summary>
        /// <param name="type">The ETAMP type for which to create a data generator.</param>
        /// <returns>The ETAMP data generator associated with the specified type.</returns>
        /// <exception cref="ArgumentException">Thrown if the specified ETAMP type is not supported or has no registered generator.</exception>
        public virtual IETAMPData CreateGenerator(ETAMPType type)
        {
            if (Factory.TryGetValue(type, out var func))
                return func();

            throw new ArgumentException($"Unsupported ETAMP generator type: {type}", nameof(type));
        }

        /// <summary>
        /// Registers a generator function for a specified ETAMP type.
        /// This method allows dynamic addition of ETAMP data generators to the factory.
        /// </summary>
        /// <param name="type">The ETAMP type to register the generator for.</param>
        /// <param name="generator">The generator function that creates an instance of <see cref="IETAMPData"/>.</param>
        /// <exception cref="ArgumentException">Thrown if a generator for the specified ETAMP type is already registered.</exception>
        public virtual void RegisterGenerator(ETAMPType type, Func<IETAMPData> generator)
        {
            if (Factory.ContainsKey(type))
                throw new ArgumentException($"A generator for the ETAMP type '{type}' is already registered.", nameof(type));

            Factory[type] = generator;
        }

        /// <summary>
        /// Unregisters an ETAMP generator for a specified ETAMP type.
        /// This method allows removal of ETAMP data generators from the factory, providing flexibility in managing available generators.
        /// </summary>
        /// <param name="type">The ETAMP type whose generator is to be unregistered.</param>
        /// <returns><c>true</c> if the generator was successfully unregistered; otherwise, <c>false</c>.</returns>
        public virtual bool UnregisterETAMPGenerator(ETAMPType type)
        {
            return Factory.Remove(type);
        }
    }
}