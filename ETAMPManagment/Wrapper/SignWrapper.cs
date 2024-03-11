using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ETAMPManagment.Wrapper
{
    /// <summary>
    /// Provides functionality for signing data using Elliptic Curve Digital Signature Algorithm (ECDsa).
    /// </summary>
    /// <remarks>
    /// This class allows for the creation of digital signatures using ECDsa, supporting multiple initializations
    /// including from a private key, an <see cref="ECDsa"/> instance, or an <see cref="IEcdsaWrapper"/> for custom
    /// implementation scenarios. It supports signing byte arrays, streams, and JSON representations of specific models.
    /// </remarks>
    public class SignWrapper : ISignWrapper
    {
        private readonly ECDsa _ecdsa;
        private readonly HashAlgorithmName _algorithmName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using an existing <see cref="ECDsa"/> instance.
        /// </summary>
        /// <param name="ecdsa">The <see cref="ECDsa"/> instance for signing.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(ECDsa ecdsa, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsa;
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using a private key.
        /// </summary>
        /// <param name="privateKey">The private key in PKCS#8 format.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(string privateKey, HashAlgorithmName algorithmName)
        {
            _ecdsa = ECDsa.Create();
            _ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using an <see cref="IEcdsaWrapper"/> and a private key.
        /// </summary>
        /// <param name="ecdsaWrapper">The wrapper for ECDsa operations.</param>
        /// <param name="privateKey">The private key as a string.</param>
        /// <param name="curve">The ECCurve for the ECDsa.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(IEcdsaWrapper ecdsaWrapper, string privateKey, ECCurve curve, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsaWrapper.ImportECDsa(privateKey, curve);
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class. This constructor allows the creation of a signing utility
        /// by specifying an <see cref="IEcdsaWrapper"/> for elliptic curve cryptography operations, a private key as a read-only span of bytes,
        /// the elliptic curve to use, and the hash algorithm for signing operations.
        /// </summary>
        /// <param name="ecdsaWrapper">The <see cref="IEcdsaWrapper"/> instance used for ECDsa operations, providing methods to create and import ECDsa keys.</param>
        /// <param name="privateKey">The private key used for signing operations, provided as a read-only span of bytes. This allows for the efficient handling of the private key data without unnecessary copying or conversions.</param>
        /// <param name="curve">The <see cref="ECCurve"/> specifying the elliptic curve parameters to use for the cryptographic operations. This parameter is essential for configuring the ECDsa instance with the correct curve.</param>
        /// <param name="algorithmName">The <see cref="HashAlgorithmName"/> specifying the hash algorithm to use for generating signatures. This determines how data will be hashed before being signed with the private key.</param>
        public SignWrapper(IEcdsaWrapper ecdsaWrapper, ReadOnlySpan<byte> privateKey, ECCurve curve, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsaWrapper.ImportECDsa(privateKey, curve);
            _algorithmName = algorithmName;
        }

        private byte[] Sign(byte[] data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        /// <summary>
        /// Signs the ETAMP data given as a JSON string and updates its signature fields.
        /// </summary>
        /// <param name="jsonEtamp">The ETAMP data in JSON format.</param>
        /// <returns>A JSON string of the ETAMP data with updated signature fields.</returns>
        public virtual string SignEtamp(string jsonEtamp)
        {
            ETAMPModel etamp = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Signs the ETAMP model and updates its signature fields.
        /// </summary>
        /// <param name="etamp">The ETAMP model to be signed.</param>
        /// <returns>A JSON string of the ETAMP data with updated signature fields.</returns>
        public virtual string SignEtamp(ETAMPModel etamp)
        {
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Signs the ETAMP model and returns the model with updated signature fields.
        /// </summary>
        /// <param name="etamp">The ETAMP model to be signed.</param>
        /// <returns>The ETAMP model with updated signature fields.</returns>
        public virtual ETAMPModel SignEtampModel(ETAMPModel etamp)
        {
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return etamp;
        }
    }
}