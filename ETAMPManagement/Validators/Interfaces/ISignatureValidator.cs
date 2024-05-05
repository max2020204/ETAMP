#region

using ETAMPManagement.Models;

#endregion

namespace ETAMPManagement.Validators.Interfaces;

/// <summary>
///     Defines methods for validating the signatures of tokens and ETAMP messages.
/// </summary>
public interface ISignatureValidator
{
    /// <summary>
    ///     Validates the signature of a given token.
    /// </summary>
    /// <param name="token">The token whose signature is to be validated.</param>
    /// <param name="tokenSignature">The signature of the token.</param>
    /// <returns><c>true</c> if the token's signature is valid; otherwise, <c>false</c>.</returns>
    bool ValidateToken(string token, string tokenSignature);

    /// <summary>
    ///     Validates the integrity and authenticity of an ETAMP message given as a JSON string.
    /// </summary>
    /// <param name="etamp">The ETAMP message in JSON string format to be validated.</param>
    /// <returns><c>true</c> if the ETAMP message is valid; otherwise, <c>false</c>.</returns>
    bool ValidateETAMPMessage(string etamp);

    /// <summary>
    ///     Validates the integrity and authenticity of an ETAMP message encapsulated in an ETAMPModel object.
    /// </summary>
    /// <param name="etamp">The ETAMPModel object representing the ETAMP message to be validated.</param>
    /// <returns><c>true</c> if the ETAMP message is valid; otherwise, <c>false</c>.</returns>
    bool ValidateETAMPMessage(ETAMPModel etamp);
}