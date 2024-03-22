using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using System.Text;

namespace ETAMPManagment.Services
{
    /// <summary>
    /// Implements Elliptic Curve Integrated Encryption Scheme (ECIES) for encrypting and decrypting messages.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EciesEncryptionService"/> class.
    /// </remarks>
    /// <param name="keyExchanger">The key exchanger to derive the shared secret for encryption and decryption.</param>
    /// <param name="factory">The factory to create the encryption service based on a specified encryption type.</param>
    /// <param name="encryptionType">The type of encryption to be used by the encryption service.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyExchanger"/> or the encryption service creation fails.</exception>
    public class EciesEncryptionService(IKeyExchanger keyExchanger, IEncryptionServiceFactory factory, string encryptionType) : IEciesEncryptionService
    {
        private readonly IKeyExchanger _keyExchanger = keyExchanger ?? throw new ArgumentNullException(nameof(keyExchanger));

        private readonly IEncryptionService _encryptionService = factory.CreateEncryptionService(encryptionType) ?? throw new ArgumentNullException("Encryption service creation failed.");

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
            return Convert.ToBase64String(encryptedMessage);
        }

        /// <summary>
        /// Decrypts a given encrypted message back into its plain text form using ECIES.
        /// </summary>
        /// <param name="encryptedMessageBase64">The encrypted message in Base64 encoding to decrypt.</param>
        /// <param name="publicKey">The public key used to derive the shared secret for decryption.</param>
        /// <returns>The decrypted plain text message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the shared secret is not initialized or empty.</exception>
        /// <exception cref="FormatException">Thrown if the encrypted message is not in a valid Base64 format.</exception>
        public virtual string Decrypt(string encryptedMessageBase64, byte[] publicKey)
        {
            if (_keyExchanger.GetSharedSecret() == null || _keyExchanger.GetSharedSecret().Length == 0)
                throw new InvalidOperationException("KeyExchanger is null. The ECDH key wrapper must be initialized with key material.");

            byte[] encryptedMessage;
            try
            {
                encryptedMessage = Convert.FromBase64String(encryptedMessageBase64);
            }
            catch (FormatException ex)
            {
                throw new FormatException("The encrypted message is not in a valid Base64 format.", ex);
            }

            byte[] secretKey = _keyExchanger.DeriveKey(publicKey);
            byte[] decryptedMessage = _encryptionService.Decrypt(encryptedMessage, secretKey);

            return Encoding.UTF8.GetString(decryptedMessage);
        }
    }
}