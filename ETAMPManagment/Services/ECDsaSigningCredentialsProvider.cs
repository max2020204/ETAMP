using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace ETAMPManagment.Services
{
    /// <summary>
    /// Provides signing credentials using the ECDsa cryptographic algorithm.
    /// This provider facilitates the creation of signing credentials that can be used for digital signatures.
    /// </summary>
    public class ECDsaSigningCredentialsProvider : ISigningCredentialsProvider
    {
        private readonly ECDsa _ecdsa;

        public string SecurityAlgorithm { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ECDsaSigningCredentialsProvider"/> class using a provider for ECDsa instances and a security algorithm.
        /// This constructor allows for the flexible use of an ECDsa instance provided by the <see cref="IECDsaProvider"/> for signing operations.
        /// </summary>
        /// <param name="ecdsaProvider">The provider for obtaining an ECDsa instance to use for signing.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier to use for signing, such as "ES256", "ES384", or "ES512".</param>
        public ECDsaSigningCredentialsProvider(IECDsaProvider ecdsaProvider)
        {
            ArgumentNullException.ThrowIfNull(ecdsaProvider);
            _ecdsa = ecdsaProvider.GetECDsa()
                ?? throw new InvalidOperationException("ECDsa instance cannot be null.");
            SecurityAlgorithm = SecurityAlgorithms.EcdsaSha256Signature;
        }

        /// <summary>
        /// Creates and returns SigningCredentials based on the ECDsa instance and security algorithm provided during initialization.
        /// This method encapsulates the ECDsa instance and the chosen security algorithm into a SigningCredentials object, suitable for use in signing tokens.
        /// </summary>
        /// <returns>A new SigningCredentials instance configured with an ECDsaSecurityKey and the specified security algorithm.</returns>
        public SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(new ECDsaSecurityKey(_ecdsa), SecurityAlgorithm);
        }
    }
}