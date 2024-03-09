using ETAMPManagment.Models;

namespace ETAMPManagment.Wrapper.Interfaces
{
    /// <summary>
    /// Provides functionality for signing ETAMP messages.
    /// </summary>
    public interface ISignWrapper
    {
        /// <summary>
        /// Signs an ETAMP message given as a JSON string.
        /// </summary>
        /// <param name="jsonEtamp">The ETAMP message in JSON format.</param>
        /// <returns>The signed ETAMP message as a string.</returns>
        string SignEtamp(string jsonEtamp);

        /// <summary>
        /// Signs an ETAMP message provided as an ETAMPModel.
        /// </summary>
        /// <param name="etamp">The ETAMP message as an ETAMPModel.</param>
        /// <returns>The signed ETAMP message as a string.</returns>
        string SignEtamp(ETAMPModel etamp);

        /// <summary>
        /// Signs an ETAMPModel and returns a modified ETAMPModel with the signature.
        /// </summary>
        /// <param name="etamp">The ETAMPModel to be signed.</param>
        /// <returns>A new ETAMPModel that includes the signature.</returns>
        ETAMPModel SignEtampModel(ETAMPModel etamp);
    }
}