using ETAMPManagment.Models;
using ETAMPManagmentTests.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Tests
{
    public class EtampTests
    {
        private ETAMP? _etamp;
        private readonly Data _data;

        public EtampTests()
        {
            _data = new Data()
            {
                Expires = DateTime.UtcNow.AddHours(1),
                Audience = "Someone",
                Surname = "Neo",
                Name = "Alex",
                IssuedAt = DateTime.UtcNow,
                Issuer = "Mike",
                JTI = Guid.NewGuid()
            };
        }

        [Theory]
        [InlineData("nistP256")]
        [InlineData("nistP384")]
        [InlineData("nistP521")]
        public void CreateETAMPWithSignature_GetEqualResult(string curve)
        {
            _etamp = new ETAMP(curve: ECCurve.CreateFromFriendlyName(curve));
            string updateType = "Message";
            double version = 2;

            string result = _etamp.CreateETAMP(updateType, _data, true, version);

            var etamp = JsonConvert.DeserializeObject<ETAMPModel>(result);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(etamp.Token).Payload.ToDictionary();

            Assert.NotNull(etamp);
            Assert.Equal(version, etamp.Version);
            Assert.Equal(updateType, etamp.UpdateType);
            Assert.Equal(etamp.Id.ToString(), payload["MessageId"].ToString());
        }

        [Theory]
        [InlineData("nistP256")]
        [InlineData("nistP384")]
        [InlineData("nistP521")]
        public void CreateETAMPWithoutSignatureAndWithTokenSignature_GetEqualResult(string curve)
        {
            _etamp = new ETAMP(curve: ECCurve.CreateFromFriendlyName(curve));
            string updateType = "Message";
            double version = 2;

            string result = _etamp.CreateETAMPWithoutSignature(updateType, _data, true, version);

            var etamp = JsonConvert.DeserializeObject<ETAMPModel>(result);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(etamp.Token).Payload.ToDictionary();

            Assert.NotNull(etamp);
            Assert.Equal(version, etamp.Version);
            Assert.Equal(updateType, etamp.UpdateType);
            Assert.Equal(etamp.Id.ToString(), payload["MessageId"].ToString());
            Assert.Null(etamp.SignatureToken);
            Assert.Null(etamp.SignatureMessage);
        }

        [Theory]
        [InlineData("nistP256")]
        [InlineData("nistP384")]
        [InlineData("nistP521")]
        public void CreateETAMPWithoutSignatureAndWithoutTokenSignature_GetEqualResult(string curve)
        {
            _etamp = new ETAMP(curve: ECCurve.CreateFromFriendlyName(curve));
            string updateType = "Message";
            double version = 2;

            string result = _etamp.CreateETAMPWithoutSignature(updateType, _data, false, version);

            var etamp = JsonConvert.DeserializeObject<ETAMPModel>(result);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(etamp.Token).Payload.ToDictionary();

            Assert.NotNull(etamp);
            Assert.Equal(version, etamp.Version);
            Assert.Equal(updateType, etamp.UpdateType);
            Assert.Equal(etamp.Id.ToString(), payload["MessageId"].ToString());
            Assert.Null(etamp.SignatureToken);
            Assert.Null(etamp.SignatureMessage);
        }

        [Fact]
        public void CreateETAMP_ReturnPrivateKey()
        {
            _etamp = new ETAMP();
            Assert.NotNull(_etamp.PrivateKey);
        }

        [Fact]
        public void CreateETAMP_ReturnPublicKey()
        {
            _etamp = new ETAMP();
            Assert.NotNull(_etamp.PublicKey);
        }

        [Fact]
        public void CreateETAMPWithEcdsaParametr_ReturnPrivateKey()
        {
            ECDsa eCDsa = ECDsa.Create();
            _etamp = new ETAMP(eCDsa);
            Assert.NotNull(_etamp.PrivateKey);
        }

        [Fact]
        public void CreateETAMPWithEcdsaParametr_ReturnPublicKey()
        {
            ECDsa eCDsa = ECDsa.Create();
            _etamp = new ETAMP(eCDsa);
            Assert.NotNull(_etamp.PublicKey);
        }

        [Fact]
        public void CreateETAMP_ReturnCurve()
        {
            _etamp = new ETAMP();
            Assert.Equal(ECCurve.NamedCurves.nistP521.Oid.FriendlyName, _etamp.Curve.Oid.FriendlyName);
        }

        [Theory]
        [InlineData("SHA1")]
        [InlineData("SHA256")]
        public void CreateETAMP_ReturnHashAlgorithm(string hashAlgorithmName)
        {
            var hashAlgorithm = new HashAlgorithmName(hashAlgorithmName);
            _etamp = new ETAMP(hash: hashAlgorithm);
            Assert.Equal(hashAlgorithm, _etamp.HashAlgorithm);
        }

        [Theory]
        [InlineData(SecurityAlgorithms.EcdhEs)]
        public void CreateETAMP_ReturnSecurityAlgorithm(string securityAlgorthm)
        {
            _etamp = new ETAMP(securityAlgorthm: securityAlgorthm);
            Assert.NotNull(_etamp.SecurityAlgorithm);
            Assert.Equal(_etamp.SecurityAlgorithm, securityAlgorthm);
        }

        [Fact]
        public void CreateETAMP_ReturnEcdsa()
        {
            _etamp = new ETAMP();
            Assert.NotNull(_etamp.Ecdsa);
        }

        [Fact]
        public void CreateETAMPWithParametr_ReturnEcdsa()
        {
            ECDsa eCDsa = ECDsa.Create();
            _etamp = new ETAMP(eCDsa);
            Assert.NotNull(_etamp.Ecdsa);
        }
    }
}