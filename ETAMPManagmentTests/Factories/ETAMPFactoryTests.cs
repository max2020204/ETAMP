using ETAMPManagment.ETAMP.Base.Interfaces;
using ETAMPManagment.Utils;
using Moq;
using Xunit;

namespace ETAMPManagment.Factories.Tests
{
    public class ETAMPFactoryTests
    {
        private readonly ETAMPFactory _factory;
        private readonly Mock<IETAMPData> _mockGenerator;

        public ETAMPFactoryTests()
        {
            _factory = new ETAMPFactory();
            _mockGenerator = new Mock<IETAMPData>();
        }

        private void RegisterBaseGenerator()
        {
            _factory.RegisterGenerator(ETAMPType.Base, () => _mockGenerator.Object);
        }

        [Fact]
        public void RegisterGenerator_AddsGeneratorSuccessfully()
        {
            RegisterBaseGenerator();
            Assert.Contains(ETAMPType.Base, _factory.Factory.Keys);
        }

        [Fact]
        public void CreateGenerator_ReturnsCorrectGenerator()
        {
            RegisterBaseGenerator();
            var result = _factory.CreateGenerator(ETAMPType.Base);
            Assert.Equal(_mockGenerator.Object, result);
        }

        [Fact]
        public void CreateGenerator_WithUnregisteredType_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => _factory.CreateGenerator(ETAMPType.Sign));
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void RegisterGenerator_WithExistingType_ThrowsArgumentException()
        {
            RegisterBaseGenerator();
            var exception = Assert.Throws<ArgumentException>(() => _factory.RegisterGenerator(ETAMPType.Base, () => _mockGenerator.Object));
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void UnregisterETAMPGenerator_RemovesGeneratorSuccessfully()
        {
            RegisterBaseGenerator();
            var unregisterResult = _factory.UnregisterETAMPGenerator(ETAMPType.Base);
            Assert.True(unregisterResult);
            Assert.DoesNotContain(ETAMPType.Base, _factory.Factory.Keys);
        }
    }
}