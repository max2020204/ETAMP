using System.Security.Cryptography;

namespace ETAMPManagment.Wrapper.Interfaces
{
    /// <summary>
    /// Provides functionality for verifying signatures using ECDsa.
    /// </summary>
    public interface IVerifyWrapper : IDisposable
    {
        /// <summary>
        /// Gets the ECDsa instance used for verification.
        /// </summary>
        ECDsa ECDsa { get; }

        /// <summary>
        /// Verifies the signature of a given string data.
        /// </summary>
        /// <param name="data">The data in string format.</param>
        /// <param name="signature">The signature in string format.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        bool VerifyData(string data, string signature);

        /// <summary>
        /// Verifies the signature of a given byte array data.
        /// </summary>
        /// <param name="data">The data as a byte array.</param>
        /// <param name="signature">The signature as a byte array.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        bool VerifyData(byte[] data, byte[] signature);

        /// <summary>
        /// Verifies the signature of given string data against a byte array signature.
        /// </summary>
        /// <param name="data">The data in string format.</param>
        /// <param name="signature">The signature as a byte array.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        bool VerifyData(string data, byte[] signature);

        /// <summary>
        /// Verifies the signature of given byte array data against a string signature.
        /// </summary>
        /// <param name="data">The data as a byte array.</param>
        /// <param name="signature">The signature in string format.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        bool VerifyData(byte[] data, string signature);
    }
}