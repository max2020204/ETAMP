using ETAMPManagment.Encryption.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.Factories.Tests
{
    public class EncryptionServiceFactoryTests
    {
        private readonly EncryptionServiceFactory _factory;
        private const string TestServiceName = "TestService";

        public EncryptionServiceFactoryTests()
        {
            _factory = new EncryptionServiceFactory();
        }

        [Fact]
        public void RegisterEncryptionService_ShouldStoreServiceCreator()
        {
            Func<IEncryptionService> serviceCreator = () => Mock.Of<IEncryptionService>();

            _factory.RegisterEncryptionService(TestServiceName, serviceCreator);

            Assert.Contains(TestServiceName, _factory.Services.Keys);
        }

        [Fact]
        public void CreateEncryptionService_ShouldReturnRegisteredServiceInstance()
        {
            var mockService = new Mock<IEncryptionService>();
            Func<IEncryptionService> serviceCreator = () => mockService.Object;
            _factory.RegisterEncryptionService(TestServiceName, serviceCreator);

            var service = _factory.CreateEncryptionService(TestServiceName);

            Assert.NotNull(service);
            Assert.IsAssignableFrom<IEncryptionService>(service);
        }

        [Fact]
        public void CreateEncryptionService_WithUnsupportedName_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _factory.CreateEncryptionService("UnsupportedService"));
        }

        [Fact]
        public void CreateEncryptionService_AfterRegisteringMultipleServices_ShouldReturnCorrectService()
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
        public void RegisterEncryptionService_WithDuplicateName_ShouldThrowArgumentException()
        {
            Func<IEncryptionService> serviceCreator = () => Mock.Of<IEncryptionService>();
            _factory.RegisterEncryptionService(TestServiceName, serviceCreator);

            var exception = Assert.Throws<ArgumentException>(() => _factory.RegisterEncryptionService(TestServiceName, serviceCreator));

            Assert.Equal("name", exception.ParamName);
            Assert.Contains(TestServiceName, exception.Message);
        }

        [Fact]
        public void UnregisterEncryptionService_ShouldRemoveService()
        {
            var mockService = new Mock<IEncryptionService>();
            Func<IEncryptionService> serviceCreator = () => mockService.Object;
            _factory.RegisterEncryptionService(TestServiceName, serviceCreator);

            var result = _factory.UnregisterEncryptionService(TestServiceName);

            Assert.True(result);
            Assert.DoesNotContain(TestServiceName, _factory.Services.Keys);
        }
    }
}