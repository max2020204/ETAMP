using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Provides a base implementation for generating ETAMP (Encrypted Token And Message Protocol) payloads,
    /// utilizing a provided signing credentials provider for digital signature operations. This class serves as
    /// a foundation for creating and managing ETAMP tokens with built-in support for digital signing, ensuring
    /// the authenticity and integrity of the data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the ETAMPBase class using the specified signing credentials provider.
    /// This constructor is suitable for scenarios where there is a need to use a specific mechanism for
    /// signing, determined by the provided signing credentials provider.
    /// </remarks>
    /// <param name="signingCredentialsProvider">The provider used to create signing credentials for digital signature operations.</param>
    public class ETAMPBase(ISigningCredentialsProvider signingCredentialsProvider) : ETAMPData(signingCredentialsProvider), IETAMPBase
    {
        /// <summary>
        /// Creates a serialized ETAMP token with the specified update type and payload.
        /// This method constructs an ETAMP token that encapsulates the payload and meta-information
        /// like the update type and protocol version, then serializes this information into a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP token.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP token, indicating the purpose or action associated with the token.</param>
        /// <param name="payload">The payload to be included in the ETAMP token. This data will be signed to ensure its integrity and authenticity.</param>
        /// <param name="version">The version of the ETAMP protocol. This allows for versioning control and future protocol upgrades.</param>
        /// <returns>A serialized ETAMP token as a string, ready for transmission or storage.</returns>
        public virtual string CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload
        {
            return JsonConvert.SerializeObject(CreateETAMPModel(updateType, payload, version));
        }

        /// <summary>
        /// Creates an ETAMP model with the specified update type and payload without serialization.
        /// This method assembles an ETAMP model, combining the payload with meta-information
        /// and preparing it for digital signing. Unlike CreateETAMP, this method returns the
        /// model instance directly for cases where serialization is not immediately required.
        /// </summary>
        /// <typeparam name="T">The type of the payload included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model, indicating the purpose or action associated with the model.</param>
        /// <param name="payload">The payload to be included in the ETAMP model. This data will be signed to ensure its integrity and authenticity.</param>
        /// <param name="version">The version of the ETAMP protocol. This allows for versioning control and future protocol upgrades.</param>
        /// <returns>An instance of the ETAMP model, containing the payload and meta-information.</returns>
        public virtual ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePayload
        {
            Guid messageId = Guid.NewGuid();
            string token = CreateEtampData(messageId.ToString(), payload);
            return new()
            {
                Id = messageId,
                Version = version,
                Token = token,
                UpdateType = updateType
            };
        }
    }
}