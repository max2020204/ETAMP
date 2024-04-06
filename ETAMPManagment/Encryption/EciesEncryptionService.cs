using ETAMPManagment.Encryption.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ETAMPManagment.Encryption
{
    public class EciesEncryptionService(IKeyExchanger keyExchanger, IEncryptionService encryptionService) : IEciesEncryptionService
    {
        private readonly IKeyExchanger _keyExchanger = keyExchanger
            ?? throw new ArgumentNullException(nameof(keyExchanger));

        private readonly IEncryptionService _encryptionService = encryptionService
            ?? throw new ArgumentNullException(nameof(encryptionService));

        /// <summary>
        /// Encrypts a given message using ECIES.
        /// </summary>
        /// <param name="message">The plain text message to encrypt.</param>
        /// <returns>A Base64 encoded string representing the encrypted message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the shared secret is not initialized or empty.</exception>
        public virtual string Encrypt(string message)
        {
            if (_keyExchanger.GetSharedSecret() == null || _keyExchanger.GetSharedSecret().Length == 0)
                throw new InvalidOperationException("KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");

            byte[] secretKey = _keyExchanger.GetSharedSecret();
            byte[] encryptedMessage = _encryptionService.Encrypt(Encoding.UTF8.GetBytes(message), secretKey);
            return Base64UrlEncoder.Encode(encryptedMessage);
        }

        /// <summary>
        /// Decrypts a given encrypted message back into its plain text form using ECIES.
        /// </summary>
        /// <param name="encryptedMessageBase64">The encrypted message in Base64 encoding to decrypt.</param>
        /// <param name="privateKey">The private key used to derive the shared secret for decryption.</param>
        /// <returns>The decrypted plain text message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the shared secret is not initialized or empty.</exception>
        /// <exception cref="FormatException">Thrown if the encrypted message is not in a valid Base64 format.</exception>
        public virtual string Decrypt(string encryptedMessageBase64, byte[] privateKey)
        {
            if (_keyExchanger.GetSharedSecret() == null || _keyExchanger.GetSharedSecret().Length == 0)
                throw new InvalidOperationException("KeyExchanger is null. The ECDH key wrapper must be initialized with key material.");

            byte[] encryptedMessage;
            try
            {
                encryptedMessage = Base64UrlEncoder.DecodeBytes(encryptedMessageBase64);
            }
            catch (FormatException ex)
            {
                throw new FormatException("The encrypted message is not in a valid Base64 format.", ex);
            }

            byte[] secretKey = _keyExchanger.DeriveKey(privateKey);
            byte[] decryptedMessage = _encryptionService.Decrypt(encryptedMessage, secretKey);

            return Encoding.UTF8.GetString(decryptedMessage);
        }
    }
}