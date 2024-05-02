#region

using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace ETAMPManagment.Validators;

/// <summary>
///     Validates ETAMP tokens, combining JWT validation, structural checks, and signature verification.
/// </summary>
/// <param name="jwtValidator">Validator for JWT token attributes.</param>
/// <param name="structureValidator">Validator for ETAMP token structure.</param>
/// <param name="signatureValidator">Validator for token and message signatures.</param>
public sealed class ETAMPValidator(
    IJwtValidator jwtValidator,
    IStructureValidator structureValidator,
    ISignatureValidator signatureValidator) : IETAMPValidator
{
    /// <summary>
    ///     Validates an ETAMP token against audience, issuer, and signature requirements.
    /// </summary>
    /// <param name="etamp">ETAMP token model to validate.</param>
    /// <param name="audience">Expected audience claim.</param>
    /// <param name="issuer">Expected issuer claim.</param>
    /// <param name="tokenSecurityKey">ECDsa security key for signature verification.</param>
    /// <returns>True if the ETAMP token is valid; otherwise, false.</returns>
    public async Task<bool> ValidateETAMP(ETAMPModel etamp, string audience, string issuer,
        ECDsaSecurityKey tokenSecurityKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.Token);
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.SignatureToken);
        return new List<bool>
        {
            structureValidator.ValidateETAMPStructure(etamp).IsValid,
            (await jwtValidator.ValidateToken(etamp.Token, audience, issuer, tokenSecurityKey)).IsValid,
            signatureValidator.ValidateToken(etamp.Token, etamp.SignatureToken),
            signatureValidator.ValidateETAMPMessage(etamp)
        }.TrueForAll(x => x);
    }

    /// <summary>
    ///     Validates the ETAMP token's structure, signature, and lifetime.
    /// </summary>
    /// <param name="etamp">ETAMP token model to validate.</param>
    /// <param name="tokenSecurityKey">ECDsa security key for lifetime validation.</param>
    /// <returns>True if the ETAMP token is valid; otherwise, false.</returns>
    public async Task<bool> ValidateETAMP(ETAMPModel etamp, ECDsaSecurityKey tokenSecurityKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.Token);
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.SignatureToken);
        return new List<bool>
        {
            structureValidator.ValidateETAMPStructure(etamp).IsValid,
            (await jwtValidator.ValidateLifeTime(etamp.Token, tokenSecurityKey)).IsValid,
            signatureValidator.ValidateToken(etamp.Token, etamp.SignatureToken),
            signatureValidator.ValidateETAMPMessage(etamp)
        }.TrueForAll(x => x);
    }

    /// <summary>
    ///     Performs a basic validation of the ETAMP token's structure and lifetime.
    /// </summary>
    /// <param name="etamp">ETAMP token model to validate.</param>
    /// <param name="tokenSecurityKey">ECDsa security key for lifetime validation.</param>
    /// <returns>True if the basic structure and lifetime of the ETAMP token are valid; otherwise, false.</returns>
    public async Task<bool> ValidateETAMPLite(ETAMPModel etamp, ECDsaSecurityKey tokenSecurityKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(etamp.Token);
        return new List<bool>
        {
            structureValidator.ValidateETAMPStructureLite(etamp).IsValid,
            (await jwtValidator.ValidateLifeTime(etamp.Token, tokenSecurityKey)).IsValid
        }.TrueForAll(x => x);
    }
}