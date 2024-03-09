using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Implements encryption functionalities for ETAMP (Encrypted Token And Message Protocol) tokens using ECIES (Elliptic Curve Integrated Encryption Scheme).
    /// </summary>
    public class ETAMPEncrypted : IETAMPEncrypted
    {
        /// <summary>
        /// Base implementation for ETAMP token creation.
        /// </summary>
        private readonly IETAMPBase _etampBase;

        /// <summary>
        /// Service for encrypting ETAMP tokens.
        /// </summary>

        private readonly IEciesEncryptionService _eciesEncryptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPEncrypted"/> class with specified ETAMP base and encryption service.
        /// </summary>
        /// <param name="etampBase">The base ETAMP service for creating tokens.</param>
        /// <param name="eciesEncryptionService">The encryption service to be used for encrypting tokens.</param>
        public ETAMPEncrypted(IETAMPBase etampBase, IEciesEncryptionService eciesEncryptionService)
        {
            _etampBase = etampBase;
            _eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Creates an encrypted ETAMP token as a string.
        /// </summary>
        /// <typeparam name="T">The type of the payload within the ETAMP token.</typeparam>
        /// <param name="updateType">The type of update the ETAMP token represents.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <param name="version">The protocol version for the ETAMP token.</param>
        /// <returns>A string representation of the encrypted ETAMP token.</returns>
        public virtual string CreateEncryptETAMPToken<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            ETAMPModel model = _etampBase.CreateETAMPModel(updateType, payload, version);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Creates an encrypted ETAMP token model.
        /// </summary>
        /// <typeparam name="T">The type of the payload within the ETAMP token.</typeparam>
        /// <param name="updateType">The type of update the ETAMP token represents.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <param name="version">The protocol version for the ETAMP token.</param>
        /// <returns>An ETAMPModel of the encrypted ETAMP token.</returns>
        public virtual ETAMPModel CreateEncryptETAMPTokenModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            ETAMPModel model = _etampBase.CreateETAMPModel(updateType, payload, version);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return model;
        }
    }
}