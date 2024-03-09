using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Implements validation for ETAMP tokens, incorporating JWT validation, structural integrity checks, and signature verification.
    /// </summary>
    public class ETAMPValidator : IETAMPValidator
    {
        private readonly IJwtValidator _jwtValidator;
        private readonly IStructureValidator _structureValidator;
        private readonly ISignatureValidator _signatureValidator;

        /// <summary>
        /// Initializes a new instance of the ETAMPValidator class with the specified validators.
        /// </summary>
        /// <param name="jwtValidator">The JWT validator to use for validating token lifetime and claims.</param>
        /// <param name="structureValidator">The structure validator to use for checking ETAMP token format and structure.</param>
        /// <param name="signatureValidator">The signature validator to use for verifying the authenticity of tokens and messages.</param>
        public ETAMPValidator(IJwtValidator jwtValidator, IStructureValidator structureValidator, ISignatureValidator signatureValidator)
        {
            _jwtValidator = jwtValidator;
            _structureValidator = structureValidator;
            _signatureValidator = signatureValidator;
        }

        /// <summary>
        /// Validates an ETAMP token, including JWT validation, structure, and signature checks, against expected audience and issuer values.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim.</param>
        /// <param name="issuer">The expected issuer (iss) claim.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key for signature verification.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the ETAMP token passes all validations; otherwise, false.</returns>
        public virtual async Task<bool> ValidateETAMP(string etamp, string audience, string issuer, ECDsaSecurityKey tokenSecurityKey)
        {
            var structure = _structureValidator.IsValidEtampFormat(etamp);
            if (structure.isValid)
            {
                List<bool> valid = new()
                {
                    _structureValidator.ValidateETAMPStructure(etamp),
                    await _jwtValidator.ValidateToken(structure.model.Token,audience,issuer,tokenSecurityKey),
                    _signatureValidator.ValidateToken(structure.model.Token,structure.model.SignatureToken),
                    _signatureValidator.ValidateETAMPMessage(structure.model)
                };
                return valid.TrueForAll(x => x);
            }
            return false;
        }

        /// <summary>
        /// Validates an ETAMP token for structure and signature checks, and validates the token's lifetime using a specified security key.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key for signature verification.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the ETAMP token passes all validations; otherwise, false.</returns>
        public virtual async Task<bool> ValidateETAMP(string etamp, ECDsaSecurityKey tokenSecurityKey)
        {
            var structure = _structureValidator.IsValidEtampFormat(etamp);
            if (structure.isValid)
            {
                List<bool> valid = new()
                {
                    _structureValidator.ValidateETAMPStructure(etamp),
                    await _jwtValidator.ValidateLifeTime(structure.model.Token,tokenSecurityKey),
                    _signatureValidator.ValidateToken(structure.model.Token,structure.model.SignatureToken),
                    _signatureValidator.ValidateETAMPMessage(structure.model)
                };
                return valid.TrueForAll(x => x);
            }
            return false;
        }

        /// <summary>
        /// Performs a lightweight validation of an ETAMP token, focusing on structure and lifetime checks.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key for validating the token's lifetime.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the ETAMP token's structure and lifetime are valid; otherwise, false.</returns>
        public virtual async Task<bool> ValidateETAMPLite(string etamp, ECDsaSecurityKey tokenSecurityKey)
        {
            var structure = _structureValidator.IsValidEtampFormat(etamp);
            if (structure.isValid)
            {
                List<bool> valid = new()
                {
                    _structureValidator.ValidateETAMPStructure(etamp),
                    await _jwtValidator.ValidateLifeTime(structure.model.Token,tokenSecurityKey)
                };
                return valid.TrueForAll(x => x);
            }
            return false;
        }
    }
}