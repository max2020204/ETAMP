using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Extends <see cref="ETAMPEncrypted"/> to include digital signing of the encrypted ETAMP (Encrypted Token And Message Protocol) tokens.
    /// This class enhances the security of ETAMP tokens by adding a digital signature on top of encryption, ensuring both confidentiality and authenticity.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ETAMPEncryptedSigned"/> class, enabling both encryption and digital signing of ETAMP tokens.
    /// </remarks>
    /// <param name="signWrapper">The digital signature wrapper to sign the ETAMP tokens.</param>
    /// <param name="eciesEncryptionService">The encryption service to encrypt the ETAMP tokens.</param>
    public class ETAMPEncryptedSigned(ISignWrapper signWrapper, IEciesEncryptionService eciesEncryptionService, ISigningCredentialsProvider signingCredentialsProvider) : ETAMPEncrypted(eciesEncryptionService, signingCredentialsProvider)
    {
        /// <summary>
        /// Creates an encrypted and signed ETAMP token as a serialized string.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP token.</param>
        /// <param name="payload">The payload to include in the token.</param>
        /// <param name="version">The version of the protocol, defaulting to 1.</param>
        /// <returns>A serialized string representation of the encrypted and signed ETAMP token.</returns>
        public override string CreateEncryptETAMP<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(CreateEncryptETAMPModel(updateType, payload, version));
        }

        /// <summary>
        /// Creates an encrypted and signed ETAMP model.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload to include in the model.</param>
        /// <param name="version">The version of the protocol, defaulting to 1.</param>
        /// <returns>An ETAMPModel instance containing the encrypted and signed token.</returns>
        public override ETAMPModel CreateEncryptETAMPModel<T>(string updateType, T payload, double version = 1)
        {
            return signWrapper.SignEtampModel(base.CreateEncryptETAMPModel(updateType, payload, version));
        }
    }
}