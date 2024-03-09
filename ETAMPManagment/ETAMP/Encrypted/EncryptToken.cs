using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    public class EncryptToken : IEncryptToken
    {
        private readonly IStructureValidator _structureValidator;
        private readonly IEciesEncryptionService _eciesEncryptionService;

        public EncryptToken(IStructureValidator structureValidator, IEciesEncryptionService eciesEncryptionService)
        {
            _structureValidator = structureValidator;
            _eciesEncryptionService = eciesEncryptionService;
        }

        public virtual ETAMPModel EncryptETAMP(string jsonEtamp)
        {
            var valid = _structureValidator.IsValidEtampFormat(jsonEtamp);
            if (valid.isValid)
            {
                valid.model.Token = _eciesEncryptionService.Encrypt(valid.model.Token);
                return valid.model;
            }
            throw new InvalidOperationException("ETAMP data is invalid. Ensure the JSON structure includes all required fields and matches the expected format.");
        }

        public virtual string EncryptETAMPToken(string jsonEtamp)
        {
            return JsonConvert.SerializeObject(EncryptETAMP(jsonEtamp));
        }
    }
}