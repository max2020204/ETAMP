#region

using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;

#endregion

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Interface for validating signatures in ETAMP messages.
/// </summary>
public interface ISignatureValidator : IInitialize
{
    /// <summary>
    ///     Validates the signature of an ETAMP message asynchronously.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the token contained within the ETAMP model, which must derive from the
    ///     <see cref="Token" /> class.
    /// </typeparam>
    /// <param name="etamp">The ETAMP model containing the message and token to be validated.</param>
    /// <returns>
    ///     A <see cref="ValidationResult" /> containing the result of the validation, indicating whether the signature is
    ///     valid and any associated error or exception details.
    /// </returns>
    Task<ValidationResult> ValidateETAMPMessageAsync<T>(ETAMPModel<T> etamp) where T : Token;
}