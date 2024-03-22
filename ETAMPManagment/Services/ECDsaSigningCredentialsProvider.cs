using ETAMPManagment.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace ETAMPManagment.Services
{
    /// <summary>
    /// Provides signing credentials using the ECDsa cryptographic algorithm.
    /// </summary>
    public class ECDsaSigningCredentialsProvider : ISigningCredentialsProvider
    {
        private readonly ECDsa _ecdsa;
        private readonly string _securityAlgorithm;

        /// <summary>
        /// Initializes a new instance of the ECDsaSigningCredentialsProvider class with the specified ECDsa instance and security algorithm.
        /// </summary>
        /// <param name="ecdsa">The ECDsa instance to use for signing.</param>
        /// <param name="securityAlgorithm">The security algorithm identifier to use for signing.</param>
        public ECDsaSigningCredentialsProvider(ECDsa ecdsa, string securityAlgorithm)
        {
            _ecdsa = ecdsa;
            _securityAlgorithm = securityAlgorithm;
        }

        /// <inheritdoc />
        public virtual SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(new ECDsaSecurityKey(_ecdsa), _securityAlgorithm);
        }
    }
}