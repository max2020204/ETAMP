#region

using ETAMP.Core.Models;

#endregion

namespace ETAMP.Wrapper.Interfaces;

/// <summary>
///     Provides functionality for signing ETAMP messages.
/// </summary>
public interface ISignWrapper
{
    /// <summary>
    ///     Signs the provided ETAMP model by generating a signature for the model's token and message.
    /// </summary>
    /// <typeparam name="T">The type of the token contained in the ETAMP model.</typeparam>
    /// <param name="etamp">The ETAMP model to sign.</param>
    /// <returns>The signed ETAMP model with the generated signature for the token and message.</returns>
    ETAMPModel<T> SignEtampModel<T>(ETAMPModel<T> etamp) where T : Token;
}