using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Validates ETAMP messages and tokens for authenticity and integrity by verifying their signatures.
    /// </summary>
    public class SignatureValidator : ISignatureValidator
    {
        private readonly IVerifyWrapper _verifyWrapper;
        private readonly IStructureValidator _structureValidator;

        /// <summary>
        /// Initializes a new instance of the SignatureValidator class with specified verify and structure validators.
        /// </summary>
        /// <param name="verifyWrapper">The verify wrapper used for signature verification.</param>
        /// <param name="structureValidator">The structure validator used for ETAMP message structure validation.</param>
        /// <exception cref="ArgumentNullException">Thrown if either verifyWrapper or structureValidator is null.</exception>
        public SignatureValidator(IVerifyWrapper verifyWrapper, IStructureValidator structureValidator)
        {
            _verifyWrapper = verifyWrapper
                ?? throw new ArgumentNullException(nameof(verifyWrapper));
            _structureValidator = structureValidator
                ?? throw new ArgumentNullException(nameof(structureValidator));
        }

        /// <summary>
        /// Validates the signature of an ETAMP message given as a JSON string.
        /// </summary>
        /// <param name="etamp">The ETAMP message as a JSON string.</param>
        /// <returns>True if the message signature is valid, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if IStructureValidator is not initialized.</exception>
        public virtual bool ValidateETAMPMessage(string etamp)
        {
            if (_structureValidator == null)
                throw new InvalidOperationException("IStructureValidator is not initialized. Ensure that validator is provided.");

            var model = _structureValidator.IsValidEtampFormat(etamp);
            if (_structureValidator.ValidateETAMPStructure(model).IsValid)
                return _verifyWrapper.VerifyData($"{model.Id}{model.Version}{model.Token}{model.UpdateType}{model.SignatureToken}", model.SignatureMessage);

            return false;
        }

        /// <summary>
        /// Validates the signature of an ETAMP message provided as an ETAMPModel.
        /// </summary>
        /// <param name="etamp">The ETAMP message as an ETAMPModel.</param>
        /// <returns>True if the message signature is valid, false otherwise.</returns>
        public virtual bool ValidateETAMPMessage(ETAMPModel etamp)
        {
            return _verifyWrapper.VerifyData($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}", etamp.SignatureMessage);
        }

        /// <summary>
        /// Validates a token against its signature.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <param name="tokenSignature">The signature of the token.</param>
        /// <returns>True if the token signature is valid, false otherwise.</returns>
        public virtual bool ValidateToken(string token, string tokenSignature)
        {
            return _verifyWrapper.VerifyData(token, tokenSignature);
        }
    }
}