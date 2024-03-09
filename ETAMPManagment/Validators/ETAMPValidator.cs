using ETAMPManagment.Validators.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ETAMPManagment.Validators
{
    public class ETAMPValidator : IETAMPValidator
    {
        private readonly IJwtValidator _jwtValidator;
        private readonly IStructureValidator _structureValidator;
        private readonly ISignatureValidator _signatureValidator;

        public ETAMPValidator(IJwtValidator jwtValidator, IStructureValidator structureValidator, ISignatureValidator signatureValidator)
        {
            _jwtValidator = jwtValidator;
            _structureValidator = structureValidator;
            _signatureValidator = signatureValidator;
        }

        public virtual async Task<bool> ValidateETAMP(string etamp, string audience, string issuer, ECDsaSecurityKey tokenSecurityKey)
        {
            var structure = _structureValidator.IsValidEtampFormat(etamp);
            if (structure.isValid)
            {
                List<bool> valid = new List<bool>()
                {
                    _structureValidator.ValidateETAMPStructure(etamp),
                    await _jwtValidator.ValidateToken(structure.model.Token,audience,issuer,tokenSecurityKey),
                    _signatureValidator.ValidateToken(structure.model.Token,structure.model.SignatureToken),
                    _signatureValidator.ValidateETAMPMessage(structure.model)
                };
                return valid.TrueForAll(x => x);
            }
            return false;
        }

        public virtual async Task<bool> ValidateETAMP(string etamp, ECDsaSecurityKey tokenSecurityKey)
        {
            var structure = _structureValidator.IsValidEtampFormat(etamp);
            if (structure.isValid)
            {
                List<bool> valid = new List<bool>()
                {
                    _structureValidator.ValidateETAMPStructure(etamp),
                    await _jwtValidator.ValidateLifeTime(structure.model.Token,tokenSecurityKey),
                    _signatureValidator.ValidateToken(structure.model.Token,structure.model.SignatureToken),
                    _signatureValidator.ValidateETAMPMessage(structure.model)
                };
                return valid.TrueForAll(x => x);
            }
            return false;
        }

        public virtual async Task<bool> ValidateETAMPLite(string etamp, ECDsaSecurityKey tokenSecurityKey)
        {
            var structure = _structureValidator.IsValidEtampFormat(etamp);
            if (structure.isValid)
            {
                List<bool> valid = new List<bool>()
                {
                    _structureValidator.ValidateETAMPStructure(etamp),
                    await _jwtValidator.ValidateLifeTime(structure.model.Token,tokenSecurityKey)
                };
                return valid.TrueForAll(x => x);
            }
            return false;
        }
    }
}