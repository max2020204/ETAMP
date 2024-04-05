using ETAMPManagment.Encryption.Interfaces;
using System.Security.Cryptography;

namespace ETAMPManagment.Encryption
{
    /// <summary>
    /// Provides AES encryption and decryption services.
    /// </summary>
    public class AesEncryptionService : IAesEncryptionService
    {
        private byte[]? _iv;

        /// <summary>
        /// Gets the initialization vector (IV) used in encryption or decryption.
        /// </summary>
        public byte[]? IV
        {
            get { return _iv; }
        }

        /// <summary>
        /// Initializes a new instance of the AesEncryptionService class with a specific IV.
        /// </summary>
        /// <param name="iv">The initialization vector (IV) to use for encryption and decryption.</param>
        public AesEncryptionService(byte[]? iv) => _iv = iv;

        /// <summary>
        /// Encrypts the specified data using the AES algorithm.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data or key is null.</exception>
        public virtual byte[] Encrypt(byte[] data, byte[] key)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(key);
            using Aes aes = Aes.Create();
            aes.Key = key;
            _iv = aes.IV;

            using ICryptoTransform encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(data, 0, data.Length);
        }

        /// <summary>
        /// Decrypts the specified data using the AES algorithm.
        /// </summary>
        /// <param name="data">The data to decrypt.</param>
        /// <param name="key">The decryption key.</param>
        /// <returns>The decrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data or key is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the IV is not set.</exception>

        public virtual byte[] Decrypt(byte[] data, byte[] key)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(key);

            if (IV == null)
                throw new InvalidOperationException("IV is not set.");

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = IV;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}