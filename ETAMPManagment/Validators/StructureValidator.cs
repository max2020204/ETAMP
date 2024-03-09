using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ETAMPManagment.Validators
{
    public class StructureValidator : IStructureValidator
    {
        private readonly IJwtValidator _jwtValidator;

        public StructureValidator(IJwtValidator jwtValidator)
        {
            _jwtValidator = jwtValidator;
        }

        public StructureValidator()
        {
        }

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