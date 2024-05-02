#region

using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

#endregion

namespace ETAMPManagment.ETAMP.Base.Interfaces;

/// <summary>
///     Interface for ETAMP data processing, providing a method to create digitally signed ETAMP token data.
/// </summary>
public interface IETAMPData
{
    /// <summary>
    ///     Creates ETAMP token data with a digital signature for the specified payload and message ID. This method allows for
    ///     optional specification of a signing credentials provider.
    /// </summary>
    /// <typeparam name="T">The type of the payload to be included in the ETAMP token.</typeparam>
    /// <param name="messageId">The message identifier, which uniquely identifies the ETAMP token.</param>
    /// <param name="payload">
    ///     The payload to be included in the ETAMP token. This payload forms the content of the token which
    ///     is signed to ensure integrity and authenticity.
    /// </param>
    /// <param name="provider">
    ///     Optional. A custom provider for signing credentials. If provided, it allows for the use of a
    ///     specific digital signature algorithm for this particular ETAMP token creation. If null, the default
    ///     system-configured signing credentials provider is used.
    /// </param>
    /// <returns>
    ///     A string representing the serialized and signed ETAMP token data, ensuring that the token is tamper-proof and
    ///     verifiable.
    /// </returns>
    string CreateEtampData<T>(string messageId, T payload, ISigningCredentialsProvider? provider = null)
        where T : BasePayload;
}