namespace ETAMPManagment.Models
{
    /// <summary>
    /// Represents the encryption details for an ETAMP (Encrypted Token And Message Protocol) structure.
    /// This class encapsulates the cryptographic keys and encrypted data, ensuring secure communication
    /// and data exchange within the ETAMP framework.
    /// </summary>
    public class ETAMPEncrypted
    {
        /// <summary>
        /// Gets or sets the private key used for decrypting data encrypted with the corresponding public key.
        /// The private key should be securely stored and managed to prevent unauthorized access.
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the public key that can be shared with other parties to encrypt data intended for decryption
        /// by the holder of the corresponding private key. The public key facilitates secure data exchange by ensuring
        /// that only the holder of the private key can decrypt the encrypted content.
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the encrypted ETAMP data. This data is encrypted using ECIES (Elliptic Curve Integrated Encryption Scheme)
        /// with an AES (Advanced Encryption Standard) encryption method, providing a strong layer of security for the encapsulated
        /// token and message. The ETAMP string encapsulates the essential information required for secure and efficient
        /// message and transaction exchanges, adhering to the principles of confidentiality, integrity, and authenticity.
        /// </summary>
        public string ETAMP { get; set; }
    }
}