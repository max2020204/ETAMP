namespace ETAMPManagment.Encryption.Interfaces
{
    /// <summary>
    /// Defines a service for encrypting and decrypting messages using Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    public interface IEciesEncryptionService
    {
        /// <summary>
        /// Encrypts a plain text message using ECIES.
        /// </summary>
        /// <param name="message">The plain text message to be encrypted.</param>
        /// <returns>The encrypted message as a Base64-encoded string.</returns>
        string Encrypt(string message);

        /// <summary>
        /// Decrypts an encrypted message back to its plain text form using ECIES.
        /// </summary>
        /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
        /// <param name="privateKey">The private key used for decryption, matching the public key used during encryption.</param>
        /// <returns>The decrypted plain text message.</returns>
        string Decrypt(string encryptedMessageBase64, byte[] privateKey);
    }
}