#region

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ETAMP.Core.Models;
using ETAMP.Validation.Interfaces;
using ETAMP.Wrapper.Interfaces;

#endregion

namespace ETAMP.Validation;

/// <summary>
///     Validates the signature of ETAMP messages.
/// </summary>
public sealed class SignatureValidator : ISignatureValidator
{
    /// <summary>
    ///     Validates the structure and consistency of ETAMP tokens and models.
    /// </summary>
    private readonly IStructureValidator _structureValidator;

    private readonly IVerifyWrapper _verifyWrapper;

    private ECDsa _ecdsa;


    /// <summary>
    ///     SignatureValidatorValidator is a class that implements the ISignatureValidator interface.
    ///     It is responsible for validating signatures in ETAMP messages.
    /// </summary>
    public SignatureValidator(IVerifyWrapper verifyWrapper, IStructureValidator structureValidator)
    {
        _verifyWrapper = verifyWrapper;
        _structureValidator = structureValidator
                              ?? throw new ArgumentNullException(nameof(structureValidator));
    }


    public async Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp) where T : Token
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
        var isVerified = _verifyWrapper.VerifyData(dataToVerify, etamp.SignatureMessage);

        return isVerified ? new ValidationResult(true) : new ValidationResult(false, "Failed to verify data.");
    }

    public void Initialize(ECDsa provider, HashAlgorithmName algorithmName)
    {
        _ecdsa = provider;
        _verifyWrapper.Initialize(provider, algorithmName);
    }

    public void Dispose()
    {
        _verifyWrapper.Dispose();
        _ecdsa.Dispose();
    }
}