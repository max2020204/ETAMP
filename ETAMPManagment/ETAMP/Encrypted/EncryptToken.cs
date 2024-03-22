using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    /// <summary>
    /// Provides functionalities for encrypting ETAMP tokens using the Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EncryptToken"/> class.
    /// </remarks>
    /// <param name="structureValidator">The validator used to ensure the integrity and structure of ETAMP tokens.</param>
    /// <param name="eciesEncryptionService">The encryption service to encrypt ETAMP tokens.</param>
    public class EncryptToken(IStructureValidator structureValidator, IEciesEncryptionService eciesEncryptionService) : IEncryptToken
    {
        /// <summary>
        /// Encrypts an ETAMP token and returns the encrypted token as an ETAMPModel.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representation of an ETAMP token to be encrypted.</param>
        /// <returns>An ETAMPModel instance containing the encrypted token.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ETAMP data is invalid.</exception>
        public virtual ETAMPModel EncryptETAMP(string jsonEtamp)
        {
            var model = structureValidator.IsValidEtampFormat(jsonEtamp);
            model.Token = eciesEncryptionService.Encrypt(model.Token);
            return model;
        }

        /// <summary>
        /// Encrypts a serialized ETAMP token string.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representation of an ETAMP token to be encrypted.</param>
        /// <returns>A string representing the encrypted ETAMP token.</returns>
        public virtual string EncryptETAMPToken(string jsonEtamp)
        {
            return JsonConvert.SerializeObject(EncryptETAMP(jsonEtamp));
        }
    }
}