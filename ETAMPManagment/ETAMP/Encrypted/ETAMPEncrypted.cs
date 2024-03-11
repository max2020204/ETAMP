using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Extends <see cref="ETAMPBase"/> to incorporate encryption functionalities into ETAMP (Encrypted Token And Message Protocol) tokens,
    /// using the Elliptic Curve Integrated Encryption Scheme (ECIES). This class allows for the creation of ETAMP tokens that are not only signed but also encrypted,
    /// enhancing confidentiality alongside the existing integrity and authentication provided by the base functionality.
    /// </summary>
    public class ETAMPEncrypted : ETAMPBase, IETAMPEncrypted
    {
        /// <summary>
        /// The encryption service responsible for encrypting the payload of the ETAMP tokens.
        /// </summary>
        private readonly IEciesEncryptionService _eciesEncryptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPEncrypted"/> class, solely with an encryption service for encrypting ETAMP tokens.
        /// This constructor can be used when encryption is the only requirement and a new ECDsa instance for signing is created internally.
        /// </summary>
        /// <param name="eciesEncryptionService">The service used to encrypt ETAMP tokens.</param>
        public ETAMPEncrypted(IEciesEncryptionService eciesEncryptionService)
        {
            _eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPEncrypted"/> class with an encryption service, a specified ECDsa instance, and a security algorithm.
        /// This constructor is suitable for scenarios requiring both encryption of the ETAMP tokens and the use of a specific ECDsa instance for digital signatures.
        /// </summary>
        /// <param name="eciesEncryptionService">The service used to encrypt ETAMP tokens.</param>
        /// <param name="ecdsa">The ECDsa instance used for digital signature operations.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier used for signing. Defaults to EcdsaSha512Signature.</param>
        public ETAMPEncrypted(IEciesEncryptionService eciesEncryptionService, ECDsa ecdsa, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature) : base(ecdsa, securityAlgorithm)
        {
            _eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAMPEncrypted"/> class with an encryption service and a security algorithm, creating a new ECDsa instance internally.
        /// This constructor is ideal for use cases where encryption is needed alongside digital signing with a default ECDsa configuration.
        /// </summary>
        /// <param name="eciesEncryptionService">The service used to encrypt ETAMP tokens.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier used for signing.</param>
        public ETAMPEncrypted(IEciesEncryptionService eciesEncryptionService, string securityAlgorithm) : base(securityAlgorithm)
        {
            _eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Creates a serialized, encrypted ETAMP token based on the specified update type, payload, and protocol version.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP token.</param>
        /// <param name="payload">The payload to be encrypted and included in the ETAMP token.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>A serialized string representation of the encrypted ETAMP token.</returns>
        public virtual string CreateEncryptETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            ETAMPModel model = CreateETAMPModel(updateType, payload, version);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Creates an ETAMP model with encryption based on the specified update type, payload, and protocol version.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload to be encrypted and included in the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1.</param>
        /// <returns>An instance of <see cref="ETAMPModel"/> containing the encrypted payload.</returns>
        public virtual ETAMPModel CreateEncryptETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            ETAMPModel model = CreateETAMPModel(updateType, payload, version);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return model;
        }
    }
}