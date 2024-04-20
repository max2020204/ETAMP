using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Provides encryption capabilities for ETAMP tokens using ECIES, extending the basic ETAMP functionality.
    /// </summary>
    public class ETAMPEncrypted : ETAMPBase, IETAMPEncrypted
    {
        private readonly IEciesEncryptionService eciesEncryptionService;

        /// <summary>
        /// Initializes a new instance of the ETAMPEncrypted class with specified encryption and signing services.
        /// </summary>
        /// <param name="eciesEncryptionService">The service used to encrypt ETAMP tokens.</param>
        /// <param name="signingCredentialsProvider">The provider used for digital signature operations.</param>
        public ETAMPEncrypted(IEciesEncryptionService eciesEncryptionService, ISigningCredentialsProvider signingCredentialsProvider)
            : base(signingCredentialsProvider)
        {
            this.eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Creates an encrypted ETAMP model based on the given parameters.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be encrypted and included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload to be encrypted and included in the ETAMP model.</param>
        /// <param name="version">The ETAMP protocol version, defaulting to 1.</param>
        /// <returns>An encrypted ETAMP model instance.</returns>
        public virtual ETAMPModel CreateEncryptETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePayload
        {
            ETAMPModel model = base.CreateETAMPModel(updateType, payload, version);
            ArgumentException.ThrowIfNullOrWhiteSpace(model.Token);
            model.Token = eciesEncryptionService.Encrypt(model.Token);
            return model;
        }
    }
}