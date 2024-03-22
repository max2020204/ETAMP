using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ETAMPManagment.ETAMP.Base
{
    /// <summary>
    /// Implements the ETAMP data processing logic, utilizing a signing credentials provider to sign the data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the ETAMPData class with the specified signing credentials provider.
    /// </remarks>
    /// <param name="signingCredentialsProvider">The provider used to create signing credentials.</param>
    public class ETAMPData(ISigningCredentialsProvider signingCredentialsProvider) : IETAMPData
    {
        /// <inheritdoc />
        public virtual string CreateEtampData<T>(string messageId, T payload) where T : BasePayload
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
                SigningCredentials = signingCredentialsProvider.CreateSigningCredentials()
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}