#region

using System.Security.Cryptography;
using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;

#endregion

namespace ETAMP.Validation;

/// <summary>
///     Class for validating ETAMP objects.
/// </summary>
public sealed class ETAMPValidator : IETAMPValidator
{
    private readonly ISignatureValidator _signuture;
    private readonly IStructureValidator _structureValidator;
    private readonly ITokenValidator _tokenValidator;


    /// <summary>
    ///     ETAMP Validator class.
    /// </summary>
    public ETAMPValidator(ITokenValidator tokenValidator, IStructureValidator structureValidator,
        ISignatureValidator signuture)
    {
        _tokenValidator = tokenValidator;
        _structureValidator = structureValidator;
        _signuture = signuture;
    }

    /// <summary>
    ///     Validates the ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of Token.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <param name="validateLite">Indicates whether to perform lite validation.</param>
    /// <returns>A ValidationResult indicating the validation result.</returns>
    public async Task<ValidationResult> ValidateETAMPAsync<T>(ETAMPModel<T> etamp, bool validateLite) where T : Token
    {
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp, validateLite);
        if (!structureValidationResult.IsValid)
            return structureValidationResult;

        var tokenValidationResult = _tokenValidator.ValidateToken(etamp);
        if (!tokenValidationResult.IsValid)
            return tokenValidationResult;


        var signatureValidationResult = await _signuture.ValidateETAMPMessageAsync(etamp);

        return !signatureValidationResult.IsValid
            ? signatureValidationResult
            : new ValidationResult(true);
    }

    public void Dispose()
    {
        _signuture.Dispose();
    }

    public void Initialize(ECDsa provider, HashAlgorithmName algorithmName)
    {
        _signuture.Initialize(provider, algorithmName);
    }
}