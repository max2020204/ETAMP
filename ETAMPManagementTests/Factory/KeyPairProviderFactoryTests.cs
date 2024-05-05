#region

using ETAMPManagement.Encryption;
using ETAMPManagement.Factory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.Factory;

public class KeyPairProviderFactoryTests
{
    [Fact]
    public void Constructor_ThrowsArgumentNullException_IfServiceProviderIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new KeyPairProviderFactory(null));
        Assert.Equal("serviceProvider", exception.ParamName); // Verify that the correct parameter name is reported
    }

    [Fact]
    public void CreateInstance_ReturnsKeyPairProviderInstance()
    {
        // Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var keyPairProvider = new KeyPairProvider(); // Assuming you have a default constructor or mock

        // Setup dependency injection to return a KeyPairProvider instance
        serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);
        serviceScopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);
        serviceScopeMock.Setup(x => x.ServiceProvider)
            .Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(KeyPairProvider)))
            .Returns(keyPairProvider);

        var factory = new KeyPairProviderFactory(serviceProviderMock.Object);

        // Act
        var result = factory.CreateInstance();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<KeyPairProvider>(result);
    }
}