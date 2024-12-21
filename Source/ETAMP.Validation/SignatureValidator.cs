using ETAMP.Core.Models;
using ETAMP.Validation.Base;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper.Base;
using Newtonsoft.Json;

namespace ETAMP.Validation;

/// <summary>
///     Validates the signature of ETAMP messages.
/// </summary>
public sealed class SignatureValidator : SignatureValidatorBase
{
    /// <summary>
    ///     Validates the structure and consistency of ETAMP tokens and models.
    /// </summary>
    private readonly IStructureValidator _structureValidator;


    /// <summary>
    ///     SignatureValidator is a class that implements the ISignatureValidator interface.
    ///     It is responsible for validating signatures in ETAMP messages.
    /// </summary>
    public SignatureValidator(VerifyWrapperBase wrapperBase, IStructureValidator structureValidator)
        : base(wrapperBase)
    {
        _structureValidator = structureValidator
                              ?? throw new ArgumentNullException(nameof(structureValidator));
    }


    /// <summary>
    ///     Validates an ETAMP message.
    /// </summary>
    /// <typeparam name="T">The type of the token, which must derive from <see cref="Token" />.</typeparam>
    /// <param name="etamp">The ETAMP model to validate.</param>
    /// <returns>
    ///     A <see cref="ValidationResult" /> indicating whether the ETAMP message is valid.
    ///     If invalid, it includes an error message.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="etamp" /> is null.</exception>
    public override ValidationResult ValidateETAMPMessage<T>(ETAMPModel<T> etamp)
    {
        ArgumentNullException.ThrowIfNull(etamp);
        ArgumentNullException.ThrowIfNull(etamp.Token);
        var structureValidationResult = _structureValidator.ValidateETAMP(etamp);
        if (!structureValidationResult.IsValid)
            return new ValidationResult(false, "Invalid ETAMP Model.");

        if (string.IsNullOrWhiteSpace(etamp.SignatureMessage))
            return new ValidationResult(false, "SignatureMessage is missing in the ETAMP model.");
        var token = JsonConvert.SerializeObject(etamp.Token);
        var dataToVerify = $"{etamp.Id}{etamp.Version}{token}{etamp.UpdateType}{etamp.CompressionType}";
        var isVerified = VerifyWrapper.VerifyData(dataToVerify, etamp.SignatureMessage);

        return isVerified ? new ValidationResult(true) : new ValidationResult(false, "Failed to verify data.");
    }
}