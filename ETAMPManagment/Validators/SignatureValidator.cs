using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Provides functionalities for validating ETAMP messages and tokens, including signature verification.
    /// </summary>
    public class SignatureValidator : ISignatureValidator
    {
        private readonly IVerifyWrapper _verifyWrapper;
        private readonly IStructureValidator _structureValidator;

        /// <summary>
        /// Initializes a new instance of the SignatureValidator class with a verify wrapper.
        /// </summary>
        /// <param name="verifyWrapper">The verify wrapper used for signature verification.</param>
        public SignatureValidator(IVerifyWrapper verifyWrapper)
        {
            _verifyWrapper = verifyWrapper ?? throw new ArgumentNullException(nameof(verifyWrapper));
        }

        /// <summary>
        /// Initializes a new instance of the SignatureValidator class with a structure validator.
        /// </summary>
        /// <param name="structureValidator">The structure validator used for ETAMP message structure validation.</param>
        public SignatureValidator(IStructureValidator structureValidator)
        {
            _structureValidator = structureValidator ?? throw new ArgumentNullException(nameof(structureValidator));
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
            {
                throw new InvalidOperationException("IStructureValidator is not initialized. Ensure that validator is provided.");
            }
            var valid = _structureValidator.IsValidEtampFormat(etamp);
            if (valid.isValid && _structureValidator.ValidateETAMPStructure(valid.model))
            {
                return _verifyWrapper.VerifyData($"{valid.model.Id}{valid.model.Version}{valid.model.Token}{valid.model.UpdateType}{valid.model.SignatureToken}", valid.model.SignatureMessage);
            }
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