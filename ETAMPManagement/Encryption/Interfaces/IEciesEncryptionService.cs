namespace ETAMPManagement.Encryption.Interfaces;

/// <summary>
///     Defines a service for encrypting and decrypting messages using Elliptic Curve Integrated Encryption Scheme (ECIES).
/// </summary>
public interface IEciesEncryptionService
{
    /// <summary>
    ///     Initializes the ECIES encryption service with the necessary key exchange and encryption components.
    /// </summary>
    /// <param name="keyExchanger">The key exchanger used for deriving the shared secret.</param>
    /// <param name="encryptionService">The underlying encryption service used for encrypting and decrypting the message.</param>
    void Initialize(IKeyExchanger keyExchanger, IEncryptionService encryptionService);

    /// <summary>
    ///     Encrypts a plain text message using ECIES.
    /// </summary>
    /// <param name="message">The plain text message to be encrypted.</param>
    /// <returns>The encrypted message as a Base64-encoded string.</returns>
    string? Encrypt(string message);

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>The decrypted plain text message.</returns>
    string Decrypt(string? encryptedMessageBase64);
}