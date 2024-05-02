#region

using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagment.ETAMP.Base.Tests;

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