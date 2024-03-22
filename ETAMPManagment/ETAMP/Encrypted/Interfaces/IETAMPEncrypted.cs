using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Encrypted.Interfaces
{
    /// <summary>
    /// Defines methods for creating encrypted ETAMP (Encrypted Token And Message Protocol) tokens and models.
    /// This interface extends <see cref="IETAMPBase"/> to include encryption functionalities, ensuring the confidentiality of the payload.
    /// </summary>
    public interface IETAMPEncrypted : IETAMPBase
    {
        /// <summary>
        /// Creates a serialized ETAMP token with encryption, based on the specified update type, payload, and protocol version.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP token.</typeparam>
        /// <param name="updateType">The update type identifier indicating the purpose or action associated with the ETAMP token.</param>
        /// <param name="payload">The payload object to be included and encrypted within the ETAMP token.</param>
        /// <param name="version">The version of the ETAMP protocol to be used. Defaults to 1.</param>
        /// <returns>A json string representing the serialized and encrypted ETAMP token.</returns>
        /// <remarks>
        /// This method provides an additional layer of security by encrypting the payload,
        /// which is useful for sensitive or confidential information transfer.
        /// </remarks>
        string CreateEncryptETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload;

        /// <summary>
        /// Creates an ETAMP model with encryption, based on the specified update type, payload, and protocol version, without serialization.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier indicating the purpose or action associated with the ETAMP model.</param>
        /// <param name="payload">The payload object to be included and encrypted within the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol to be used. Defaults to 1.</param>
        /// <returns>An instance of <see cref="ETAMPModel"/> representing the encrypted ETAMP model.</returns>
        /// <remarks>
        /// Unlike <see cref="CreateEncryptETAMP"/>, this method does not serialize the ETAMP model into a string,
        /// providing flexibility in how the encrypted model is further processed or utilized.
        /// </remarks>
        ETAMPModel CreateEncryptETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePayload;
    }
}