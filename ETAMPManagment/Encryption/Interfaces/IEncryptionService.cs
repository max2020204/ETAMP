namespace ETAMPManagment.Encryption.Interfaces
{
    /// <summary>
    /// Provides mechanisms for encrypting and decrypting data.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts the specified data using the provided key.
        /// </summary>
        /// <param name="data">The data to encrypt, represented as a byte array.</param>
        /// <param name="key">The encryption key, represented as a byte array.</param>
        /// <returns>The encrypted data as a byte array.</returns>
        byte[] Encrypt(byte[] data, byte[] key);

        /// <summary>
        /// Decrypts the specified data using the provided key.
        /// </summary>
        /// <param name="data">The data to decrypt, represented as a byte array.</param>
        /// <param name="key">The decryption key, represented as a byte array.</param>
        /// <returns>The decrypted data as a byte array.</returns>
        byte[] Decrypt(byte[] data, byte[] key);
    }
}