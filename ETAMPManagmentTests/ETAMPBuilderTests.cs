using ETAMPManagment.ETAMP.Encrypted.Interfaces;
using ETAMPManagment.Managment;
using ETAMPManagment.Models;
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
        public void CreateEncryptedETAMP_ReturnsBuilderForChaining()
        {
            var encryptedMock = new Mock<IETAMPEncrypted>();
            _serviceProviderMock.Setup(p => p.GetService(typeof(IETAMPEncrypted))).Returns(encryptedMock.Object);

            var result = _etampBuilder.CreateETAMP(ETAMPTypeNames.Encrypted, "update_type", new BasePayload(), 1.0);

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
            Assert.Throws<ArgumentException>(() => _etampBuilder.CreateETAMP("", "update_type", new BasePayload(), 1.0));
        }
    }
}