using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Extends <see cref="ETAMPBase"/> to include digital signature functionality for ETAMP (Encrypted Token And Message Protocol) tokens
    /// using a provided signature wrapper. This enhancement ensures the integrity and authenticity of the ETAMP tokens by digitally signing them.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ETAMPSign"/> class with a specified signature wrapper and signing credentials provider.
    /// This constructor is suited for scenarios where specific digital signing functionality and signing credentials are required
    /// for signing operations.
    /// </remarks>
    /// <param name="signWrapper">The wrapper that provides the digital signing functionality.</param>
    /// <param name="signingCredentialsProvider">The provider used to create signing credentials for digital signature operations.</param>
    public class ETAMPSign(ISignWrapper signWrapper, ISigningCredentialsProvider signingCredentialsProvider) : ETAMPBase(signingCredentialsProvider)
    {
        /// <summary>
        /// Creates a serialized ETAMP token with a digital signature based on the specified update type, payload, and protocol version.
        /// This method enhances the security of the ETAMP token by not only creating it but also signing it to ensure its integrity and authenticity.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be included in the ETAMP token.</typeparam>
        /// <param name="updateType">The update type identifier indicating the purpose or action associated with the ETAMP token.</param>
        /// <param name="payload">The payload to be included and signed within the ETAMP token.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>A serialized string representation of the ETAMP token, including its digital signature.</returns>
        public override string CreateETAMP<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(CreateETAMPModel(updateType, payload, version));
        }

        /// <summary>
        /// Creates a signed ETAMP model based on the specified update type, payload, and protocol version.
        /// This method allows for the creation of an ETAMP model that is both constructed and signed, ready for serialization or further processing.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier indicating the purpose or action associated with the ETAMP model.</param>
        /// <param name="payload">The payload to be included and signed within the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>An instance of <see cref="ETAMPModel"/> that has been digitally signed.</returns>
        public override ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1)
        {
            return signWrapper.SignEtampModel(base.CreateETAMPModel(updateType, payload, version));
        }
    }
}