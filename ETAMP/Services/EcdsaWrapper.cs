﻿using ETAMP.Services.Interfaces;
using System.Security.Cryptography;

namespace ETAMP.Services
{
    /// <summary>
    /// A factory class for creating cryptographic objects such as ECDsa and JwtSecurityTokenHandler.
    /// This factory allows for centralized creation and configuration of cryptographic components,
    /// facilitating easier management and potential customization of cryptographic operations.
    /// </summary>
    public class EcdsaWrapper : IEcdsaWrapper
    {
        /// <summary>
        /// Creates a default ECDsa instance.
        /// This method is suitable for scenarios where no specific elliptic curve or public key is required.
        /// </summary>
        /// <returns>A new instance of ECDsa with default settings.</returns>
        public virtual ECDsa CreateECDsa()
        {
            return ECDsa.Create();
        }

        /// <summary>
        /// Creates an ECDsa instance using a specified elliptic curve.
        /// This method allows for custom configuration of ECDsa using a specific curve,
        /// providing more control over the cryptographic characteristics.
        /// </summary>
        /// <param name="curve">The elliptic curve to use for creating the ECDsa instance.</param>
        /// <returns>A new instance of ECDsa configured with the specified curve.</returns>
        public virtual ECDsa CreateECDsa(ECCurve curve)
        {
            return ECDsa.Create(curve);
        }

        /// <summary>
        /// Creates an ECDsa instance using a specified public key and elliptic curve.
        /// This method is ideal for use cases where both the public key and the elliptic curve are known,
        /// allowing for the creation of a pre-configured ECDsa instance.
        /// </summary>
        /// <param name="publicKey">The public key in Base64 string format.</param>
        /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
        /// <returns>A new instance of ECDsa configured with the specified public key and curve.</returns>
        public virtual ECDsa CreateECDsa(string publicKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
            return ecdsa;
        }

        /// <summary>
        /// Creates an ECDsa instance using a public key as a byte array and a specified elliptic curve.
        /// This method is tailored for scenarios where the public key is available in byte array format,
        /// enabling direct use of byte array keys for cryptographic operations.
        /// </summary>
        /// <param name="publicKey">The public key as a byte array.</param>
        /// <param name="curve">The elliptic curve to use for the ECDsa instance.</param>
        /// <returns>A new instance of ECDsa configured with the specified public key (byte array) and curve.</returns>
        public virtual ECDsa CreateECDsa(byte[] publicKey, ECCurve curve)
        {
            ECDsa ecdsa = ECDsa.Create(curve);
            ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
            return ecdsa;
        }
    }
}