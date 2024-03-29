﻿using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Base.Interfaces
{
    /// <summary>
    /// Extends IETAMPData to include methods for creating ETAMP tokens and models, supporting encryption and digital signing.
    /// </summary>
    public interface IETAMPBase : IETAMPData
    {
        /// <summary>
        /// Creates a serialized ETAMP token with the specified update type and payload.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP token.</param>
        /// <param name="payload">The payload to be included in the ETAMP token.</param>
        /// <param name="version">The version of the ETAMP protocol.</param>
        /// <returns>A serialized ETAMP token as a json string.</returns>
        string CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePayload;

        /// <summary>
        /// Creates an ETAMP model with the specified update type and payload.
        /// </summary>
        /// <typeparam name="T">The payload type.</typeparam>
        /// <param name="updateType">The update type identifier for the ETAMP model.</param>
        /// <param name="payload">The payload to be included in the ETAMP model.</param>
        /// <param name="version">The version of the ETAMP protocol.</param>
        /// <returns>An ETAMP model instance.</returns>
        ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePayload;
    }
}