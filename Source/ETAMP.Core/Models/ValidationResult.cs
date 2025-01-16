namespace ETAMP.Core.Models;

/// <summary>
///     Represents the result of a validation process.
/// </summary>
public class ValidationResult
{
    /// <summary>
    ///     Represents the result of a validation process.
    /// </summary>
    public ValidationResult(bool isValid, string errorMessage = "")
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///     Gets a value indicating whether the validation result represents a successful validation.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the validation result is successful; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid { get; init; }

    /// <summary>
    ///     Gets the error message associated with a validation failure.
    /// </summary>
    /// <value>
    ///     A string containing the error message when the validation fails; otherwise, an empty string if no error exists.
    /// </value>
    public string ErrorMessage { get; init; }

    /// <summary>
    ///     Gets or sets the exception associated with a validation failure, if one occurred.
    /// </summary>
    /// <value>
    ///     The exception related to the validation failure, or <c>null</c> if no exception occurred.
    /// </value>
    public Exception Exception { get; set; }


    /// <summary>
    ///     Creates a successful instance of the <see cref="ValidationResult" /> class.
    /// </summary>
    /// <returns>A <see cref="ValidationResult" /> instance indicating success.</returns>
    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    /// <summary>
    ///     Creates a <see cref="ValidationResult" /> instance representing a failed validation.
    /// </summary>
    /// <param name="errorMessage">The error message describing the reason for validation failure.</param>
    /// <param name="exception">An optional exception associated with the validation failure.</param>
    /// <returns>A <see cref="ValidationResult" /> instance indicating a failure state.</returns>
    public static ValidationResult Fail(string errorMessage, Exception exception = null)
    {
        return new ValidationResult(false, errorMessage)
        {
            Exception = exception
        };
    }
}