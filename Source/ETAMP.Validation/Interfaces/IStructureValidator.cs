#region

using ETAMP.Core.Models;

#endregion

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Defines methods for validating the structure and consistency of ETAMP tokens and models.
/// </summary>
public interface IStructureValidator
{
    /// <summary>
    ///     Validates the structure and consistency of an ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of token used in the ETAMPModel.</typeparam>
    /// <param name="model">The ETAMP model to be validated.</param>
    /// <param name="validateLite">Indicates whether to perform a lite validation (optional, default is false).</param>
    /// <returns>The validation result indicating whether the ETAMP model is valid or not, and any error message if applicable.</returns>
    ValidationResult ValidateETAMP<T>(ETAMPModel<T> model, bool validateLite = false) where T : Token;

    /// <summary>
    ///     Validates the structure and consistency of an ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of token used in the ETAMPModel.</typeparam>
    /// <param name="etampJson">The ETAMP JSON string to be validated.</param>
    /// <param name="validateLite">Indicates whether to perform a lite validation (optional, default is false).</param>
    /// <returns>The validation result indicating whether the ETAMP model is valid or not, and any error message if applicable.</returns>
    ValidationResult ValidateETAMP<T>(string etampJson, bool validateLite = false) where T : Token;
}