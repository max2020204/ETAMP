#region

using System.Text;
using ETAMP.Core.Utils;
using ETAMP.Encryption.Base;

#endregion

namespace ETAMP.Encryption;

/// <summary>
///     Provides encryption and decryption functionalities using ECIES encryption algorithm.
/// </summary>
public sealed class ECIESEncryptionService : ECIESEncryptionServiceBase
{
    /// <summary>
    ///     Encrypts the given message using ECIES encryption algorithm.
    /// </summary>
    /// <param name="message">The message to encrypt.</param>
    /// <returns>The encrypted message.</returns>
    public override string Encrypt(string message)
    {
        CheckInitialization();
        ValidateSecret();
        var encryptedMessage = EncryptionService!.Encrypt(Encoding.UTF8.GetBytes(message),
            KeyExchanger!.GetSharedSecret()!, EncryptionService.IV);

        return Base64UrlEncoder.Encode(encryptedMessage);
    }

    /// <summary>
    ///     Decrypts an encrypted message back to its plain text form using ECIES.
    /// </summary>
    /// <param name="encryptedMessageBase64">The encrypted message as a Base64-encoded string.</param>
    /// <returns>The decrypted plain text message.</returns>
    public override string Decrypt(string? encryptedMessageBase64)
    {
        CheckInitialization();
        ValidateSecret();
        var encryptedMessage = Base64UrlEncoder.DecodeBytes(encryptedMessageBase64);

        var decryptedMessage =
            EncryptionService!.Decrypt(encryptedMessage, KeyExchanger?.GetSharedSecret()!, EncryptionService.IV!);

        return Encoding.UTF8.GetString(decryptedMessage);
    }

    private void ValidateSecret()
    {
        if (KeyExchanger!.GetSharedSecret() == null || KeyExchanger!.GetSharedSecret()!.Length == 0)
            throw new InvalidOperationException(
                "KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");
    }
}