#region

using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;
using ETAMP.Validation.Base;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper.Base;

#endregion

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


    public override async Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp)
    {
        ArgumentNullException.ThrowIfNull(etamp);
        ArgumentNullException.ThrowIfNull(etamp.Token);

        var structureValidationResult = _structureValidator.ValidateETAMP(etamp);
        if (!structureValidationResult.IsValid)
            return new ValidationResult(false, "Invalid ETAMP Model.");

        if (string.IsNullOrWhiteSpace(etamp.SignatureMessage))
            return new ValidationResult(false, "SignatureMessage is missing in the ETAMP model.");

        string token;
        await using (var memoryStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(memoryStream, etamp.Token);
            token = Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        var dataToVerify = $"{etamp.Id}{etamp.Version}{token}{etamp.UpdateType}{etamp.CompressionType}";

        // Асинхронная проверка подписи, если VerifyWrapper поддерживает её
        var isVerified = VerifyWrapper.VerifyData(dataToVerify, etamp.SignatureMessage);

        return isVerified ? new ValidationResult(true) : new ValidationResult(false, "Failed to verify data.");
    }
}