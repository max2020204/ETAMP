namespace ETAMPManagment.Encryption.Interfaces
{
    /// <summary>
    /// Extends the basic encryption service interface to provide AES-specific encryption capabilities,
    /// including support for an Initialization Vector (IV).
    /// </summary>
    public interface IAesEncryptionService : IEncryptionService
    {
        /// <summary>
        /// Gets the Initialization Vector (IV) used for AES encryption and decryption.
        /// The IV is a byte array that is used to add randomness to the encryption process
        /// and to ensure that encrypted values are unique.
        /// </summary>
        byte[] IV { get; }
    }
}