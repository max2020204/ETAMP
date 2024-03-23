using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Base.Interfaces
{
    /// <summary>
    /// Interface for ETAMP data processing, providing a method to create ETAMP token data.
    /// </summary>
    public interface IETAMPData
    {
        /// <summary>
        /// Creates ETAMP token data with a digital signature for the specified payload and message ID.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="messageId">The message identifier for the ETAMP token.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <returns>A string representing the serialized and signed ETAMP token data.</returns>
        string CreateEtampData<T>(string messageId, T payload) where T : BasePayload;
    }
}