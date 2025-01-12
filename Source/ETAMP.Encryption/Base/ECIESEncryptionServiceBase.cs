#region

using ETAMP.Encryption.Helper;
using ETAMP.Encryption.Interfaces;

#endregion

namespace ETAMP.Encryption.Base;

/// <summary>
///     This is an abstract base class for ECIES (Elliptic Curve Integrated Encryption Scheme)
///     encryption service. It provides the basic functionality for encrypting and decrypting messages.
/// </summary>
public abstract class ECIESEncryptionServiceBase : CheckInitialize, IECIESEncryptionService
{
    /// <summary>
    ///     Represents a base class for all key exchangers.
    /// </summary>
    protected KeyExchangerBase? KeyExchanger { get; private set; }

    /// <summary>
    ///     Provides encryption and decryption services.
    /// </summary>
    protected IEncryptionService? EncryptionService { get; private set; }

    /// <summary>
    ///     Encrypts the given message using ECIES encryption algorithm.
    /// </summary>
    /// <param name="message">The message to encrypt.</param>
    /// <returns>The encrypted message.</returns>
    public abstract string? Encrypt(string message);

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>The decrypted plain text message.</returns>
    public abstract string Decrypt(string? encryptedMessageBase64);

    /// <summary>
    ///     Initializes the ECIES encryption service with the necessary key exchange and encryption components.
    /// </summary>
    /// <param name="keyExchanger">The key exchanger used for deriving the shared secret.</param>
    /// <param name="encryptionService">The underlying encryption service used for encrypting and decrypting the message.</param>
    public void Initialize(KeyExchangerBase keyExchanger, IEncryptionService encryptionService)
    {
        Init = true;
        KeyExchanger = keyExchanger ?? throw new ArgumentNullException(nameof(keyExchanger));
        EncryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }
}