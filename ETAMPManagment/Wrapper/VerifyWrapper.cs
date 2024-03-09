using ETAMPManagment.Wrapper.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ETAMPManagment.Wrapper
{
    /// <summary>
    /// Provides a wrapper for cryptographic verification using the Elliptic Curve Digital Signature Algorithm (ECDSA).
    /// This class facilitates the verification of digital signatures on data, offering various constructors to support
    /// different initialization scenarios based on available cryptographic parameters.
    /// </summary>
    public class VerifyWrapper : IVerifyWrapper
    {
        private ECDsa _ecdsa;

        /// <summary>
        /// Gets the underlying ECDsa instance used for cryptographic operations.
        /// </summary>
        public ECDsa ECDsa
        {
            get { return _ecdsa; }
        }

        private readonly HashAlgorithmName _algorithm;

        /// <summary>
        /// Initializes a new instance of the VerifyWrapper class using a token services factory to create a default ECDsa instance.
        /// This approach is suitable when the specific configuration of the ECDsa instance, including the elliptic curve, is managed externally.
        /// </summary>
        /// <param name="tokenServicesFactory">The factory used to create the ECDsa instance.</param>
        /// <param name="algorithm">The hashing algorithm to be used with ECDsa for cryptographic operations.</param>
        public VerifyWrapper(IEcdsaWrapper tokenServicesFactory, HashAlgorithmName algorithm)
        {
            _ecdsa = tokenServicesFactory.CreateECDsa();
            _algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the VerifyWrapper class using a token services factory to create an ECDsa instance with a specified elliptic curve.
        /// This constructor is ideal for use cases where a specific elliptic curve needs to be explicitly provided for ECDsa.
        /// </summary>
        /// <param name="tokenServicesFactory">The factory used to create the ECDsa instance.</param>
        /// <param name="curve">The elliptic curve to be used for creating the ECDsa instance.</param>
        /// <param name="algorithm">The hashing algorithm to be used with ECDsa for cryptographic operations.</param>
        public VerifyWrapper(IEcdsaWrapper tokenServicesFactory, ECCurve curve, HashAlgorithmName algorithm)
        {
            _ecdsa = tokenServicesFactory.CreateECDsa(curve);
            _algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the VerifyWrapper class using a token services factory to create an ECDsa instance with a specified public key in string format and elliptic curve.
        /// This constructor is ideal for scenarios where both a specific elliptic curve and a public key in Base64 string format are provided for the configuration of the ECDsa instance.
        /// It allows for the configuration of ECDsa using a public key in a convenient string format along with the specified curve.
        /// </summary>
        /// <param name="tokenServicesFactory">The factory used to create the ECDsa instance.</param>
        /// <param name="publicKey">The public key in Base64 string format for the ECDsa instance.</param>
        /// <param name="curve">The elliptic curve to be used for creating the ECDsa instance.</param>
        /// <param name="algorithm">The hashing algorithm to be used with ECDsa for cryptographic operations.</param>

        public VerifyWrapper(IEcdsaWrapper tokenServicesFactory, string publicKey, ECCurve curve, HashAlgorithmName algorithm)
        {
            _ecdsa = tokenServicesFactory.CreateECDsa(publicKey, curve);
            _algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the VerifyWrapper class using a token services factory to create an ECDsa instance with a specified public key as a byte array and elliptic curve.
        /// This constructor is designed for situations where the elliptic curve and public key are provided as a byte array, offering a direct approach to configure the ECDsa instance.
        /// It is useful in scenarios where public key and curve details are already available in byte array format, simplifying the ECDsa setup process.
        /// </summary>
        /// <param name="tokenServicesFactory">The factory used to create the ECDsa instance.</param>
        /// <param name="publicKey">The public key as a byte array for the ECDsa instance.</param>
        /// <param name="curve">The elliptic curve to be used for creating the ECDsa instance.</param>
        /// <param name="algorithm">The hashing algorithm to be used with ECDsa for cryptographic operations.</param>
        public VerifyWrapper(IEcdsaWrapper tokenServicesFactory, byte[] publicKey, ECCurve curve, HashAlgorithmName algorithm)
        {
            _ecdsa = tokenServicesFactory.CreateECDsa(publicKey, curve);
            _algorithm = algorithm;
        }

        /// <summary>
        /// Initializes a new instance of the VerifyWrapper class with a pre-configured ECDsa instance.
        /// This constructor is useful when an ECDsa instance has already been set up elsewhere in the application and needs to be reused.
        /// It allows for the integration of a custom-configured ECDsa instance, providing flexibility in how cryptographic operations are handled.
        /// </summary>
        /// <param name="ecdsa">The pre-configured ECDsa instance to be used.</param>
        /// <param name="algorithm">The hashing algorithm to be used with ECDsa for cryptographic operations.</param>
        public VerifyWrapper(ECDsa ecdsa, HashAlgorithmName algorithm)
        {
            _ecdsa = ecdsa;
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
        /// Verifies the given data string against the specified signature byte array.
        /// </summary>
        /// <param name="data">The data string to verify.</param>
        /// <param name="signature">The signature as a byte array to verify against.</param>
        public virtual bool VerifyData(string data, byte[] signature)
        {
            return _ecdsa.VerifyData(Encoding.UTF8.GetBytes(data), signature, _algorithm);
        }

        /// <summary>
        /// Verifies the given data byte array against the specified signature string.
        /// </summary>
        /// <param name="data">The data as a byte array to verify.</param>
        /// <param name="signature">The signature string to verify against.</param>
        public virtual bool VerifyData(byte[] data, string signature)
        {
            return _ecdsa.VerifyData(data, Convert.FromBase64String(signature), _algorithm);
        }

        /// <summary>
        /// Disposes the underlying ECDsa instance, releasing all associated resources.
        /// </summary>
        public virtual void Dispose()
        {
            _ecdsa?.Dispose();
        }
    }
}