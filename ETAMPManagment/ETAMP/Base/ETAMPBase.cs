using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ETAMPManagment.ETAMP.Base
{
    public class ETAMPBase : IETAMPBase
    {
        private readonly ECDsa _ecdsa;
        private readonly string _securityAlgorithm;

        public ETAMPBase(ECDsa ecdsa, string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature)
        {
            _ecdsa = ecdsa;
            _securityAlgorithm = securityAlgorithm;
        }

        public ETAMPBase(string securityAlgorithm = SecurityAlgorithms.EcdsaSha512Signature)
        {
            _ecdsa = ECDsa.Create();
            _securityAlgorithm = securityAlgorithm;
        }

        public virtual string CreateETAMP<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            return JsonConvert.SerializeObject(CreateETAMPModel(updateType, payload, version));
        }

        public virtual ETAMPModel CreateETAMPModel<T>(string updateType, T payload, double version = 1) where T : BasePaylaod
        {
            Guid messageId = Guid.NewGuid();
            string token = CreateEtampData(messageId.ToString(), payload);
            return new()
            {
                Id = messageId,
                Version = version,
                Token = token,
                UpdateType = updateType
            };
        }

        private string CreateEtampData<T>(string messageId, T payload) where T : BasePaylaod
        {
            var handler = new JwtSecurityTokenHandler();
            string payloadJson = JsonConvert.SerializeObject(payload);
            var claimsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);

            var descriptor = new SecurityTokenDescriptor
            {
                TokenType = "ETAMP",
                Claims = claimsDictionary,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("MessageId", messageId),
                    new Claim("Timestamp", DateTimeOffset.UtcNow.ToString())
                }),
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(_ecdsa), _securityAlgorithm)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}