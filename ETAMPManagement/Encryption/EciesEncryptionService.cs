#region

using System.Text;
using ETAMPManagement.Encryption.Interfaces;
using ETAMPManagement.Helper;

#endregion

namespace ETAMPManagement.Encryption;

/// <summary>
///     Provides encryption and decryption functionalities using ECIES encryption algorithm.
/// </summary>
public sealed class EciesEncryptionService : InitializeBase, IEciesEncryptionService
{
    private IEncryptionService? _encryptionService;

    /// <summary>
    ///     Provides functionality for key exchange and derivation using Elliptic Curve Diffie-Hellman (ECDH) algorithm.
    /// </summary>
    /// <remarks>
    ///     This class is used by the EciesEncryptionService to perform key exchange and derivation operations.
    /// </remarks>
    private IKeyExchanger? _keyExchanger;

    /// <summary>
    ///     Initializes the ECIES encryption service with the necessary key exchange and encryption components.
    /// </summary>
    /// <param name="keyExchanger">The key exchanger used for deriving the shared secret.</param>
    /// <param name="encryptionService">The underlying encryption service used for encrypting and decrypting the message.</param>
    public void Initialize(IKeyExchanger keyExchanger, IEncryptionService encryptionService)
    {
        Init = true;
        _keyExchanger = keyExchanger ?? throw new ArgumentNullException(nameof(keyExchanger));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    ///     Encrypts the given message using ECIES encryption algorithm.
    /// </summary>
    /// <param name="message">The message to encrypt.</param>
    /// <returns>The encrypted message.</returns>
    public string? Encrypt(string message)
    {
        CheckInitialization();
        ValidateSecret();
        var encryptedMessage = _encryptionService!.Encrypt(Encoding.UTF8.GetBytes(message),
            _keyExchanger!.GetSharedSecret()!, _encryptionService.IV);

        return Base64UrlEncoder.Encode(encryptedMessage);
    }

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>The decrypted plain text message.</returns>
    public string Decrypt(string? encryptedMessageBase64)
    {
        CheckInitialization();
        ValidateSecret();
        var encryptedMessage = Base64UrlEncoder.DecodeBytes(encryptedMessageBase64);

        var decryptedMessage =
            _encryptionService!.Decrypt(encryptedMessage, _keyExchanger?.GetSharedSecret()!, _encryptionService.IV!);

        return Encoding.UTF8.GetString(decryptedMessage);
    }

    private void ValidateSecret()
    {
        if (_keyExchanger!.GetSharedSecret() == null || _keyExchanger!.GetSharedSecret()!.Length == 0)
            throw new InvalidOperationException(
                "KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");
    }
}