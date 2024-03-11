using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Defines functionality for generating data for ETAMP (Encrypted Token And Message Protocol) payloads.
    /// </summary>
    public interface IETAMPData
    {
        /// <summary>
        /// Generates serialized data for an ETAMP payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="messageId">The unique identifier for the message.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <returns>A serialized string representing the token data.</returns>
        string CreateEtampData<T>(string messageId, T payload) where T : BasePaylaod;
    }
}