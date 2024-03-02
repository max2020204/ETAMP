using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using System.Text;

namespace ETAMPManagment.Services
{
    /// <summary>
    /// Provides encryption and decryption services using Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    public class EciesEncryptionService : IEciesEncryptionService
    {
        /// <summary>
        /// Gets the ECDH key wrapper used for key exchange.
        /// </summary>
        public IEcdhKeyWrapper EcdhKeyWrapper { get; }

        /// <summary>
        /// Gets the encryption service used for encrypting and decrypting data.
        /// </summary>
        public IEncryptionService EncryptionService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EciesEncryptionService"/> class.
        /// </summary>
        /// <param name="ecdhKeyWrapper">The ECDH key wrapper used for key exchange.</param>
        /// <param name="factory">The factory used to create the encryption service.</param>
        /// <param name="encryptionType">The type of encryption to be used.</param>
        /// <exception cref="ArgumentNullException">Thrown if the ecdhKeyWrapper or factory is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the encryption service creation fails.</exception>
        public EciesEncryptionService(IEcdhKeyWrapper ecdhKeyWrapper, IEncryptionServiceFactory factory, string encryptionType)
        {
            EcdhKeyWrapper = ecdhKeyWrapper ?? throw new ArgumentNullException(nameof(ecdhKeyWrapper));
            EncryptionService = factory.CreateEncryptionService(encryptionType) ?? throw new ArgumentNullException("Encryption service creation failed.");
        }

        /// <summary>
        /// Encrypts the specified message using the configured encryption service.
        /// </summary>
        /// <param name="message">The message to encrypt.</param>
        /// <returns>The encrypted message as a base64 string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the key exchanger is not initialized.</exception>

        public virtual string Encrypt(string message)
        {
            if (EcdhKeyWrapper.KeyExchanger == null || EcdhKeyWrapper.KeyExchanger.Length == 0)
            {
                throw new InvalidOperationException("KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");
            }
            byte[] secretKey = EcdhKeyWrapper.KeyExchanger;
            byte[] encryptedMessage = EncryptionService.Encrypt(Encoding.UTF8.GetBytes(message), secretKey);

            // Предположим, что encryptedMessage включает в себя IV и зашифрованные данные
            return Convert.ToBase64String(encryptedMessage);
        }

        /// <summary>
        /// Decrypts the specified encrypted message using the configured encryption service.
        /// </summary>
        /// <param name="encryptedMessageBase64">The encrypted message in base64 format.</param>
        /// <param name="publicKey">The public key used for deriving the secret key in ECDH key exchange.</param>
        /// <returns>The decrypted message.</returns>
        /// <exception cref="FormatException">Thrown if the encrypted message is not in a valid base64 format.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the key exchanger is not initialized.</exception>
        public virtual string Decrypt(string encryptedMessageBase64, byte[] publicKey)
        {
            if (EcdhKeyWrapper.KeyExchanger == null || EcdhKeyWrapper.KeyExchanger.Length == 0)
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
            byte[] secretKey = EcdhKeyWrapper.DeriveKey(publicKey);

            byte[] decryptedMessage = EncryptionService.Decrypt(encryptedMessage, secretKey);

            return Encoding.UTF8.GetString(decryptedMessage);
        }
    }
}