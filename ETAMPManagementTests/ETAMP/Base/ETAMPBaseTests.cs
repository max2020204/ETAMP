#region

using ETAMPManagement.ETAMP.Base;
using ETAMPManagement.Models;
using ETAMPManagement.Services.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagementTests.ETAMP.Base;

public class ETAMPBaseTests
{
    private const string UpdateType = "updateType";
    private readonly BasePayload _basePayload;
    private readonly ETAMPBase _etampBase;

    public ETAMPBaseTests()
    {
        var signatureMock = new Mock<ISigningCredentialsProvider>();
        _etampBase = new ETAMPBase(signatureMock.Object, new ETAMPData(signatureMock.Object));
        _basePayload = new BasePayload();
    }

    [Fact]
    public void CreateETAMPModel_ReturnsCorrectModelInstance()
    {
        var result = _etampBase.CreateETAMPModel(UpdateType, _basePayload);

        Assert.NotNull(result);
        Assert.IsType<ETAMPModel>(result);
        Assert.Equal(UpdateType, result.UpdateType);
        Assert.NotNull(result.Token);
        Assert.True(result.Version > 0);
    }
}