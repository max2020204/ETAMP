using ETAMPManagment.Models;

namespace ETAMPManagment.Wrapper.Interfaces
{
    /// <summary>
    /// Provides functionality for signing ETAMP messages.
    /// </summary>
    public interface ISignWrapper
    {
        /// <summary>
        /// Signs an ETAMP message provided as a JSON string and returns the signed message as a string.
        /// </summary>
        /// <param name="jsonEtamp">The ETAMP message in JSON format.</param>
        /// <returns>The signed ETAMP message as a JSON string.</returns>
        string SignEtamp(string jsonEtamp);

        /// <summary>
        /// Signs an ETAMP message provided as an ETAMPModel and returns the signed message as a string.
        /// </summary>
        /// <param name="etamp">The ETAMP message as an ETAMPModel.</param>
        /// <returns>The signed ETAMP message as a string.</returns>
        string SignEtamp(ETAMPModel etamp);

        /// <summary>
        /// Signs an ETAMPModel and returns a new ETAMPModel that includes the signature.
        /// </summary>
        /// <param name="etamp">The ETAMPModel to be signed.</param>
        /// <returns>A new ETAMPModel that includes the signature, enhancing the original model.</returns>
        ETAMPModel SignEtampModel(ETAMPModel etamp);
    }
}