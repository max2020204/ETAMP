using ETAMPManagment.Encryption.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Factories.Tests
{
    public class EncryptionServiceFactoryTests
    {
        private readonly EncryptionServiceFactory _factory;

        public EncryptionServiceFactoryTests()
        {
            _factory = new EncryptionServiceFactory();
        }

        [Fact]
        public void RegisterEncryptionService_StoresServiceCreator()
        {
            Func<IEncryptionService> serviceCreator = () => Mock.Of<IEncryptionService>();

            // Act
            _factory.RegisterEncryptionService("TestService", serviceCreator);

            // Assert
            Assert.Contains("TestService", _factory.Services.Keys);
        }

        [Fact]
        public void CreateEncryptionService_ReturnsServiceInstance()
        {
            var mockService = new Mock<IEncryptionService>();
            Func<IEncryptionService> serviceCreator = () => mockService.Object;
            _factory.RegisterEncryptionService("TestService", serviceCreator);

            var service = _factory.CreateEncryptionService("TestService");

            Assert.NotNull(service);
            Assert.IsAssignableFrom<IEncryptionService>(service);
        }

        [Fact]
        public void CreateEncryptionService_WithUnsupportedName_ThrowsArgumentException()
        {
            var factory = new EncryptionServiceFactory();

            Assert.Throws<ArgumentException>(() => factory.CreateEncryptionService("UnsupportedService"));
        }

        [Fact]
        public void CreateEncryptionService_AfterRegisteringMultipleServices_ReturnsCorrectService()
        {
            var mockService1 = new Mock<IEncryptionService>();
            var mockService2 = new Mock<IEncryptionService>();
            _factory.RegisterEncryptionService("Service1", () => mockService1.Object);
            _factory.RegisterEncryptionService("Service2", () => mockService2.Object);

            var service1 = _factory.CreateEncryptionService("Service1");
            var service2 = _factory.CreateEncryptionService("Service2");

            Assert.Equal(mockService1.Object, service1);
            Assert.Equal(mockService2.Object, service2);
        }

        [Fact]
        public void RegisterEncryptionService_WithDuplicateName_ThrowsArgumentException()
        {
            Func<IEncryptionService> serviceCreator = Mock.Of<IEncryptionService>;
            _factory.RegisterEncryptionService("TestService", serviceCreator);
            var exception = Assert.Throws<ArgumentException>(() => _factory.RegisterEncryptionService("TestService", serviceCreator));
            Assert.Equal("name", exception.ParamName);
            Assert.Contains("TestService", exception.Message);
        }

        [Fact]
        public void UnregisterEncryptionService_RemovesService()
        {
            var mockService = new Mock<IEncryptionService>();
            Func<IEncryptionService> serviceCreator = () => mockService.Object;
            _factory.RegisterEncryptionService("TestService", serviceCreator);

            var result = _factory.UnregisterEncryptionService("TestService");

            Assert.True(result);
            Assert.DoesNotContain("TestService", _factory.Services.Keys);
        }
    }
}