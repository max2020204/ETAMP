using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Provides a base implementation for generating ETAMP payloads and managing digital signatures.
    /// This class integrates ETAMP data handling and digital signature management to facilitate the creation of ETAMP models with optional custom signing mechanisms.
    /// </summary>
    public class ETAMPBase : IETAMPBase
    {
        private readonly ISigningCredentialsProvider _signingCredentialsProvider;
        private readonly IETAMPData _etampData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPBase"/> class using the provided signing credentials provider and ETAMP data handler.
        /// </summary>
        /// <param name="signingCredentialsProvider">The default provider for creating signing credentials.</param>
        /// <param name="etampData">The handler responsible for creating ETAMP token data.</param>
        public ETAMPBase(ISigningCredentialsProvider signingCredentialsProvider, IETAMPData etampData)
        {
            _signingCredentialsProvider = signingCredentialsProvider ?? throw new ArgumentNullException(nameof(signingCredentialsProvider));
            _etampData = etampData ?? throw new ArgumentNullException(nameof(etampData));
        }

        /// <summary>
        /// Creates an ETAMP model with the specified details. This method allows specifying an alternative signing credentials provider to be used for this specific model creation.
        /// </summary>
        /// <typeparam name="T">The payload type to be included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model, describing the nature of the update.</param>
        /// <param name="payload">The payload for the ETAMP model.</param>
        /// <param name="version">The ETAMP protocol version, defaulting to 1. Specifies the version of the protocol to ensure compatibility.</param>
        /// <param name="provider">Optional. An alternative provider for signing credentials. If not provided, the default provider is used.</param>
        /// <returns>An ETAMP model instance containing the digitally signed payload.</returns>
        public virtual ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1, ISigningCredentialsProvider? provider = null) where T : BasePayload
        {
            var credentialsProvider = provider ?? _signingCredentialsProvider;
            Guid messageId = Guid.NewGuid();
            return new ETAMPModel
            {
                Id = messageId,
                Version = version,
                Token = _etampData.CreateEtampData(messageId.ToString(), payload, credentialsProvider),
                UpdateType = updateType
            };
        }
    }
}