using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.ETAMP.Encrypted;
using ETAMPManagment.Factories.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Services;
using ETAMPManagment.Services.Interfaces;
using ETAMPManagment.Utils;
using ETAMPManagment.Wrapper.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Tests
{
    public class ETAMPBuilderTests
    {
        private readonly Mock<IETAMPFactory<ETAMPType>> _factoryMock;
        private readonly Mock<ISigningCredentialsProvider> _signingCredentialsProviderMock;
        private readonly Mock<IEciesEncryptionService> _eiesEncryptionServiceMock;
        private readonly Mock<ISignWrapper> _signWrapperMock;
        private readonly ETAMPBuilder _etampBuilder;

        private const string UpdateType = "update";
        private const double Version = 1.0;

        public ETAMPBuilderTests()
        {
            _signWrapperMock = new Mock<ISignWrapper>();
            _eiesEncryptionServiceMock = new Mock<IEciesEncryptionService>();
            _signingCredentialsProviderMock = new Mock<ISigningCredentialsProvider>();
            _factoryMock = new Mock<IETAMPFactory<ETAMPType>>();
            _etampBuilder = new ETAMPBuilder(_factoryMock.Object);
        }

        [Fact]
        public void CreateETAMP_ReturnsBuilderForChaining()
        {
            SetupFactoryMock(ETAMPType.Base);

            var result = _etampBuilder.CreateETAMP(UpdateType, new BasePayload(), Version);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void CreateSignETAMP_ReturnsBuilderForChaining()
        {
            SetupSignMocks();
            var sign = new ETAMPSign(_signWrapperMock.Object, _signingCredentialsProviderMock.Object);
            _factoryMock.Setup(f => f.CreateGenerator(ETAMPType.Sign)).Returns(sign);

            var result = _etampBuilder.CreateSignETAMP(UpdateType, new BasePayload(), Version);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void CreateEncryptedETAMP_ReturnsBuilderForChaining()
        {
            SetupFactoryMock(ETAMPType.Encrypted);

            var result = _etampBuilder.CreateEncryptedETAMP(UpdateType, new BasePayload(), Version);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void CreateEncryptedSignETAMP_ReturnsBuilderForChaining()
        {
            SetupSignMocks();
            var encryptedSigned = new ETAMPEncryptedSigned(_signWrapperMock.Object, _eiesEncryptionServiceMock.Object, _signingCredentialsProviderMock.Object);
            _factoryMock.Setup(f => f.CreateGenerator(ETAMPType.EncryptedSign)).Returns(encryptedSigned);

            var result = _etampBuilder.CreateEncryptedSignETAMP(UpdateType, new BasePayload(), Version);

            Assert.IsType<ETAMPBuilder>(result);
        }

        [Fact]
        public void Build_ReturnsETAMPModel()
        {
            SetupSignMocks();
            var etampBase = new ETAMPBase(_signingCredentialsProviderMock.Object);
            _factoryMock.Setup(f => f.CreateGenerator(It.IsAny<ETAMPType>())).Returns(etampBase);

            _etampBuilder.CreateETAMP(UpdateType, new BasePayload());

            var result = _etampBuilder.Build();

            Assert.IsType<ETAMPModel>(result);
        }

        private void SetupFactoryMock(ETAMPType type)
        {
            _factoryMock.Setup(f => f.CreateGenerator(type)).Returns(new Mock<IETAMPData>().Object);
        }

        private void SetupSignMocks()
        {
            _signWrapperMock.Setup(x => x.SignEtampModel(It.IsAny<ETAMPModel>())).Returns((ETAMPModel model) => model);
            _signingCredentialsProviderMock.Setup(x => x.CreateSigningCredentials()).Returns(new ECDsaSigningCredentialsProvider().CreateSigningCredentials());
        }
    }
}