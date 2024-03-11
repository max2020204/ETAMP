using System.Security.Cryptography;

namespace ETAMPManagment.Wrapper.Interfaces
{
    /// <summary>
    /// Provides a wrapper around ECDsa operations, offering creation, import, and cleanup functionalities for ECDsa keys.
    /// </summary>
    public interface IEcdsaWrapper
    {
        /// <summary>
        /// Creates a new ECDsa instance using the default parameters.
        /// </summary>
        /// <returns>A new ECDsa instance.</returns>
        ECDsa CreateECDsa();

        /// <summary>
        /// Creates a new ECDsa instance using a specific ECCurve.
        /// </summary>
        /// <param name="curve">The ECCurve to use for the ECDsa key.</param>
        /// <returns>A new ECDsa instance configured with the specified curve.</returns>
        ECDsa CreateECDsa(ECCurve curve);

        /// <summary>
        /// Creates a new ECDsa instance from a public key string and curve.
        /// </summary>
        /// <param name="publicKey">The public key in PEM format.</param>
        /// <param name="curve">The ECCurve associated with the public key.</param>
        /// <returns>A new ECDsa instance.</returns>
        ECDsa CreateECDsa(string publicKey, ECCurve curve);

        /// <summary>
        /// Creates a new ECDsa instance from a public key byte array and curve.
        /// </summary>
        /// <param name="publicKey">The public key as a byte array.</param>
        /// <param name="curve">The ECCurve associated with the public key.</param>
        /// <returns>A new ECDsa instance.</returns>
        ECDsa CreateECDsa(byte[] publicKey, ECCurve curve);

        /// <summary>
        /// Imports a private key from a byte array into an ECDsa instance.
        /// </summary>
        /// <param name="privateKey">The private key as a read-only span of bytes.</param>
        /// <param name="curve">The ECCurve associated with the private key.</param>
        /// <returns>An ECDsa instance initialized with the given private key.</returns>
        ECDsa ImportECDsa(ReadOnlySpan<byte> privateKey, ECCurve curve);

        /// <summary>
        /// Imports a private key from a PEM-formatted string into an ECDsa instance.
        /// </summary>
        /// <param name="privateKey">The private key in PEM format.</param>
        /// <param name="curve">The ECCurve associated with the private key.</param>
        /// <returns>An ECDsa instance initialized with the given private key.</returns>
        ECDsa ImportECDsa(string privateKey, ECCurve curve);

        /// <summary>
        /// Clears PEM formatting from a private key, preparing it for use.
        /// </summary>
        /// <param name="privateKey">The private key in PEM format.</param>
        /// <returns>The private key cleaned of PEM formatting.</returns>
        string ClearPEMPrivateKey(string privateKey);

        /// <summary>
        /// Clears PEM formatting from a public key, preparing it for use.
        /// </summary>
        /// <param name="publicKey">The public key in PEM format.</param>
        /// <returns>The public key cleaned of PEM formatting.</returns>
        string ClearPEMPublicKey(string publicKey);
    }
}