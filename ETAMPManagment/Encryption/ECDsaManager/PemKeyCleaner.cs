using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Models;

namespace ETAMPManagment.Encryption.ECDsaManager
{
    /// <summary>
    /// Provides functionality to clean PEM formatting from public and private keys.
    /// </summary>
    public class PemKeyCleaner : IPemKeyCleaner
    {
        /// <summary>
        /// Gets the key model provider that stores the cleaned private and public keys.
        /// </summary>
        public ECDKeyModelProvider KeyModelProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PemKeyCleaner"/> class.
        /// </summary>
        public PemKeyCleaner()
        {
            KeyModelProvider = new ECDKeyModelProvider();
        }

        /// <summary>
        /// Removes PEM formatting from a private key string.
        /// </summary>
        /// <param name="privateKey">The private key string with PEM formatting.</param>
        /// <returns>The current instance of <see cref="PemKeyCleaner"/>, allowing for method chaining.</returns>
        public IPemKeyCleaner ClearPEMPrivateKey(string privateKey)
        {
            KeyModelProvider.PrivateKey = ClearPEMFormatting(privateKey);
            return this;
        }

        /// <summary>
        /// Removes PEM formatting from a public key string.
        /// </summary>
        /// <param name="publicKey">The public key string with PEM formatting.</param>
        /// <returns>The current instance of <see cref="PemKeyCleaner"/>, allowing for method chaining.</returns>
        public IPemKeyCleaner ClearPEMPublicKey(string publicKey)
        {
            KeyModelProvider.PublicKey = ClearPEMFormatting(publicKey);
            return this;
        }

        /// <summary>
        /// Removes PEM headers and footers from a key string.
        /// </summary>
        /// <param name="key">The key string with PEM formatting.</param>
        /// <returns>A string representing the key without PEM headers and footers.</returns>
        private static string ClearPEMFormatting(string key)
        {
            return key.Replace("-----BEGIN PRIVATE KEY-----", "")
                      .Replace("-----END PRIVATE KEY-----", "")
                      .Replace("-----BEGIN PUBLIC KEY-----", "")
                      .Replace("-----END PUBLIC KEY-----", "")
                      .Replace("\n", "")
                      .Replace("\r", "");
        }
    }
}