#region

using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Validation.Interfaces;

#endregion

namespace ETAMP.Validation;

/// <summary>
///     Provides functionality to validate the structure of an ETAMP model.
/// </summary>
public sealed class StructureValidator : IStructureValidator
{
    private readonly ICompressionServiceFactory _compressionServiceFactory;

    public StructureValidator(ICompressionServiceFactory compressionServiceFactory)
    {
        _compressionServiceFactory = compressionServiceFactory;
    }

    /// <summary>
    ///     Validates an ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of token.</typeparam>
    /// <param name="model">The ETAMP model to validate.</param>
    /// <param name="validateLite">Optional. Specifies whether to perform lite validation. Default is false.</param>
    /// <returns>A ValidationResult object indicating whether the model is valid or not.</returns>
    public ValidationResult ValidateETAMP<T>(ETAMPModel<T>? model, bool validateLite = false) where T : Token
    {
        if (model == null)
            return new ValidationResult(false, "ETAMP model is null.");

        if (model.Id == Guid.Empty ||
            string.IsNullOrWhiteSpace(model.UpdateType) ||
            string.IsNullOrWhiteSpace(model.CompressionType) ||
            model.Token == null ||
            (!validateLite && string.IsNullOrWhiteSpace(model.SignatureMessage)))
            return new ValidationResult(false, "ETAMP model has empty/missing fields or contains invalid values.");

        return new ValidationResult(true);
    }


    /// <summary>
    ///     Validates an ETAMP model using its JSON representation.
    /// </summary>
    /// <typeparam name="T">The type of token.</typeparam>
    /// <param name="etampJson">The JSON representation of the ETAMP model to validate.</param>
    /// <param name="validateLite">Optional. Specifies whether to perform lite validation. Default is false.</param>
    /// <returns>A ValidationResult object indicating whether the ETAMP model is valid or not.</returns>
    public async Task<ValidationResult> ValidateETAMPAsync<T>(string etampJson, bool validateLite = false)
        where T : Token
    {
        var model = await etampJson.DeconstructETAMPAsync<T>(_compressionServiceFactory);
        return ValidateETAMP(model, validateLite);
    }
}