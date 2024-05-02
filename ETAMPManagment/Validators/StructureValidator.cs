#region

using System.IdentityModel.Tokens.Jwt;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Newtonsoft.Json;

#endregion

namespace ETAMPManagment.Validators;

/// <summary>
///     Validates the structure of ETAMP messages and tokens to ensure their conformity to expected formats and standards.
/// </summary>
public sealed class StructureValidator : IStructureValidator
{
    private readonly IJwtValidator _jwtValidator;

    public StructureValidator(IJwtValidator jwtValidator)
    {
        _jwtValidator = jwtValidator ?? throw new ArgumentNullException(nameof(jwtValidator));
    }

    /// <summary>
    ///     Validates the format of a given ETAMP string to ensure it's a valid JSON and can be deserialized into an ETAMP
    ///     model.
    /// </summary>
    /// <param name="etamp">The ETAMP string to validate.</param>
    /// <returns>The deserialized ETAMP model if the string is a valid JSON representation of an ETAMP.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the ETAMP string is invalid or cannot be deserialized into an ETAMP
    ///     model.
    /// </exception>
    public ETAMPModel IsValidEtampFormat(string etamp)
    {
        ArgumentException.ThrowIfNullOrEmpty(etamp);

        ETAMPModel? model;
        try
        {
            model = JsonConvert.DeserializeObject<ETAMPModel>(etamp);
            if (model == null || model.Id == Guid.Empty)
                throw new ArgumentException("Failed to deserialize ETAMP to model", etamp);
            return model;
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("Invalid JSON ETAMP format", etamp, ex);
        }
    }

    /// <summary>
    ///     Validates the structure of a given ETAMP string to ensure all required fields are present and valid.
    /// </summary>
    /// <param name="etamp">The ETAMP string to validate.</param>
    /// <returns>A ValidationResult indicating whether the ETAMP structure is valid.</returns>
    public ValidationResult ValidateETAMPStructure(string etamp)
    {
        var model = IsValidEtampFormat(etamp);
        if (model == null ||
            string.IsNullOrWhiteSpace(model.Token) ||
            string.IsNullOrWhiteSpace(model.UpdateType) ||
            string.IsNullOrWhiteSpace(model.SignatureToken) ||
            string.IsNullOrWhiteSpace(model.SignatureMessage))
            return new ValidationResult(false,
                "Deserialized ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values");
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Validates the structure of an ETAMP model to ensure all required fields are present and valid.
    /// </summary>
    /// <param name="model">The ETAMP model to validate.</param>
    /// <returns>A ValidationResult indicating whether the ETAMP model structure is valid.</returns>
    public ValidationResult ValidateETAMPStructure(ETAMPModel model)
    {
        if (model == null ||
            model.Id == Guid.Empty ||
            string.IsNullOrWhiteSpace(model.Token) ||
            string.IsNullOrWhiteSpace(model.UpdateType) ||
            string.IsNullOrWhiteSpace(model.SignatureToken) ||
            string.IsNullOrWhiteSpace(model.SignatureMessage))
            return new ValidationResult(false,
                "ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values");
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Validates a simplified structure of a given ETAMP string, focusing on essential fields only.
    /// </summary>
    /// <param name="etamp">The ETAMP string to validate.</param>
    /// <returns>A ValidationResult indicating whether the simplified ETAMP structure is valid.</returns>
    public ValidationResult ValidateETAMPStructureLite(string etamp)
    {
        var model = IsValidEtampFormat(etamp);
        if (model == null ||
            string.IsNullOrWhiteSpace(model.Token) ||
            string.IsNullOrWhiteSpace(model.UpdateType))
            return new ValidationResult(false,
                "Deserialized ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values.");
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Validates a simplified structure of an ETAMP model, focusing on essential fields only.
    /// </summary>
    /// <param name="model">The ETAMP model to validate.</param>
    /// <returns>A ValidationResult indicating whether the simplified ETAMP model structure is valid.</returns>
    public ValidationResult ValidateETAMPStructureLite(ETAMPModel model)
    {
        if (model == null ||
            model.Id == Guid.Empty ||
            string.IsNullOrWhiteSpace(model.Token) ||
            string.IsNullOrWhiteSpace(model.UpdateType))
            return new ValidationResult(false,
                "ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values");
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Validates the consistency of the ETAMP ID across its components.
    /// </summary>
    /// <param name="etamp">The ETAMP string to validate.</param>
    /// <returns>True if the ETAMP ID is consistent, false otherwise.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the JWT validator is not initialized.</exception>
    public bool ValidateIdConsistency(string etamp)
    {
        var model = IsValidEtampFormat(etamp);
        if (_jwtValidator.IsValidJwtToken(model.Token).IsValid)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            var tokenData = jwtSecurityTokenHandler.ReadJwtToken(model.Token).Payload.ToDictionary();
            return tokenData.ContainsKey("MessageId") && tokenData["MessageId"]!.ToString()
                .Equals(model.Id.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }

        return false;
    }
}