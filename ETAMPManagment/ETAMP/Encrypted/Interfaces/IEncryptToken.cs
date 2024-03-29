﻿using ETAMPManagment.Models;

namespace ETAMPManagment.ETAMP.Encrypted.Interfaces
{
    /// <summary>
    /// Provides functionality for encrypting ETAMP tokens and their payloads.
    /// </summary>
    public interface IEncryptToken
    {
        /// <summary>
        /// Encrypts a serialized ETAMP token string.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representation of an ETAMP token to be encrypted.</param>
        /// <returns>A string representing the encrypted ETAMP token.</returns>
        string EncryptETAMPToken(string jsonEtamp);

        /// <summary>
        /// Encrypts an ETAMP token and returns the encrypted token as an ETAMPModel.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representation of an ETAMP token to be encrypted.</param>
        /// <returns>An ETAMPModel instance containing the encrypted token and additional data.</returns>
        ETAMPModel EncryptETAMP(string jsonEtamp);
    }
}