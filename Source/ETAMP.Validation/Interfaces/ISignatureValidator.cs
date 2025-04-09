using ETAMP.Core.Interfaces;
using ETAMP.Core.Models;

namespace ETAMP.Validation.Interfaces;

/// <summary>
///     Interface for validating signatures in ETAMP messages.
/// </summary>
public interface ISignatureValidator : IInitialize
{
    /// <summary>
    ///     Asynchronously validates the signature of an ETAMP model.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of token associated with the ETAMP model. Must inherit from <see cref="Token" />.
    /// </typeparam>
    /// <param name="etamp">
    ///     The ETAMP model containing the signature to validate.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to signal the asynchronous operation should be canceled. Defaults to
    ///     <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValidationResult" /> indicating whether the signature validation succeeded or failed.
    /// </returns>
    Task<ValidationResult> ValidateETAMPSignatureAsync<T>(ETAMPModel<T> etamp,
        CancellationToken cancellationToken = default) where T : Token;
}