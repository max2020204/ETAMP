using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Moq;
using Xunit;

namespace ETAMPManagment.ETAMP.Base.Tests
{
    public class ETAMPBaseTests
    {
        private readonly ETAMPBase _etampBase;
        private readonly BasePayload _basePayload;
        private const string UpdateType = "updateType";

        public ETAMPBaseTests()
        {
            _etampBase = new ETAMPBase(new Mock<ISigningCredentialsProvider>().Object);
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
}