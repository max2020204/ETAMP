namespace ETAMP.Core.Models;

/// <summary>
///     Represents the result of a validation process.
/// </summary>
public class ValidationResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ValidationResult" /> class.
    /// </summary>
    /// <param name="isValid">Indicates whether the validation is successful.</param>
    /// <param name="errorMessage">The error message associated with a validation failure, which is empty by default.</param>
    public ValidationResult(bool isValid, string errorMessage = "")
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///     Gets a value indicating whether the validation is successful.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the validation is successful; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid { get; init; }

    /// <summary>
    ///     Gets the error message associated with a validation failure.
    /// </summary>
    /// <value>
    ///     The error message, which is empty if the validation is successful.
    /// </value>
    public string ErrorMessage { get; init; }
}