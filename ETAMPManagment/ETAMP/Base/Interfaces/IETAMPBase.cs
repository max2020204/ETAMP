using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;

namespace ETAMPManagment.ETAMP.Base.Interfaces
{
    /// <summary>
    /// Extends IETAMPData to include methods for creating ETAMP tokens and models, supporting dynamic encryption and digital signing capabilities.
    /// </summary>
    public interface IETAMPBase
    {
        /// <summary>
        /// Creates an ETAMP model with the specified update type, payload, and optionally, a custom signing credentials provider.
        /// </summary>
        /// <typeparam name="T">The type of the payload to be included in the ETAMP model.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model, defining the context or the nature of the update.</param>
        /// <param name="payload">The payload to be included in the ETAMP model. This is the main content of the ETAMP message.</param>
        /// <param name="version">The version of the ETAMP protocol, defaulting to 1. This helps in maintaining compatibility across different versions of ETAMP protocol implementations.</param>
        /// <param name="provider">Optional. A custom provider for signing credentials. If provided, it allows the use of a specific digital signature algorithm for this particular ETAMP model creation. If null, the default system-configured signing credentials provider is used.</param>
        /// <returns>An instance of ETAMPModel that encapsulates the provided data, signed with the specified or default digital signing method.</returns>
        ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1, ISigningCredentialsProvider? provider = null) where T : BasePayload;
    }
}