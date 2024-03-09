using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ETAMPManagment.ETAMP.Base
{
    public class ETAMPSign : ETAMPBase
    {
        private readonly ISignWrapper _signWrapper;

        public ETAMPSign(ECDsa ecdsa, ISignWrapper signWrapper, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature) : base(ecdsa, securityAlgorithm)
        {
            _signWrapper = signWrapper;
        }

        public override string CreateETAMP<T>(string updateType, T payload, double version = 1)
        {
            return JsonConvert.SerializeObject(CreateETAMPModel(updateType, payload, version));
        }

        public override ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1)
        {
            return _signWrapper.SignEtampModel(base.CreateETAMPModel(updateType, payload, version));
        }
    }
}