using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    public class ETAMPEncrypted : IETAMPEncrypted
    {
        private readonly IETAMPBase _etampBase;
        private readonly IEciesEncryptionService _eciesEncryptionService;

        public ETAMPEncrypted(IETAMPBase etampBase, IEciesEncryptionService eciesEncryptionService)
        {
            _etampBase = etampBase;
            _eciesEncryptionService = eciesEncryptionService;
        }

        public virtual string CreateEncryptETAMPToken<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            ETAMPModel model = _etampBase.CreateETAMPModel(updateType, payload, version);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return JsonConvert.SerializeObject(model);
        }

        public virtual ETAMPModel CreateEncryptETAMPTokenModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            ETAMPModel model = _etampBase.CreateETAMPModel(updateType, payload, version);
            model.Token = _eciesEncryptionService.Encrypt(model.Token);
            return model;
        }
    }
}