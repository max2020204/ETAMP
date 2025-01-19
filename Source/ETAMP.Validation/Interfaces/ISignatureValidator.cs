#region

using ETAMP.Core.Models;

#endregion

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Interface for validating signatures in ETAMP messages.
/// </summary>
public interface ISignatureValidator
{
    /// <summary>
    ///     Validates an ETAMP message and its signature.
    /// </summary>
    /// <typeparam name="T">The type of the token encapsulated in the ETAMP message.</typeparam>
    /// <param name="etamp">The ETAMP message to be validated, including its signature information and token.</param>
    /// <returns>
    ///     A <see cref="ValidationResult" /> object representing whether the message is valid and any associated error
    ///     details.
    /// </returns>
    Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp) where T : Token;
}