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
    public abstract Task<string> EncryptAsync(MemoryStream message);

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>The decrypted plain text message.</returns>
    public abstract Task<string> DecryptAsync(MemoryStream? encryptedMessageBase64);


    /// <summary>
    /// Initializes the encryption service with the specified key exchanger and encryption service implementation.
    /// </summary>
    /// <param name="keyExchanger">The key exchanger to use for key derivation and exchange operations.</param>
    /// <param name="encryptionService">The encryption service to use for performing encryption and decryption operations.</param>
    /// <exception cref="InvalidOperationException">Thrown if the service is already initialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyExchanger"/> or <paramref name="encryptionService"/> is null.</exception>
    public void Initialize(KeyExchangerBase keyExchanger, IEncryptionService encryptionService)
    {
        if (Init) throw new InvalidOperationException("Service is already initialized.");

        Init = true;
        KeyExchanger = keyExchanger ?? throw new ArgumentNullException(nameof(keyExchanger));
        EncryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    ///     Validates the presence of a shared secret. Can be overridden for custom validation logic.
    /// </summary>
    protected virtual void ValidateSecret()
    {
        if (KeyExchanger!.GetSharedSecret() == null || KeyExchanger!.GetSharedSecret()!.Length == 0)
            throw new InvalidOperationException(
                "KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");
    }
}