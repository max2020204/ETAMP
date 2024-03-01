using ETAMP.Models;
using ETAMP.Services;
using ETAMP.Wrapper;
using ETAMP.Wrapper.Interfaces;
using ETAMPTests.Models;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace ETAMPTests.Services
{
    public class ValidateTokenTests
    {
        private readonly ETAMP.ETAMP _etamp;
        private readonly Data _data;
        private readonly ECDsa _ecdsa;
        private ValidateToken _token;
        private readonly Mock<IVerifyWrapper> _verifyWrapperMock;
        private TestableValidateToken _testableValidateToken;
        private readonly Mock<JwtSecurityTokenHandler> _jwtSecurityTokenHandler;

        public ValidateTokenTests()
        {
            _ecdsa = ECDsa.Create();

            _etamp = new ETAMP.ETAMP();
            _data = new Data()
            {
                Audience = "Someone",
                Expires = DateTime.UtcNow.AddDays(10),
                IssuedAt = DateTime.UtcNow,
                Issuer = "Me",
                JTI = Guid.NewGuid(),
                Surname = "Bill",
                Name = "Alex"
            };
            _verifyWrapperMock = new Mock<IVerifyWrapper>();
            _jwtSecurityTokenHandler = new Mock<JwtSecurityTokenHandler>();
            _testableValidateToken = new TestableValidateToken(_verifyWrapperMock.Object);
            _token = new ValidateToken(_verifyWrapperMock.Object);
        }

        [Fact]
        public void VerifyETAMP_WhithNull_ReturnFalse()
        {
            bool result = _token.VerifyETAMP("");
            Assert.False(result);
        }

        [Fact]
        public void VerifyETAMP_ModelIsNull_ReturnFalse()
        {
            bool result = _token.VerifyETAMP("{}");
            Assert.False(result);
        }

        [Fact]
        public void VerifyETAMP_DifferentId_ReturnFalse()
        {
            bool result = _token.VerifyETAMP($"{{Id:\"{Guid.NewGuid()}\", Token: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c\"}}");
            Assert.False(result);
        }

        [Fact]
        public void VerifyETAMP_WithRealData_ReturnTrue()
        {
            string etamp = _etamp.CreateETAMP("Message", _data);
            ValidateToken validate = new ValidateToken(new VerifyWrapper(_etamp.Ecdsa, _etamp.HashAlgorithm));
            bool result = validate.VerifyETAMP(etamp);
            Assert.True(result);
        }

        [Fact]
        public void VerifyETAMP_DifferentSignatureToken_ReturnFalse()
        {
            _verifyWrapperMock.Setup(x => x.VerifyData(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(false);
            _token = new ValidateToken(_verifyWrapperMock.Object);
            bool result = _token.VerifyETAMP($"{{Id:\"bf4e7e00-aa07-42a7-825c-da6178119d2b\", Token: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJNZXNzYWdlSWQiOiJiZjRlN2UwMC1hYTA3LTQyYTctODI1Yy1kYTYxNzgxMTlkMmIifQ.upbMMn6Ztz0b_olsQAKBLrRYL7IiczGdNgVrG1P-6s\",SignatureToken: \"JNZXNzYWdlSWQiOiJiZjRlN2UwMC1hYTA3LTQyYTctODI1Yy1kYTYxNzgxMTlkMmIifQ\"}}");
            Assert.False(result);
        }

        [Fact]
        public void VerifyETAMP_DifferentSignatureMessage_ReturnFalse()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJNZXNzYWdlSWQiOiJiZjRlN2UwMC1hYTA3LTQyYTctODI1Yy1kYTYxNzgxMTlkMmIifQ.upbMMn6Ztz0b_olsQAKBLrRYL7IiczGdNgVrG1P-6s";
            ECDsa ecdsa = ECDsa.Create();
            _token = new ValidateToken(new VerifyWrapper(ecdsa, HashAlgorithmName.SHA256));
            string signToken = Convert.ToBase64String(ecdsa.SignData(Encoding.UTF8.GetBytes(token), HashAlgorithmName.SHA256));
            bool result = _token.VerifyETAMP($"{{Id:\"bf4e7e00-aa07-42a7-825c-da6178119d2b\", Token: \"{token}\",SignatureToken: \"{signToken}\", SignatureMessage:\"8ca66ee6b2fe4bb928a8e3cd2f508de4119c0895f22e011117e22cf9b13de7ef\"}}");
            Assert.False(result);
        }

        [Fact]
        public void VerifyETAMP_CorrectInput_ReturnTrue()
        {
            string id = "bf4e7e00-aa07-42a7-825c-da6178119d2b";
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJNZXNzYWdlSWQiOiJiZjRlN2UwMC1hYTA3LTQyYTctODI1Yy1kYTYxNzgxMTlkMmIifQ.upbMMn6Ztz0b_olsQAKBLrRYL7IiczGdNgVrG1P-6s";
            string signToken = Convert.ToBase64String(_ecdsa.SignData(Encoding.UTF8.GetBytes(token), HashAlgorithmName.SHA256));
            double version = 1;
            string updateType = "Message";
            string signMessage = Convert.ToBase64String(_ecdsa.SignData(Encoding.UTF8.GetBytes($"{id}{version}{token}{updateType}{signToken}"), HashAlgorithmName.SHA256));
            ETAMPModel model = new ETAMPModel()
            {
                Id = Guid.Parse(id),
                Token = token,
                UpdateType = updateType,
                Version = version,
                SignatureToken = signToken,
                SignatureMessage = signMessage
            };
            _verifyWrapperMock.Setup(x => x.VerifyData(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(true);
            bool result = _token.VerifyETAMP(JsonConvert.SerializeObject(model));
            Assert.True(result);
        }

        [Fact]
        public async Task FullVerify_WithRealData_ReturnTrue()
        {
            string etamp = _etamp.CreateETAMP("Message", _data, false);
            ValidateToken validate = new ValidateToken(new VerifyWrapper(_etamp.Ecdsa, _etamp.HashAlgorithm));
            bool result = await validate.FullVerify(etamp, _data.Audience, _data.Issuer);
            Assert.True(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithRealData_ReturnTrue()
        {
            string etamp = _etamp.CreateETAMP("Message", _data);
            ValidateToken validate = new ValidateToken(new VerifyWrapper(_etamp.Ecdsa, _etamp.HashAlgorithm));
            bool result = await validate.FullVerify(etamp, _data.Audience, _data.Issuer);
            Assert.True(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_IncorrectInput_ReturnFalse()
        {
            string id = "bf4e7e00-aa07-42a7-825c-da6178119d2b";
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJNZXNzYWdlSWQiOiJiZjRlN2UwMC1hYTA3LTQyYTctODI1Yy1kYTYxNzgxMTlkMmIifQ.upbMMn6Ztz0b_olsQAKBLrRYL7IiczGdNgVrG1P-6s";
            string signToken = Convert.ToBase64String(_ecdsa.SignData(Encoding.UTF8.GetBytes(token), HashAlgorithmName.SHA256));
            double version = 1;
            string updateType = "Message";
            string signMessage = Convert.ToBase64String(_ecdsa.SignData(Encoding.UTF8.GetBytes($"{id}{version}{token}{updateType}{signToken}"), HashAlgorithmName.SHA256));
            ETAMPModel model = new ETAMPModel()
            {
                Id = Guid.Parse(id),
                Token = token,
                UpdateType = updateType,
                Version = version,
                SignatureToken = signToken,
                SignatureMessage = signMessage
            };
            var result = await _token.FullVerify(JsonConvert.SerializeObject(model), "someone", "some");

            Assert.False(result);
        }

        [Fact]
        public async Task FullVerify_IncorrectETAMP_ReturnFalse()
        {
            var result = await _token.FullVerify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithIncorrectParametrs_ReturnFalse()
        {
            var result = await _token.FullVerifyWithTokenSignature(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<ECDsa>());
            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithCustomCurveAndPublickKeyIncorrect_ReturnFalse()
        {
            var result = await _token.FullVerifyWithTokenSignature(It.IsAny<string>(), It.IsAny<string>(),
               It.IsAny<string>(), It.IsAny<ECDsa>());
            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithCorrectPublicKey_ReturnTrue()
        {
            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                    .Replace("-----END PUBLIC KEY-----", "")
                                                                    .Replace("\n", "");
            _testableValidateToken = TestableValidateToken();
            var result = await _testableValidateToken.FullVerifyWithTokenSignature("{}", It.IsAny<string>(), It.IsAny<string>(), ECCurve.NamedCurves.nistP256, publicKey);

            Assert.True(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithIncorrectETAMP_ReturnFalse()
        {
            var result = await _token.FullVerifyWithTokenSignature("", It.IsAny<string>(), It.IsAny<string>(), ECCurve.NamedCurves.nistP256, "");

            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithCorrectData_ReturenTrue()
        {
            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            _testableValidateToken = TestableValidateToken();
            var result = await _testableValidateToken.FullVerifyWithTokenSignature("{}", It.IsAny<string>(), It.IsAny<string>(), ecdsa);

            Assert.True(result);
        }

        [Fact]
        public async Task FullVerifyLite_WithIncorrectData_ReturnFalse()
        {
            var result = await _token.FullVerifyLite("", ECDsa.Create());

            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyLite_WithCorrectData_ReturnTrue()
        {
            _testableValidateToken = TestableValidateToken();
            var result = await _testableValidateToken.FullVerifyLite("{}", ECDsa.Create());
            Assert.True(result);
        }

        [Fact]
        public async Task FullVerifyLite_WithCurvePublicKeyIncorrectEtamp_ReturnFalse()
        {
            var result = await _token.FullVerifyLite("", ECCurve.NamedCurves.nistP256, "", new EcdsaWrapper());

            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyLite_WithCorrectCurvePublickey_ReturnTrue()
        {
            _testableValidateToken = TestableValidateToken();

            ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                    .Replace("-----END PUBLIC KEY-----", "")
                                                                    .Replace("\n", "");
            var result = await _testableValidateToken.FullVerifyLite("{}", ECCurve.NamedCurves.nistP521, publicKey, new EcdsaWrapper());

            Assert.True(result);
        }

        [Fact]
        public async Task VerifyLifeTime_WithCorrectToken_ReturnsTrue()
        {
            string etamp = _etamp.CreateETAMP("message", _data);
            string token = JsonConvert.DeserializeObject<ETAMPModel>(etamp).Token;
            _token = new ValidateToken(new VerifyWrapper(_etamp.Ecdsa, _etamp.HashAlgorithm));
            var result = await _token.VerifyLifeTime(token);

            Assert.True(result);
        }

        [Fact]
        public async Task VerifyLifeTime_WithValidToken_ReturnsTrue()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _jwtSecurityTokenHandler.Setup(handler => handler.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                 .ReturnsAsync(new TokenValidationResult { IsValid = true });
            _token = new ValidateToken(_verifyWrapperMock.Object, _jwtSecurityTokenHandler.Object);
            var result = await _token.VerifyLifeTime(token);

            Assert.True(result);
        }

        private TestableValidateToken TestableValidateToken()
        {
            TokenValidationResult validationResult = new()
            {
                IsValid = true
            };
            _jwtSecurityTokenHandler.Setup(x => x.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>())).ReturnsAsync(validationResult);
            return new TestableValidateToken(_verifyWrapperMock.Object, _jwtSecurityTokenHandler.Object);
        }

        [Fact]
        public void VerifyETAMP_WithEmptyGuid_ReturnFalse()
        {
            string etamp = $"{{Id:\"{Guid.Empty}\"}}";

            var result = _token.VerifyETAMP(etamp);
            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignature_WithIncorrectData_ReturnFalse()
        {
            bool result = await _token.FullVerifyWithTokenSignature("{}", It.IsAny<string>(), It.IsAny<string>());
            Assert.False(result);
        }

        [Fact]
        public async Task FullVerifyWithTokenSignatureTest_WithCorrectData_ReturnTrue()
        {
            string etamp = _etamp.CreateETAMP("Message", _data);
            ValidateToken validate = new ValidateToken(new VerifyWrapper(_etamp.Ecdsa, _etamp.HashAlgorithm));
            bool result = await validate.FullVerifyWithTokenSignature(etamp, _data.Audience, _data.Issuer);
            Assert.True(result);
        }
    }
}