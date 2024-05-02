#region

using ETAMPManagment.Models;

#endregion

namespace ETAMPManagment.Validators.Interfaces;

/// <summary>
///     Defines methods for validating the structure and consistency of ETAMP tokens and models.
/// </summary>
public interface IStructureValidator
{
    /// <summary>
    ///     Validates the format of an ETAMP token and returns the deserialized model if valid.
    /// </summary>
    /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
    /// <returns>The deserialized ETAMP model if the string is a valid JSON representation of an ETAMP.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the ETAMP string is invalid or cannot be deserialized into an ETAMP
    ///     model.
    /// </exception>
    ETAMPModel IsValidEtampFormat(string etamp);

    /// <summary>
    ///     Validates the consistency of identifiers within an ETAMP token.
    /// </summary>
    /// <param name="etamp">The ETAMP token as a JSON string to be validated for identifier consistency.</param>
    /// <returns><c>true</c> if the identifier is consistent; otherwise, <c>false</c>.</returns>
    bool ValidateIdConsistency(string etamp);

    /// <summary>
    ///     Validates the structure of an ETAMP token against the expected schema.
    /// </summary>
    /// <param name="etamp">The ETAMP token as a JSON string to be validated for structural integrity.</param>
    /// <returns>A ValidationResult indicating whether the structure is valid.</returns>
    ValidationResult ValidateETAMPStructure(string etamp);

    /// <summary>
    ///     Validates the structure of an ETAMP model against the expected schema.
    /// </summary>
    /// <param name="model">The ETAMPModel object to be validated for structural integrity.</param>
    /// <returns>A ValidationResult indicating whether the structure is valid.</returns>
    ValidationResult ValidateETAMPStructure(ETAMPModel model);

    /// <summary>
    ///     Performs a lightweight structure validation of an ETAMP token.
    /// </summary>
    /// <param name="etamp">The ETAMP token as a JSON string to be validated for basic structural integrity.</param>
    /// <returns>A ValidationResult indicating whether the basic structure is valid.</returns>
    ValidationResult ValidateETAMPStructureLite(string etamp);

    /// <summary>
    ///     Performs a lightweight structure validation of an ETAMP model.
    /// </summary>
    /// <param name="model">The ETAMPModel object to be validated for basic structural integrity.</param>
    /// <returns>A ValidationResult indicating whether the basic structure is valid.</returns>
    ValidationResult ValidateETAMPStructureLite(ETAMPModel model);
}