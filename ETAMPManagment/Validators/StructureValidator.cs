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
        /// Validates the format of a given ETAMP string to ensure it's a valid JSON and can be deserialized into an ETAMP model.
        /// </summary>
        /// <param name="etamp">The ETAMP string to validate.</param>
        /// <returns>The deserialized ETAMP model if the string is a valid JSON representation of an ETAMP.</returns>
        /// <exception cref="ArgumentException">Thrown when the ETAMP string is invalid or cannot be deserialized into an ETAMP model.</exception>
        public virtual ETAMPModel IsValidEtampFormat(string etamp)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(etamp));

            ETAMPModel? model;
            try
            {
                model = JsonConvert.DeserializeObject<ETAMPModel>(etamp);
                if (model == null || model.Id == Guid.Empty)
                    throw new ArgumentException("Failed to deserialize ETAMP to model", nameof(etamp));
                return model;
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
        /// <returns>A ValidationResult indicating whether the ETAMP structure is valid.</returns>
        public virtual ValidationResult ValidateETAMPStructure(string etamp)
        {
            var model = IsValidEtampFormat(etamp);
            if (model == null ||
                string.IsNullOrWhiteSpace(model.Token) ||
                string.IsNullOrWhiteSpace(model.UpdateType) ||
                string.IsNullOrWhiteSpace(model.SignatureToken) ||
                string.IsNullOrWhiteSpace(model.SignatureMessage))
            {
                return new ValidationResult(false, "Deserialized ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values");
            }
            return new ValidationResult(true);
        }

        /// <summary>
        /// Validates a simplified structure of a given ETAMP string, focusing on essential fields only.
        /// </summary>
        /// <param name="etamp">The ETAMP string to validate.</param>
        /// <returns>A ValidationResult indicating whether the simplified ETAMP structure is valid.</returns>
        public virtual ValidationResult ValidateETAMPStructureLite(string etamp)
        {
            var model = IsValidEtampFormat(etamp);
            if (model == null ||
               string.IsNullOrWhiteSpace(model.Token) ||
               string.IsNullOrWhiteSpace(model.UpdateType))
            {
                return new ValidationResult(false, "Deserialized ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values.");
            }
            return new ValidationResult(true);
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
                throw new InvalidOperationException("JWT validator is not initialized. Ensure that a JWT validator is provided.");

            var model = IsValidEtampFormat(etamp);
            if (_jwtValidator.IsValidJwtToken(model.Token).IsValid)
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var tokenData = jwtSecurityTokenHandler.ReadJwtToken(model?.Token).Payload.ToDictionary();
                return tokenData.ContainsKey("MessageId") && tokenData["MessageId"].ToString().ToLower() == model.Id.ToString().ToLower();
            }
            return false;
        }

        /// <summary>
        /// Validates the structure of an ETAMP model to ensure all required fields are present and valid.
        /// </summary>
        /// <param name="model">The ETAMP model to validate.</param>
        /// <returns>A ValidationResult indicating whether the ETAMP model structure is valid.</returns>
        public virtual ValidationResult ValidateETAMPStructure(ETAMPModel model)
        {
            if (model == null ||
                model.Id == Guid.Empty ||
                string.IsNullOrWhiteSpace(model.Token) ||
                string.IsNullOrWhiteSpace(model.UpdateType) ||
                string.IsNullOrWhiteSpace(model.SignatureToken) ||
                string.IsNullOrWhiteSpace(model.SignatureMessage))
            {
                return new ValidationResult(false, "ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values");
            }
            return new ValidationResult(true);
        }

        /// <summary>
        /// Validates a simplified structure of an ETAMP model, focusing on essential fields only.
        /// </summary>
        /// <param name="model">The ETAMP model to validate.</param>
        /// <returns>A ValidationResult indicating whether the simplified ETAMP model structure is valid.</returns>
        public virtual ValidationResult ValidateETAMPStructureLite(ETAMPModel model)
        {
            if (model == null ||
                model.Id == Guid.Empty ||
                string.IsNullOrWhiteSpace(model.Token) ||
                string.IsNullOrWhiteSpace(model.UpdateType))
            {
                return new ValidationResult(false, "ETAMP model is invalid: it is either null, has empty/missing fields, or contains invalid values");
            }
            return new ValidationResult(true);
        }
    }
}