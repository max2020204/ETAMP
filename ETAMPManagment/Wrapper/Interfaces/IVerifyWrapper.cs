namespace ETAMPManagment.Wrapper.Interfaces
{
    /// <summary>
    /// Provides functionality for verifying signatures using ECDsa.
    /// </summary>
    public interface IVerifyWrapper : IDisposable
    {
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
    }
}