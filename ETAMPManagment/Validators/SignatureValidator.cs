using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using ETAMPManagment.Wrapper.Interfaces;

namespace ETAMPManagment.Validators
{
    public class SignatureValidator : ISignatureValidator
    {
        private readonly IVerifyWrapper _verifyWrapper;
        private readonly IStructureValidator _structureValidator;

        public SignatureValidator(IVerifyWrapper verifyWrapper)
        {
            _verifyWrapper = verifyWrapper;
        }

        public SignatureValidator(IStructureValidator structureValidator)
        {
            _structureValidator = structureValidator;
        }

        public virtual bool ValidateETAMPMessage(string etamp)
        {
            if (_structureValidator == null)
            {
                throw new InvalidOperationException("IStructureValidator is not initialized. Ensure that validator is provided.");
            }
            var valid = _structureValidator.IsValidEtampFormat(etamp);
            if (valid.isValid && _structureValidator.ValidateETAMPStructure(valid.model))
            {
                return _verifyWrapper.VerifyData($"{valid.model.Id}{valid.model.Version}{valid.model.Token}{valid.model.UpdateType}{valid.model.SignatureToken}", valid.model.SignatureMessage);
            }
            return false;
        }

        public virtual bool ValidateETAMPMessage(ETAMPModel etamp)
        {
            return _verifyWrapper.VerifyData($"{etamp.Id}{etamp.Version}{etamp.Token}{etamp.UpdateType}{etamp.SignatureToken}", etamp.SignatureMessage);
        }

        public virtual bool ValidateToken(string token, string tokenSignature)
        {
            return _verifyWrapper.VerifyData(token, tokenSignature);
        }
    }
}