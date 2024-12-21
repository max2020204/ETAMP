using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Validation.Interfaces;

namespace ETAMP.Validation;

/// <summary>
///     Represents a validator for an ETAMP model structure.
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
    ///     Validates the structure and consistency of an ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of token used in the ETAMPModel.</typeparam>
    /// <param name="etampJson">The ETAMP JSON string to be validated.</param>
    /// <param name="validateLite">Indicates whether to perform a lite validation (optional, default is false).</param>
    /// <returns>
    ///     The validation result indicating whether the ETAMP model is valid or not, and any error message if applicable.
    /// </returns>
    public ValidationResult ValidateETAMP<T>(string etampJson, bool validateLite = false) where T : Token
    {
        var model = etampJson.DeconstructETAMP<T>(_compressionServiceFactory);
        return ValidateETAMP(model, validateLite);
    }
}