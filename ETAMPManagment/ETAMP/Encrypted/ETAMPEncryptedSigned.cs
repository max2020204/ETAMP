using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Newtonsoft.Json;

namespace ETAMPManagment.ETAMP.Encrypted
{
    public class ETAMPEncryptedSigned : ETAMPEncrypted
    {
        private readonly ISignWrapper _signWrapper;

        public ETAMPEncryptedSigned(IETAMPBase etampBase, ISignWrapper signWrapper, IEciesEncryptionService eciesEncryptionService) : base(etampBase, eciesEncryptionService)
        {
            _signWrapper = signWrapper;
        }

        public override string CreateEncryptETAMPToken<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(this.CreateEncryptETAMPTokenModel(updateType, payload, version));
        }

        public override ETAMPModel CreateEncryptETAMPTokenModel<T>(string updateType, T payload, double version = 1)
        {
            return _signWrapper.SignEtampModel(base.CreateEncryptETAMPTokenModel(updateType, payload, version));
        }
    }
}