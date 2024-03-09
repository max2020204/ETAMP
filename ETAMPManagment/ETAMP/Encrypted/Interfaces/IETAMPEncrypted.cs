using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Encrypted.Interfaces
{
    /// <summary>
    /// Defines methods for creating encrypted ETAMP (Encrypted Token And Message Protocol) tokens.
    /// </summary>
    public interface IETAMPEncrypted
    {
        /// <summary>
        /// Creates and returns an encrypted ETAMP token as a string.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be encrypted within the ETAMP token.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP token.</param>
        /// <param name="payload">The payload data to be included and encrypted in the ETAMP token.</param>
        /// <param name="version">The version of the ETAMP protocol to use, defaulting to 1.</param>
        /// <returns>A string representing the encrypted ETAMP token.</returns>
        string CreateEncryptETAMPToken<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;

        /// <summary>
        /// Creates and returns an encrypted ETAMP token as an ETAMPModel.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be encrypted within the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload data to be included and encrypted in the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol to use, defaulting to 1.</param>
        /// <returns>An ETAMPModel instance representing the encrypted ETAMP token.</returns>
        ETAMPModel CreateEncryptETAMPTokenModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod;
    }
}