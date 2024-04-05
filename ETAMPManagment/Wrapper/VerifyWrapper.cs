using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ETAMPManagment.Wrapper
{
    /// <summary>
    /// Provides a wrapper for cryptographic verification using the Elliptic Curve Digital Signature Algorithm (ECDSA).
    /// This class facilitates the verification of digital signatures on data, offering simplified initialization to support
    /// different cryptographic setups.
    /// </summary>
    public sealed class VerifyWrapper : IVerifyWrapper
    {
        private readonly ECDsa? _ecdsa;
        private readonly HashAlgorithmName _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyWrapper"/> class using an ECDsa instance provided by an IECDsaProvider.
        /// This constructor is suitable for scenarios where ECDsa configuration, including the elliptic curve, is managed externally.
        /// </summary>
        /// <param name="ecdsaProvider">The IECDsaProvider instance used to obtain the ECDsa instance.</param>
        /// <param name="algorithm">The hashing algorithm to be used with ECDsa for cryptographic operations.</param>
        public VerifyWrapper(IECDsaProvider ecdsaProvider, HashAlgorithmName algorithm)
        {
            if (ecdsaProvider == null)
                throw new ArgumentNullException(nameof(ecdsaProvider), "IECDsaProvider instance cannot be null.");

            _ecdsa = ecdsaProvider.GetECDsa();
            if (_ecdsa == null)
                throw new InvalidOperationException("ECDsa instance cannot be null after extraction from IECDsaProvider.");

            _algorithm = algorithm;
        }

        /// <summary>
        /// Verifies the given data against the specified signature. This method supports verification of data provided
        /// in string format against a signature in string format, converting both to their respective byte array representations
        /// for the cryptographic operation.
        /// </summary>
        /// <param name="data">The data string to verify.</param>
        /// <param name="signature">The signature string to verify against, encoded in Base64.</param>
        /// <returns>True if the signature is valid for the given data; otherwise, false.</returns>
        public virtual bool VerifyData(string data, string signature)
        {
            return _ecdsa.VerifyData(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(signature), _algorithm);
        }

        /// <summary>
        /// Verifies the given data against the specified signature. This overload allows for direct verification of byte array data
        /// against a byte array signature, providing a lower-level interface for cryptographic operations.
        /// </summary>
        /// <param name="data">The data as a byte array to verify.</param>
        /// <param name="signature">The signature as a byte array to verify against.</param>
        /// <returns>True if the signature is valid for the given data; otherwise, false.</returns>
        public virtual bool VerifyData(byte[] data, byte[] signature)
        {
            return _ecdsa.VerifyData(data, signature, _algorithm);
        }

        /// <summary>
        /// Disposes the underlying ECDsa instance, releasing all associated resources.
        /// </summary>
        public void Dispose()
        {
            _ecdsa?.Dispose();
        }
    }
}