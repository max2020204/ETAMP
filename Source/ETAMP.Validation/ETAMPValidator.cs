using ETAMP.Core.Models;
using ETAMP.Validation.Base;
using ETAMP.Validation.Interfaces;

namespace ETAMP.Validation;

/// <summary>
///     Class for validating ETAMP objects.
/// </summary>
public sealed class ETAMPValidator : ETAMPValidatorBase
{
    private readonly IStructureValidator _structureValidator;
    private readonly ITokenValidator _tokenValidator;

    /// <summary>
    ///     ETAMP Validator class.
    /// </summary>
    public ETAMPValidator(ITokenValidator tokenValidator, IStructureValidator structureValidator,
        SignatureValidatorBase signatureValidatorBase) : base(signatureValidatorBase)
    {
        _tokenValidator = tokenValidator;
        _structureValidator = structureValidator;
    }

    /// <summary>
    ///     Validates the ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of Token.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <param name="validateLite">Indicates whether to perform lite validation.</param>
    /// <returns>A ValidationResult indicating the validation result.</returns>
    public override ValidationResult ValidateETAMP<T>(ETAMPModel<T> etamp, bool validateLite)
    {
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp, validateLite);
        if (!structureValidationResult.IsValid)
            return structureValidationResult;

        var tokenValidationResult = _tokenValidator.ValidateToken(etamp);
        if (!tokenValidationResult.IsValid)
            return tokenValidationResult;


        var signatureValidationResult = signutureValidatorAbstract.ValidateETAMPMessage(etamp);

        return !signatureValidationResult.IsValid
            ? signatureValidationResult
            : new ValidationResult(true);
    }
}