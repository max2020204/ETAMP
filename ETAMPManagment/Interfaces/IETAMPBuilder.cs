using ETAMPManagment.Models;

namespace ETAMPManagment.Interfaces
{
    /// <summary>
    /// Defines the contract for a builder that creates and configures ETAMP (Encrypted Token And Message Protocol) models.
    /// </summary>
    public interface IETAMPBuilder
    {
        /// <summary>
        /// Builds and returns the configured ETAMP model.
        /// </summary>
        /// <returns>The configured ETAMP model after applying all specified configurations.</returns>
        ETAMPModel Build();

        /// <summary>
        /// Creates and configures an encrypted ETAMP model.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload data to be included in the model.</param>
        /// <param name="version">The version number of the ETAMP protocol to use.</param>
        /// <returns>The builder instance for chaining further configuration calls.</returns>
        ETAMPBuilder CreateEncryptedETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload;

        /// <summary>
        /// Creates and configures a model that is both encrypted and signed, enhancing the security and integrity of the ETAMP model.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier.</param>
        /// <param name="payload">The payload data.</param>
        /// <param name="version">The protocol version.</param>
        /// <returns>The builder instance for chaining.</returns>
        ETAMPBuilder CreateEncryptedSignETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload;

        /// <summary>
        /// Creates and configures a basic ETAMP model.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload data to be included in the model.</param>
        /// <param name="version">The version number of the ETAMP protocol to use.</param>
        /// <returns>The builder instance for chaining further configuration calls.</returns>
        ETAMPBuilder CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload;

        /// <summary>
        /// Creates and configures a signed ETAMP model.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier.</param>
        /// <param name="payload">The payload data.</param>
        /// <param name="version">The protocol version.</param>
        /// <returns>The builder instance for chaining.</returns>
        ETAMPBuilder CreateSignETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload;
    }
}