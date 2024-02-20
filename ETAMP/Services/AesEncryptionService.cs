using ETAMP.Services.Interfaces;
using System.Security.Cryptography;

namespace ETAMP.Services
{
    /// <summary>
    /// Provides AES encryption and decryption services.
    /// </summary>
    public class AesEncryptionService : IAesEncryptionService
    {
        private byte[] _iv;

        /// <summary>
        /// Gets the initialization vector (IV) used in encryption or decryption.
        /// </summary>
        public byte[] IV
        {
            get { return _iv; }
        }

        /// <summary>
        /// Initializes a new instance of the AesEncryptionService class.
        /// </summary>
        public AesEncryptionService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the AesEncryptionService class with a specific IV.
        /// </summary>
        /// <param name="iv">The initialization vector (IV) to use for encryption and decryption.</param>
        public AesEncryptionService(byte[] iv)
        {
            _iv = iv;
        }

        /// <summary>
        /// Encrypts the specified data using the AES algorithm.
        /// </summary>
        /// <param name="dataToEncrypt">The data to encrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data or key is null.</exception>
        public virtual byte[] Encrypt(byte[] dataToEncrypt, byte[] key)
        {
            if (dataToEncrypt == null)
                throw new ArgumentNullException(nameof(dataToEncrypt));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                _iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                }
            }
        }

        /// <summary>
        /// Decrypts the specified data using the AES algorithm.
        /// </summary>
        /// <param name="dataToDecrypt">The data to decrypt.</param>
        /// <param name="key">The decryption key.</param>
        /// <returns>The decrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data or key is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the IV is not set.</exception>

        public virtual byte[] Decrypt(byte[] dataToDecrypt, byte[] key)
        {
            if (dataToDecrypt == null)
                throw new ArgumentNullException(nameof(dataToDecrypt));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (IV == null)
                throw new InvalidOperationException("IV is not set.");
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                }
            }
        }
    }
}