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
        private readonly string _securityAlgorithm;

        /// <summary>
        /// Initializes a new instance of the ECDsaSigningCredentialsProvider class using a specific ECDsa instance and security algorithm.
        /// This constructor allows for the use of an existing ECDsa instance for signing operations.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance to use for signing.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier to use for signing.</param>
        public ECDsaSigningCredentialsProvider(ECDsa ecdsa, string securityAlgorithm)
        {
            _ecdsa = ecdsa;
            _securityAlgorithm = securityAlgorithm;
        }

        /// <summary>
        /// Initializes a new instance of the ECDsaSigningCredentialsProvider class using a default ECDsa instance and an optional security algorithm.
        /// This constructor creates a new ECDsa instance internally using the default algorithm.
        /// </summary>
        /// <param name="securityAlgorithm">The security algorithm identifier to use for signing. Defaults to EcdsaSha256Signature.</param>

        public ECDsaSigningCredentialsProvider(string securityAlgorithm = SecurityAlgorithms.EcdsaSha256Signature)
        {
            _ecdsa = ECDsa.Create();
            _securityAlgorithm = securityAlgorithm;
        }

        /// <summary>
        /// Initializes a new instance of the ECDsaSigningCredentialsProvider class with a specific elliptic curve and an optional security algorithm.
        /// This constructor allows specifying the curve to be used by the ECDsa instance, providing more control over the cryptographic process.
        /// </summary>
        /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier to use for signing. Defaults to EcdsaSha256Signature.</param>

        public ECDsaSigningCredentialsProvider(ECCurve curve, string securityAlgorithm = SecurityAlgorithms.EcdsaSha256Signature)
        {
            _ecdsa = ECDsa.Create(curve);
            _securityAlgorithm = securityAlgorithm;
        }

        /// <summary>
        /// Creates and returns SigningCredentials based on the ECDsa instance and security algorithm provided during initialization.
        /// This method encapsulates the ECDsa instance and the chosen security algorithm into a SigningCredentials object, suitable for use in signing tokens.
        /// </summary>
        /// <returns>A new SigningCredentials instance configured with an ECDsaSecurityKey and the specified security algorithm.</returns>
        public SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(new ECDsaSecurityKey(_ecdsa), _securityAlgorithm);
        }
    }
}