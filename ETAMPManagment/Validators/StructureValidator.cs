using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagment.Validators
{
    /// <summary>
    /// Provides functionality for validating the structure of ETAMP messages, including JSON format, field consistency, and signature presence.
    /// </summary>
    public class StructureValidator : IStructureValidator
    {
        private readonly IJwtValidator _jwtValidator;

        /// <summary>
        /// Initializes a new instance of the StructureValidator class with a specified JWT validator.
        /// </summary>
        /// <param name="jwtValidator">The JWT validator to use for JWT token validation.</param>
        public StructureValidator(IJwtValidator jwtValidator)
        {
            _jwtValidator = jwtValidator;
        }

        /// <summary>
        /// Initializes a new instance of the StructureValidator class without a specific JWT validator.
        /// </summary>
        public StructureValidator()
        {
        }

        /// <summary>
        /// Validates the format of a given ETAMP string to ensure it's a valid JSON and can be deserialized into an ETAMP model.
        /// </summary>
        /// <param name="etamp">The ETAMP string to validate.</param>
        /// <returns>A tuple indicating whether the ETAMP is valid and the deserialized ETAMP model.</returns>
        /// <exception cref="ArgumentException">Thrown when the ETAMP string is invalid or cannot be deserialized.</exception>
        public virtual (bool isValid, ETAMPModel model) IsValidEtampFormat(string etamp)
        {
            if (string.IsNullOrEmpty(etamp))
            {
                throw new ArgumentException("JSON ETAMP cannot be null or empty", nameof(etamp));
            }

            ETAMPModel? model;
            try
            {
                model = JsonConvert.DeserializeObject<ETAMPModel>(etamp);
                if (model == null)
                {
                    throw new ArgumentException("Failed to deserialize ETAMP to model", nameof(etamp));
                }
                return (true, model);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Invalid JSON ETAMP format", nameof(etamp), ex);
            }
        }

        /// <summary>
        /// Validates the structure of a given ETAMP string to ensure all required fields are present and valid.
        /// </summary>
        /// <param name="etamp">The ETAMP string to validate.</param>
        /// <returns>True if the ETAMP structure is valid, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the deserialized ETAMP model contains invalid values.</exception>
        public virtual bool ValidateETAMPStructure(string etamp)
        {
            var valid = IsValidEtampFormat(etamp);
            if (valid.isValid)
            {
                if (valid.model == null ||
                    valid.model.Id == Guid.Empty ||
                    valid.model.Version == default ||
                    string.IsNullOrWhiteSpace(valid.model.Token) ||
                    string.IsNullOrWhiteSpace(valid.model.UpdateType) ||
                    string.IsNullOrWhiteSpace(valid.model.SignatureToken) ||
                    string.IsNullOrWhiteSpace(valid.model.SignatureMessage))
                {
                    throw new InvalidOperationException("Deserialized ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values.");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates a simplified structure of a given ETAMP string, focusing on essential fields only.
        /// </summary>
        /// <param name="etamp">The ETAMP string to validate.</param>
        /// <returns>True if the simplified ETAMP structure is valid, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the deserialized ETAMP model contains invalid values.</exception>
        public virtual bool ValidateETAMPStructureLite(string etamp)
        {
            var valid = IsValidEtampFormat(etamp);
            if (valid.isValid)
            {
                if (valid.model == null ||
                    valid.model.Id == Guid.Empty ||
                    valid.model.Version == default ||
                    string.IsNullOrWhiteSpace(valid.model.Token) ||
                    string.IsNullOrWhiteSpace(valid.model.UpdateType))
                {
                    throw new InvalidOperationException("Deserialized ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values.");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the consistency of the ETAMP ID across its components.
        /// </summary>
        /// <param name="etamp">The ETAMP string to validate.</param>
        /// <returns>True if the ETAMP ID is consistent, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the JWT validator is not initialized.</exception>
        public virtual bool ValidateIdConsistency(string etamp)
        {
            if (_jwtValidator == null)
            {
                throw new InvalidOperationException("JWT validator is not initialized. Ensure that a JWT validator is provided.");
            }
            var valid = IsValidEtampFormat(etamp);
            if (valid.isValid && _jwtValidator.IsValidJwtToken(valid.model.Token))
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var tokenData = jwtSecurityTokenHandler.ReadJwtToken(valid.model?.Token).Payload.ToDictionary();
                if (!tokenData.ContainsKey("MessageId") || tokenData["MessageId"].ToString() != valid.model.Id.ToString())
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the structure of an ETAMP model to ensure all required fields are present and valid.
        /// </summary>
        /// <param name="model">The ETAMP model to validate.</param>
        /// <returns>True if the ETAMP model structure is valid, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the ETAMP model contains invalid values.</exception>
        public virtual bool ValidateETAMPStructure(ETAMPModel model)
        {
            if (model == null ||
                model.Id == Guid.Empty ||
                model.Version == default ||
                string.IsNullOrWhiteSpace(model.Token) ||
                string.IsNullOrWhiteSpace(model.UpdateType))
            {
                throw new InvalidOperationException("ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values.");
            }
            return true;
        }

        /// <summary>
        /// Validates a simplified structure of an ETAMP model, focusing on essential fields only.
        /// </summary>
        /// <param name="model">The ETAMP model to validate.</param>
        /// <returns>True if the simplified ETAMP model structure is valid, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the ETAMP model contains invalid values.</exception>
        public virtual bool ValidateETAMPStructureLite(ETAMPModel model)
        {
            if (model == null ||
                model.Id == Guid.Empty ||
                model.Version == default ||
                string.IsNullOrWhiteSpace(model.Token) ||
                string.IsNullOrWhiteSpace(model.UpdateType) ||
                string.IsNullOrWhiteSpace(model.SignatureToken) ||
                string.IsNullOrWhiteSpace(model.SignatureMessage))
            {
                throw new InvalidOperationException("ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values.");
            }
            return true;
        }
    }
}