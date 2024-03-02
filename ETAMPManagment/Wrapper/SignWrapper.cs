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
        /// <param name="ecdsa">The ECDsa instance for signing.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(ECDsa ecdsa, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsa;
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using a private key.
        /// </summary>
        /// <param name="privateKey">The private key in PKCS#8 format as a base64-encoded string.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(string privateKey, HashAlgorithmName algorithmName)
        {
            _ecdsa = ECDsa.Create();
            _ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using an <see cref="IEcdsaWrapper"/> for importing keys, a private key, and a curve.
        /// </summary>
        /// <param name="ecdsaWrapper">The wrapper for ECDsa operations.</param>
        /// <param name="privateKey">The private key as a string.</param>
        /// <param name="curve">The elliptic curve to use.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(IEcdsaWrapper ecdsaWrapper, string privateKey, ECCurve curve, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsaWrapper.ImportECDsa(privateKey, curve);
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignWrapper"/> class using an <see cref="IEcdsaWrapper"/> for importing keys, a private key as a byte array, and a curve.
        /// </summary>
        /// <param name="ecdsaWrapper">The wrapper for ECDsa operations.</param>
        /// <param name="privateKey">The private key as a byte array.</param>
        /// <param name="curve">The elliptic curve to use.</param>
        /// <param name="algorithmName">The hash algorithm to use for signing.</param>
        public SignWrapper(IEcdsaWrapper ecdsaWrapper, byte[] privateKey, ECCurve curve, HashAlgorithmName algorithmName)
        {
            _ecdsa = ecdsaWrapper.ImportECDsa(privateKey, curve);
            _algorithmName = algorithmName;
        }

        /// <summary>
        /// Signs the specified data using the initialized ECDsa instance and algorithm.
        /// </summary>
        /// <param name="data">The data to sign as a byte array.</param>
        /// <returns>The digital signature as a byte array.</returns>
        public byte[] Sign(byte[] data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        /// <summary>
        /// Signs the specified data from a stream using the initialized ECDsa instance and algorithm.
        /// </summary>
        /// <param name="data">The data stream to sign.</param>
        /// <returns>The digital signature as a byte array.</returns>
        public byte[] Sign(Stream data)
        {
            return _ecdsa.SignData(data, _algorithmName);
        }

        /// <summary>
        /// Signs a JSON representation of an ETAMP model, updating its signature fields.
        /// </summary>
        /// <param name="jsonEtamp">The JSON string representing the ETAMP model to sign.</param>
        /// <returns>A JSON string of the ETAMP model with updated signature fields.</returns>
        public string SignEtamp(string jsonEtamp)
        {
            ETAMPModel etamp = JsonConvert.DeserializeObject<ETAMPModel>(jsonEtamp);
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }

        /// <summary>
        /// Signs an ETAMP model, updating its signature fields.
        /// </summary>
        /// <param name="etamp">The ETAMP model to sign.</param>
        /// <returns>A JSON string of the ETAMP model with updated signature fields.</returns>
        public string SignEtampModel(ETAMPModel etamp)
        {
            etamp.SignatureToken = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(etamp.Token)));
            etamp.SignatureMessage = Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}")));
            return JsonConvert.SerializeObject(etamp);
        }
    }
}