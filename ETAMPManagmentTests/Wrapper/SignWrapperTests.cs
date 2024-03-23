using ETAMPManagment.Models;
using ETAMPManagment.Wrapper.Interfaces;
using Moq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Wrapper.Tests
{
    public class SignWrapperTests
    {
        private readonly ECDsa _ecdsa;
        private readonly SignWrapper _signWrapper;

        public SignWrapperTests()
        {
            _ecdsa = ECDsa.Create();
            _signWrapper = new SignWrapper(_ecdsa, HashAlgorithmName.SHA256);
        }

        [Fact]
        public void SignEtamp_WithValidJson_ReturnsUpdatedEtampModel()
        {
            var etampModel = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Token = "someToken",
            };
            var jsonEtamp = JsonConvert.SerializeObject(etampModel);

            var signedJson = _signWrapper.SignEtamp(jsonEtamp);
            var signedEtampModel = JsonConvert.DeserializeObject<ETAMPModel>(signedJson);

            Assert.NotNull(signedEtampModel);
            Assert.NotNull(signedEtampModel.SignatureToken);
            Assert.NotNull(signedEtampModel.SignatureMessage);

            Assert.NotEmpty(signedEtampModel.SignatureToken);
            Assert.NotEmpty(signedEtampModel.SignatureMessage);
        }

        [Fact]
        public void SignEtamp_WithNullInput_ThrowsArgumentNullException()
        {
            var ecdsa = ECDsa.Create();
            var signWrapper = new SignWrapper(ecdsa, HashAlgorithmName.SHA256);
            var exception = Assert.Throws<ArgumentNullException>(() => signWrapper.SignEtamp(""));
            Assert.Equal("etamp", exception.ParamName);
        }

        [Fact]
        public void SignEtampModel_UpdatesSignatureFieldsCorrectly()
        {
            var ecdsa = ECDsa.Create();
            var signWrapper = new SignWrapper(ecdsa, HashAlgorithmName.SHA256);
            var etampModel = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Version = 1.0,
                Token = "testToken",
                UpdateType = "updateType"
            };

            var signedJson = signWrapper.SignEtamp(etampModel);
            var updatedEtampModel = JsonConvert.DeserializeObject<ETAMPModel>(signedJson);

            Assert.NotNull(updatedEtampModel);
            Assert.NotNull(updatedEtampModel.SignatureToken);
            Assert.NotNull(updatedEtampModel.SignatureMessage);
        }

        [Fact]
        public void Constructor_WithStringPrivateKey_InitializesCorrectly()
        {
            string key = _ecdsa.ExportPkcs8PrivateKeyPem().Replace("-----BEGIN PRIVATE KEY-----", "")
                             .Replace("-----END PRIVATE KEY-----", "")
                             .Replace("\n", "")
                             .Replace("\r", "");
            var signWrapper = new SignWrapper(key, HashAlgorithmName.SHA256);

            Assert.NotNull(signWrapper);
        }

        [Fact]
        public void Constructor_WithEcdsaWrapperAndStringPrivateKey_InitializesCorrectly()
        {
            var mockEcdsaWrapper = new Mock<IEcdsaWrapper>();
            var curve = ECCurve.NamedCurves.nistP256;
            ECDsa ecdsa = ECDsa.Create();
            var algorithmName = HashAlgorithmName.SHA256;

            string key = ecdsa.ExportPkcs8PrivateKeyPem().Replace("-----BEGIN PRIVATE KEY-----", "")
                             .Replace("-----END PRIVATE KEY-----", "")
                             .Replace("\n", "")
                             .Replace("\r", "");

            mockEcdsaWrapper.Setup(m => m.ImportECDsa(It.IsAny<string>(), It.IsAny<ECCurve>())).Returns(ECDsa.Create(curve));

            var signWrapper = new SignWrapper(mockEcdsaWrapper.Object, key, curve, algorithmName);

            Assert.NotNull(signWrapper);
            mockEcdsaWrapper.Verify(m => m.ImportECDsa(key, curve), Times.Once());
        }

        [Fact]
        public void Constructor_WithEcdsaWrapperAndByteArrayPrivateKey_InitializesCorrectly()
        {
            var mockEcdsaWrapper = new Mock<IEcdsaWrapper>();
            var curve = ECCurve.NamedCurves.nistP256;
            ECDsa ecdsa = ECDsa.Create();
            var algorithmName = HashAlgorithmName.SHA256;

            byte[] key = Convert.FromBase64String(ecdsa.ExportPkcs8PrivateKeyPem().Replace("-----BEGIN PRIVATE KEY-----", "")
                             .Replace("-----END PRIVATE KEY-----", "")
                             .Replace("\n", "")
                             .Replace("\r", ""));

            mockEcdsaWrapper.Setup(m => m.ImportECDsa(It.IsAny<byte[]>(), It.IsAny<ECCurve>())).Returns(ECDsa.Create(curve));

            var signWrapper = new SignWrapper(mockEcdsaWrapper.Object, key, curve, algorithmName);

            Assert.NotNull(signWrapper);
            mockEcdsaWrapper.Verify(m => m.ImportECDsa(key, curve), Times.Once());
        }

        [Fact]
        public void SignEtampModel_UpdatesSignatureFields()
        {
            var etamp = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Version = 1.0,
                Token = "token",
                UpdateType = "update",
                SignatureMessage = "",
                SignatureToken = ""
            };

            var signedEtamp = _signWrapper.SignEtampModel(etamp);

            Assert.NotNull(signedEtamp);
            Assert.NotNull(signedEtamp.SignatureToken);
            Assert.NotNull(signedEtamp.SignatureMessage);
            Assert.NotEmpty(signedEtamp.SignatureToken);
            Assert.NotEmpty(signedEtamp.SignatureMessage);
        }
    }
}