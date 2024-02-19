using ETAMP.Interfaces;
using ETAMP.Models;
using ETAMP.Services.Interfaces;
using Newtonsoft.Json;

namespace ETAMP
{
    /// <summary>
    /// Provides encryption functionalities for ETAMP (Encrypted Token And Message Protocol) using ECIES (Elliptic Curve Integrated Encryption Scheme).
    /// </summary>
    public class ETAMPEncryption : ETAMP, IETAMPEncryption
    {
        private readonly IEciesEncryptionService _eciesEncryptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPEncryption"/> class.
        /// </summary>
        /// <param name="eciesEncryptionService">The ECIES encryption service used for encrypting tokens.</param>
        public ETAMPEncryption(IEciesEncryptionService eciesEncryptionService)
        {
            _eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Encrypts an ETAMP token represented as a JSON string.
        /// </summary>
        /// <param name="jsonEtamp">The JSON representation of the ETAMP token to encrypt.</param>
        /// <returns>The encrypted ETAMP token as a JSON string.</returns>
        public string EnryptETAMPToken(string jsonEtamp)
        {
            ETAMPModel model = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Encrypts an ETAMP message represented as a JSON string and returns an encrypted ETAMP object.
        /// </summary>
        /// <param name="jsonEtamp">The JSON representation of the ETAMP message to encrypt.</param>
        /// <returns>An instance of <see cref="ETAMPEncrypted"/> containing the encrypted ETAMP message.</returns>
        public ETAMPEncrypted EnryptETAMP(string jsonEtamp)
        {
            ETAMPModel model = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return new ETAMPEncrypted
            {
                ETAMP = JsonConvert.SerializeObject(model),
                PrivateKey = _eciesEncryptionService.EcdhKeyWrapper.PrivateKey,
                PublicKey = _eciesEncryptionService.EcdhKeyWrapper.PublicKey
            };
        }

        /// <summary>
        /// Creates and encrypts an ETAMP token with the specified update type and payload.
        /// </summary>
        /// <param name="updateType">The update type of the ETAMP token.</param>
        /// <param name="payload">The payload to be included in the ETAMP token.</param>
        /// <param name="signToken">Indicates whether the token should be signed.</param>
        /// <param name="version">The version of the ETAMP protocol.</param>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <returns>The encrypted ETAMP token as a JSON string.</returns>

        public string CreateEnryptETAMPToken<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod
        {
            string token = CreateETAMP(updateType, payload, signToken, version);
            return EnryptETAMPToken(token);
        }

        /// <summary>
        /// Creates and encrypts an ETAMP message with the specified update type and payload, returning an encrypted ETAMP object.
        /// </summary>
        /// <param name="updateType">The update type of the ETAMP message.</param>
        /// <param name="payload">The payload to be included in the ETAMP message.</param>
        /// <param name="signToken">Indicates whether the message should be signed.</param>
        /// <param name="version">The version of the ETAMP protocol.</param>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <returns>An instance of <see cref="ETAMPEncrypted"/> containing the encrypted ETAMP message.</returns>
        public ETAMPEncrypted CreateEnryptETAMP<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod
        {
            string token = CreateETAMP(updateType, payload, signToken, version);
            return new ETAMPEncrypted
            {
                ETAMP = EnryptETAMPToken(token),
                PrivateKey = _eciesEncryptionService.EcdhKeyWrapper.PrivateKey,
                PublicKey = _eciesEncryptionService.EcdhKeyWrapper.PublicKey
            };
        }
    }
}