using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Encrypted;
using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Utils;
using Moq;
using Xunit;

namespace ETAMPManagment.Tests
{
    public class ETAMPBuilderTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly ETAMPBuilder _etampBuilder;

        public ETAMPBuilderTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _etampBuilder = new ETAMPBuilder(_serviceProviderMock.Object);
        }

        [Fact]
        public void CreateSignETAMP_ReturnsBuilderForChaining()
        {
            var signMock = new Mock<ETAMPSign>();
            _serviceProviderMock.Setup(p => p.GetService(typeof(ETAMPSign))).Returns(signMock.Object);

            var result = _etampBuilder.CreateETAMP(ETAMPType.Sign, "update_type", new BasePayload(), 1.0);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void CreateEncryptedETAMP_ReturnsBuilderForChaining()
        {
            var encryptedMock = new Mock<IETAMPEncrypted>();
            _serviceProviderMock.Setup(p => p.GetService(typeof(IETAMPEncrypted))).Returns(encryptedMock.Object);

            var result = _etampBuilder.CreateETAMP(ETAMPType.Encrypted, "update_type", new BasePayload(), 1.0);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void CreateEncryptedSignETAMP_ReturnsBuilderForChaining()
        {
            var encryptedSignedMock = new Mock<ETAMPEncryptedSigned>();
            _serviceProviderMock.Setup(p => p.GetService(typeof(ETAMPEncryptedSigned))).Returns(encryptedSignedMock.Object);

            var result = _etampBuilder.CreateETAMP(ETAMPType.EncryptedSign, "update_type", new BasePayload(), 1.0);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void Build_ThrowsException_WhenETAMPModelIsNotCreated()
        {
            Assert.Throws<InvalidOperationException>(() => _etampBuilder.Build());
        }

        [Fact]
        public void CreateETAMP_ThrowsException_WhenETAMPTypeIsUnsupported()
        {
            Assert.Throws<ArgumentException>(() => _etampBuilder.CreateETAMP((ETAMPType)99, "update_type", new BasePayload(), 1.0));
        }
    }
}