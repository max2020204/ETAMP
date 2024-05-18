#region

using ETAMPManagement.Models;
using ETAMPManagement.Validators.Interfaces;

#endregion

namespace ETAMPManagement.Validators;

/// <summary>
///     Class for validating ETAMP objects.
/// </summary>
public sealed class ETAMPValidator : IETAMPValidator
{
    private readonly ISignatureValidator _signatureValidator;
    private readonly IStructureValidator _structureValidator;
    private readonly ITokenValidator _tokenValidator;

    /// <summary>
    ///     ETAMP Validator class.
    /// </summary>
    public ETAMPValidator(ITokenValidator tokenValidator, IStructureValidator structureValidator,
        ISignatureValidator signatureValidator)
    {
        _tokenValidator = tokenValidator;
        _structureValidator = structureValidator;
        _signatureValidator = signatureValidator;
    }

    /// <summary>
    ///     Validates the ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of Token.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <param name="validateLite">Indicates whether to perform lite validation.</param>
    /// <returns>A ValidationResult indicating the validation result.</returns>
    public ValidationResult ValidateETAMP<T>(ETAMPModel<T> etamp, bool validateLite) where T : Token
    {
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp, validateLite);
        if (!structureValidationResult.IsValid)
            return structureValidationResult;

        var tokenValidationResult = _tokenValidator.ValidateToken(etamp);
        if (!tokenValidationResult.IsValid)
            return tokenValidationResult;


        var signatureValidationResult = _signatureValidator.ValidateETAMPMessage(etamp);

        return !signatureValidationResult.IsValid
            ? signatureValidationResult
            : new ValidationResult(true);
    }
}