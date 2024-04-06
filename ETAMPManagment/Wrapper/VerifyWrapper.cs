using ETAMPManagment.Encryption.ECDsaManager.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ETAMPManagment.Wrapper
{
    /// <summary>
    /// Provides cryptographic verification using ECDSA, supporting both string and byte array data formats.
    /// </summary>>
    public sealed class VerifyWrapper : IVerifyWrapper
    {
        private readonly ECDsa? _ecdsa;
        private readonly HashAlgorithmName _algorithm;

        /// <summary>
        /// Initializes the class with an ECDsa instance from an IECDsaProvider and a hashing algorithm.
        /// </summary>
        /// <param name="ecdsaProvider">Provider to obtain the ECDsa instance.</param>
        /// <param name="algorithm">Hashing algorithm for verification.</param>
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
        /// Verifies the signature of string data.
        /// </summary>
        /// <param name="data">Data to verify, in string format.</param>
        /// <param name="signature">Base64-encoded signature to verify against.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        public bool VerifyData(string data, string signature)
        {
            return _ecdsa.VerifyData(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(signature), _algorithm);
        }

        /// <summary>
        /// Verifies the signature of byte array data.
        /// </summary>
        /// <param name="data">Data to verify, as a byte array.</param>
        /// <param name="signature">Signature to verify against, as a byte array.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        public bool VerifyData(byte[] data, byte[] signature)
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