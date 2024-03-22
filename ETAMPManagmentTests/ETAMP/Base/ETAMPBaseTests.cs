using ETAMPManagment.Models;
using ETAMPManagment.Services.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ETAMPManagment.ETAMP.Base.Tests
{
    public class ETAMPBaseTests
    {
        private readonly ETAMPBase _base;
        private readonly BasePayload _basePayload;

        public ETAMPBaseTests()
        {
            _base = new ETAMPBase(new Mock<ISigningCredentialsProvider>().Object);
            _basePayload = new BasePayload();
        }

        [Fact]
        public void CreateETAMP_ReturnsValidJsonString()
        {
            var result = _base.CreateETAMP("updateType", _basePayload);
            var deserialized = JsonConvert.DeserializeObject(result);

            Assert.NotNull(deserialized);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void CreateETAMPModel_ReturnsCorrectModelInstance()
        {
            var result = _base.CreateETAMPModel("updateType", _basePayload);
            Assert.NotNull(result);
            Assert.IsType<ETAMPModel>(result);
            Assert.Equal("updateType", result.UpdateType);
            Assert.NotNull(result.Token);
            Assert.True(result.Version > 0);
        }
    }
}