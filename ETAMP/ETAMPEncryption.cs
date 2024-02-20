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
        /// <param name="eciesEncryptionService">The ECIES encryption service used for encrypting tokens and messages.</param>
        public ETAMPEncryption(IEciesEncryptionService eciesEncryptionService)
        {
            _eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Deserializes the given JSON string into an ETAMP model and validates it.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representation of an ETAMP model.</param>
        /// <returns>The deserialized ETAMP model.</returns>
        /// <exception cref="ArgumentException">Thrown if the input string is null, empty, or not in a valid JSON format.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the deserialized ETAMP model is null.</exception>
        private ETAMPModel DeserializeETAMPModel(string jsonEtamp)
        {
            if (string.IsNullOrEmpty(jsonEtamp))
            {
                throw new ArgumentException("JSON ETAMP cannot be null or empty", nameof(jsonEtamp));
            }

            ETAMPModel? model;
            try
            {
                model = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Invalid JSON ETAMP format", nameof(jsonEtamp), ex);
            }

            if (model == null)
            {
                throw new InvalidOperationException("Deserialized ETAMP model is null");
            }

            return model;
        }

        /// <summary>
        /// Encrypts an ETAMP token represented as a JSON string.
        /// </summary>
        /// <param name="jsonEtamp">The JSON representation of the ETAMP token to encrypt.
        /// Must be a valid JSON and cannot be null or empty.</param>
        /// <returns>The encrypted ETAMP token as a JSON string.</returns>
        public virtual string EncryptETAMPToken(string jsonEtamp)
        {
            ETAMPModel model = DeserializeETAMPModel(jsonEtamp);

            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Encrypts an ETAMP message represented as a JSON string and returns an encrypted ETAMP object.
        /// </summary>
        /// <param name="jsonEtamp">The JSON representation of the ETAMP message to encrypt.
        /// Must be a valid JSON and cannot be null or empty.</param>
        /// <returns>An instance of <see cref="ETAMPEncrypted"/> containing the encrypted ETAMP message.</returns>
        public virtual ETAMPEncrypted EncryptETAMP(string jsonEtamp)
        {
            ETAMPModel model = DeserializeETAMPModel(jsonEtamp);

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
        public virtual string CreateEncryptETAMPToken<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod
        {
            string token = CreateETAMP(updateType, payload, signToken, version);
            return EncryptETAMPToken(token);
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
        public virtual ETAMPEncrypted CreateEncryptETAMP<T>(string updateType, T payload, bool signToken = true, double version = 1) where T : BasePaylaod
        {
            string token = CreateETAMP(updateType, payload, signToken, version);
            return new ETAMPEncrypted
            {
                ETAMP = EncryptETAMPToken(token),
                PrivateKey = _eciesEncryptionService.EcdhKeyWrapper.PrivateKey,
                PublicKey = _eciesEncryptionService.EcdhKeyWrapper.PublicKey
            };
        }
    }
}