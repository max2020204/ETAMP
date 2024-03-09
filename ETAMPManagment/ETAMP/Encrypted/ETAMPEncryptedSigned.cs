using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Extends <see cref="ETAMPEncrypted"/> to include digital signing of the encrypted ETAMP (Encrypted Token And Message Protocol) tokens.
    /// </summary>
    public class ETAMPEncryptedSigned : ETAMPEncrypted
    {
        /// <summary>
        /// Wrapper for digital signing functionalities.
        /// </summary>
        private readonly ISignWrapper _signWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPEncryptedSigned"/> class with an ETAMP base, signature wrapper, and encryption service.
        /// </summary>
        /// <param name="etampBase">The base ETAMP service for creating tokens.</param>
        /// <param name="signWrapper">The signature wrapper to sign ETAMP models.</param>
        /// <param name="eciesEncryptionService">The encryption service to encrypt tokens.</param>
        public ETAMPEncryptedSigned(IETAMPBase etampBase, ISignWrapper signWrapper, IEciesEncryptionService eciesEncryptionService) : base(etampBase, eciesEncryptionService)
        {
            _signWrapper = signWrapper;
        }

        /// <summary>
        /// Creates a digitally signed and encrypted ETAMP token as a string.
        /// </summary>
        /// <typeparam name="T">The type of the payload within the ETAMP token.</typeparam>
        /// <param name="updateType">The type of update the ETAMP token represents.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <param name="version">The protocol version for the ETAMP token.</param>
        /// <returns>A string representation of the signed and encrypted ETAMP token.</returns>
        public override string CreateEncryptETAMPToken<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(this.CreateEncryptETAMPTokenModel(updateType, payload, version));
        }

        /// <summary>
        /// Creates a digitally signed and encrypted ETAMP token model.
        /// </summary>
        /// <typeparam name="T">The type of the payload within the ETAMP token.</typeparam>
        /// <param name="updateType">The type of update the ETAMP token represents.</param>
        /// <param name="payload">The payload to include in the ETAMP token.</param>
        /// <param name="version">The protocol version for the ETAMP token.</param>
        /// <returns>An ETAMPModel of the signed and encrypted ETAMP token.</returns>
        public override ETAMPModel CreateEncryptETAMPTokenModel<T>(string updateType, T payload, double version = 1)
        {
            return _signWrapper.SignEtampModel(base.CreateEncryptETAMPTokenModel(updateType, payload, version));
        }
    }
}