using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Provides functionalities for encrypting ETAMP tokens using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    public class EncryptToken : IEncryptToken
    {
        private readonly IStructureValidator structureValidator;
        private readonly IEciesEncryptionService eciesEncryptionService;

        /// <summary>
        /// Initializes a new instance of the EncryptToken class with the specified structure validator and ECIES encryption service.
        /// </summary>
        /// <param name="structureValidator">The validator used to ensure the integrity and structure of ETAMP tokens.</param>
        /// <param name="eciesEncryptionService">The encryption service to encrypt ETAMP tokens.</param>
        public EncryptToken(IStructureValidator structureValidator, IEciesEncryptionService eciesEncryptionService)
        {
            this.structureValidator = structureValidator;
            this.eciesEncryptionService = eciesEncryptionService;
        }

        /// <summary>
        /// Encrypts an ETAMP token and returns the encrypted token as an ETAMPModel.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representation of an ETAMP token to be encrypted.</param>
        /// <returns>An ETAMPModel instance containing the encrypted token.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ETAMP data is invalid.</exception>
        public virtual ETAMPModel EncryptETAMP(string jsonEtamp)
        {
            var model = structureValidator.IsValidEtampFormat(jsonEtamp);
            ArgumentException.ThrowIfNullOrWhiteSpace(model.Token);
            model.Token = eciesEncryptionService.Encrypt(model.Token);
            return model;
        }
    }
}