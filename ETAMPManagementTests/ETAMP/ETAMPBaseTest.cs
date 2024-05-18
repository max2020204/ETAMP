using ETAMPManagement.ETAMP;
using ETAMPManagement.Helper;
using ETAMPManagement.Models;
using Xunit;

namespace ETAMPManagementTests.ETAMP;

public class ETAMPBaseTest
{
    [Fact]
    public void CreateETAMPModel_ShouldReturnExpectedModel()
    {
        // Arrange
        var versionInfo = new VersionInfo();
        versionInfo.GetVersionInfo();
        var sut = new ETAMPBase(versionInfo);

        var inputUpdateType = "update type test";
        var inputPayload = new Token();
        var inputCompressionType = "compression type test";

        // Act
        var result = sut.CreateETAMPModel(inputUpdateType, inputPayload, inputCompressionType);

        // Assert
        Assert.Equal(inputUpdateType, result.UpdateType);
        Assert.Equal(versionInfo.ProtocolVersion, result.Version);
        Assert.Same(inputPayload, result.Token);
    }
}