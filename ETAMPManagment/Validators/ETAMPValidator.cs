using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Provides functionalities for validating ETAMP tokens, including JWT validation, structural integrity checks, and signature verification.
    /// </summary>
    /// <remarks>
    /// This class integrates various validation mechanisms to ensure the security and integrity of ETAMP tokens.
    /// </remarks>
    /// <param name="jwtValidator">The JWT validator used for validating token lifetime and claims.</param>
    /// <param name="structureValidator">The structure validator used for checking ETAMP token format and structure.</param>
    /// <param name="signatureValidator">The signature validator used for verifying the authenticity of tokens and messages.</param>
    public class ETAMPValidator(IJwtValidator jwtValidator, IStructureValidator structureValidator, ISignatureValidator signatureValidator) : IETAMPValidator
    {
        /// <summary>
        /// Validates an ETAMP token, including checks for JWT validation, structure, and signature, against expected audience and issuer values.
        /// </summary>
        /// <param name="etamp">The ETAMP token model to be validated.</param>
        /// <param name="audience">The expected audience (aud) claim.</param>
        /// <param name="issuer">The expected issuer (iss) claim.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key for signature verification.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the ETAMP token passes all validations; otherwise, false.</returns>
        public virtual async Task<bool> ValidateETAMP(ETAMPModel etamp, string audience, string issuer, ECDsaSecurityKey tokenSecurityKey)
        {
            return new List<bool>()
            {
                structureValidator.ValidateETAMPStructure(etamp).IsValid,
                (await jwtValidator.ValidateToken(etamp.Token,audience,issuer,tokenSecurityKey)).IsValid,
                signatureValidator.ValidateToken(etamp.Token,etamp.SignatureToken),
                signatureValidator.ValidateETAMPMessage(etamp)
            }.TrueForAll(x => x);
        }

        /// <summary>
        /// Validates the structure and signature of an ETAMP token and checks the token's lifetime using a specified security key.
        /// </summary>
        /// <param name="etamp">The ETAMP token model to be validated.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key for signature verification.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the ETAMP token passes all validations; otherwise, false.</returns>
        public virtual async Task<bool> ValidateETAMP(ETAMPModel etamp, ECDsaSecurityKey tokenSecurityKey)
        {
            return new List<bool>()
            {
               structureValidator.ValidateETAMPStructure(etamp).IsValid,
               (await jwtValidator.ValidateLifeTime(etamp.Token,tokenSecurityKey)).IsValid,
               signatureValidator.ValidateToken(etamp.Token, etamp.SignatureToken),
               signatureValidator.ValidateETAMPMessage(etamp)
            }.TrueForAll(x => x);
        }

        /// <summary>
        /// Performs a lightweight validation of an ETAMP token, focusing on structure and signature checks.
        /// </summary>
        /// <param name="etamp">The ETAMP token model to be validated.</param>
        /// <param name="tokenSecurityKey">The ECDsa security key for validating the token's lifetime.</param>
        /// <returns>A task that represents the asynchronous operation, yielding true if the ETAMP token's structure and lifetime are valid; otherwise, false.</returns>
        public virtual async Task<bool> ValidateETAMPLite(ETAMPModel etamp, ECDsaSecurityKey tokenSecurityKey)
        {
            return new List<bool>()
            {
               structureValidator.ValidateETAMPStructureLite(etamp).IsValid,
               (await jwtValidator.ValidateLifeTime(etamp.Token,tokenSecurityKey)).IsValid,
            }.TrueForAll(x => x);
        }
    }
}