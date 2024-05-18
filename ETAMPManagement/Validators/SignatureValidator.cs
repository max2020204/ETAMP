#region

using ETAMPManagement.Models;
using ETAMPManagement.Validators.Interfaces;
using ETAMPManagement.Wrapper.Interfaces;

#endregion

namespace ETAMPManagement.Validators;

/// <summary>
///     Validates the signature of ETAMP messages.
/// </summary>
public sealed class SignatureValidator : ISignatureValidator
{
    /// <summary>
    ///     Validates the structure and consistency of ETAMP tokens and models.
    /// </summary>
    private readonly IStructureValidator _structureValidator;

    /// <summary>
    ///     This class is responsible for validating signatures in ETAMP messages.
    /// </summary>
    private readonly IVerifyWrapper _verifyWrapper;

    /// <summary>
    ///     SignatureValidator is a class that implements the ISignatureValidator interface.
    ///     It is responsible for validating signatures in ETAMP messages.
    /// </summary>
    public SignatureValidator(IVerifyWrapper verifyWrapper, IStructureValidator structureValidator)
    {
        _verifyWrapper = verifyWrapper
                         ?? throw new ArgumentNullException(nameof(verifyWrapper));
        _structureValidator = structureValidator
                              ?? throw new ArgumentNullException(nameof(structureValidator));
    }

    /// <summary>
    ///     Validates the ETAMP message by verifying its signature.
    /// </summary>
    /// <typeparam name="T">The type of the token used in the ETAMP model.</typeparam>
    /// <param name="etamp">The ETAMP model to be validated.</param>
    /// <returns>ValidationResult indicating whether the ETAMP message is valid or not.</returns>
    public ValidationResult ValidateETAMPMessage<T>(ETAMPModel<T> etamp) where T : Token
    {
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp);
        if (!structureValidationResult.IsValid)
            return new ValidationResult(false, "Invalid ETAMP Model.");

        if (string.IsNullOrWhiteSpace(etamp.SignatureMessage))
            return new ValidationResult(false, "SignatureMessage is missing in the ETAMP model.");

        var dataToVerify = $"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}";
        var isVerified = _verifyWrapper.VerifyData(dataToVerify, etamp.SignatureMessage);

        return isVerified ? new ValidationResult(true) : new ValidationResult(false, "Failed to verify data.");
    }

    /// <summary>
    ///     Validates the given token and its signature.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <param name="tokenSignature">The signature of the token.</param>
    /// <returns>True if the token and its signature are valid; otherwise, false.</returns>
    public bool ValidateToken(string token, string tokenSignature)
    {
        return _verifyWrapper.VerifyData(token, tokenSignature);
    }
}