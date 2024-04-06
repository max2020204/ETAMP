using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Provides a base implementation for generating ETAMP payloads and managing digital signatures.
    /// </summary>
    /// <param name="signingCredentialsProvider">The provider for creating signing credentials.</param>
    public class ETAMPBase(ISigningCredentialsProvider signingCredentialsProvider)
        : ETAMPData(signingCredentialsProvider), IETAMPBase
    {
        /// <summary>
        /// Creates an ETAMP model with the specified details.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload for the ETAMP model.</param>
        /// <param name="version">The ETAMP protocol version, defaulting to 1.</param>
        /// <returns>An ETAMP model instance.</returns>
        public virtual ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1)
            where T : BasePayload
        {
            Guid messageId = Guid.NewGuid();
            return new ETAMPModel
            {
                Id = messageId,
                Version = version,
                Token = CreateEtampData(messageId.ToString(), payload),
                UpdateType = updateType
            };
        }
    }
}