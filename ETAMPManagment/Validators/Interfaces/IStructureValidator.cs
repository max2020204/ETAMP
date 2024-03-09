using ETAMPManagment.Models;

namespace ETAMPManagment.Validators.Interfaces
{
    /// <summary>
    /// Defines methods for validating the structure and consistency of ETAMP tokens and models.
    /// </summary>
    public interface IStructureValidator
    {
        /// <summary>
        /// Validates the format of an ETAMP token and returns the deserialized model if valid.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated.</param>
        /// <returns>A tuple containing a boolean indicating validity and the deserialized ETAMP model.</returns>
        (bool isValid, ETAMPModel model) IsValidEtampFormat(string etamp);

        /// <summary>
        /// Validates the consistency of identifiers within an ETAMP token.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated for identifier consistency.</param>
        /// <returns><c>true</c> if the identifier is consistent; otherwise, <c>false</c>.</returns>
        bool ValidateIdConsistency(string etamp);

        /// <summary>
        /// Validates the structure of an ETAMP token against the expected schema.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated for structural integrity.</param>
        /// <returns><c>true</c> if the structure is valid; otherwise, <c>false</c>.</returns>
        bool ValidateETAMPStructure(string etamp);

        /// <summary>
        /// Validates the structure of an ETAMP model against the expected schema.
        /// </summary>
        /// <param name="model">The ETAMPModel object to be validated for structural integrity.</param>
        /// <returns><c>true</c> if the structure is valid; otherwise, <c>false</c>.</returns>
        bool ValidateETAMPStructure(ETAMPModel model);

        /// <summary>
        /// Performs a lightweight structure validation of an ETAMP token.
        /// </summary>
        /// <param name="etamp">The ETAMP token as a JSON string to be validated for basic structural integrity.</param>
        /// <returns><c>true</c> if the basic structure is valid; otherwise, <c>false</c>.</returns>
        bool ValidateETAMPStructureLite(string etamp);

        /// <summary>
        /// Performs a lightweight structure validation of an ETAMP model.
        /// </summary>
        /// <param name="model">The ETAMPModel object to be validated for basic structural integrity.</param>
        /// <returns><c>true</c> if the basic structure is valid; otherwise, <c>false</c>.</returns>
        bool ValidateETAMPStructureLite(ETAMPModel model);
    }
}