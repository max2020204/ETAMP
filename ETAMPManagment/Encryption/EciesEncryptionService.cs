using System.Text;
using ETAMPManagment.Encryption.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Encryption;

/// <summary>
///     Implements the Elliptic Curve Integrated Encryption Scheme (ECIES) for encrypting and decrypting messages.
/// </summary>
public class EciesEncryptionService : IEciesEncryptionService
{
    private IEncryptionService? _encryptionService;
    private IKeyExchanger? _keyExchanger;

    /// <summary>
    ///     Initializes the ECIES encryption service with the necessary key exchange and encryption components.
    /// </summary>
    /// <param name="keyExchanger">The key exchanger used for deriving the shared secret.</param>
    /// <param name="encryptionService">The underlying encryption service used for encrypting and decrypting the message.</param>
    public void Initialize(IKeyExchanger keyExchanger, IEncryptionService encryptionService)
    {
        _keyExchanger = keyExchanger ?? throw new ArgumentNullException(nameof(keyExchanger));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    ///     Encrypts a plain text message using ECIES.
    /// </summary>
    /// <param name="message">The plain text message to be encrypted.</param>
    /// <returns>The encrypted message as a Base64-encoded string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the shared secret is not initialized or empty.</exception>
    public virtual string Encrypt(string message)
    {
        if (_keyExchanger!.GetSharedSecret() == null || _keyExchanger!.GetSharedSecret()!.Length == 0)
            throw new InvalidOperationException(
                "KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");

        var encryptedMessage = _encryptionService!.Encrypt(Encoding.UTF8.GetBytes(message),
            _keyExchanger!.GetSharedSecret()!, _encryptionService.IV);

        return Base64UrlEncoder.Encode(encryptedMessage);
    }

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>The decrypted plain text message.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the shared secret is not initialized or empty.</exception>
    public virtual string Decrypt(string encryptedMessageBase64)
    {
        if (_keyExchanger!.GetSharedSecret() == null || _keyExchanger!.GetSharedSecret()!.Length == 0)
            throw new InvalidOperationException(
                "KeyExchanger is null. The ECDH key wrapper must be initialized with key material.");

        var encryptedMessage = Base64UrlEncoder.DecodeBytes(encryptedMessageBase64);

        var decryptedMessage =
            _encryptionService!.Decrypt(encryptedMessage, _keyExchanger.GetSharedSecret()!, _encryptionService.IV!);

        return Encoding.UTF8.GetString(decryptedMessage);
    }
}