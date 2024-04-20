using ETAMPManagment.Models;

namespace ETAMPManagment.Interfaces
{
    /// <summary>
    /// Defines a builder interface for creating ETAMP (Encrypted Token And Message Protocol) models.
    /// </summary>
    /// <typeparam name="Type">An enum defining the different types of ETAMP that can be built.</typeparam>
    public interface IETAMPBuilder<in Type> where Type : class
    {
        /// <summary>
        /// Builds and returns the final ETAMP model.
        /// </summary>
        /// <returns>The constructed ETAMP model.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the builder is in an invalid state when attempting to build.</exception>
        ETAMPModel Build();

        /// <summary>
        /// Initiates the creation of an ETAMP model of the specified type with the provided details.
        /// </summary>
        /// <typeparam name="T">The type of the payload to include in the ETAMP model.</typeparam>
        /// <param name="type">The specific ETAMP type to create, as defined by the enum <typeparamref name="Type"/>.</param>
        /// <param name="updateType">A string identifier describing the nature or purpose of the ETAMP being created.</param>
        /// <param name="payload">The payload object to include in the ETAMP model.</param>
        /// <param name="version">The version number of the ETAMP protocol to use, defaulting to 1.</param>
        /// <returns>A reference to the builder instance for chaining method calls.</returns>
        IETAMPBuilder<Type> CreateETAMP<T>(Type type, string updateType, T payload, double version = 1) where T : BasePayload;
    }
}