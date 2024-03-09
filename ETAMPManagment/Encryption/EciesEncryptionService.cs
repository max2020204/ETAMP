using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Factories.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using System.Text;

namespace ETAMPManagment.Services
{
    /// <summary>
    /// Provides encryption and decryption services using Elliptic Curve Integrated Encryption Scheme (ECIES).
    /// </summary>
    public class EciesEncryptionService : IEciesEncryptionService
    {
        private readonly IKeyExchanger _keyExchanger;

        private readonly IEncryptionService _encryptionService;

        public EciesEncryptionService(IKeyExchanger keyExchanger, IEncryptionServiceFactory factory, string encryptionType)
        {
            _keyExchanger = keyExchanger ?? throw new ArgumentNullException(nameof(keyExchanger));
            _encryptionService = factory.CreateEncryptionService(encryptionType) ?? throw new ArgumentNullException("Encryption service creation failed.");
        }

        public virtual string Encrypt(string message)
        {
            if (_keyExchanger.GetSharedSecret() == null || _keyExchanger.GetSharedSecret().Length == 0)
            {
                throw new InvalidOperationException("KeyExchanger is null or empty. The ECDH key wrapper must be initialized with key material.");
            }
            byte[] secretKey = _keyExchanger.GetSharedSecret();
            byte[] encryptedMessage = _encryptionService.Encrypt(Encoding.UTF8.GetBytes(message), secretKey);
            return Convert.ToBase64String(encryptedMessage);
        }

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