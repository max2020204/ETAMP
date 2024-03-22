using ETAMPManagment.ETAMP.Base;
using ETAMPManagment.Utils;
using Moq;
using Xunit;

namespace ETAMPManagment.Factories.Tests
{
    public class ETAMPFactoryTests
    {
        private readonly ETAMPFactory _factory;

        public ETAMPFactoryTests()
        {
            _factory = new ETAMPFactory();
        }

        [Fact]
        public void RegisterGenerator_AddsGeneratorSuccessfully()
        {
            var mockGenerator = Mock.Of<IETAMPData>();
            _factory.RegisterGenerator(ETAMPType.Base, () => mockGenerator);

            Assert.Contains(ETAMPType.Base, _factory.Factory.Keys);
        }

        [Fact]
        public void CreateGenerator_ReturnsCorrectGenerator()
        {
            var mockGenerator = Mock.Of<IETAMPData>();
            _factory.RegisterGenerator(ETAMPType.Base, () => mockGenerator);

            var result = _factory.CreateGenerator(ETAMPType.Base);

            Assert.Equal(mockGenerator, result);
        }

        [Fact]
        public void CreateGenerator_WithUnregisteredType_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _factory.CreateGenerator(ETAMPType.Sign));
        }

        [Fact]
        public void RegisterGenerator_WithExistingType_ThrowsArgumentException()
        {
            var mockGenerator = Mock.Of<IETAMPData>();
            _factory.RegisterGenerator(ETAMPType.Base, () => mockGenerator);

            var duplicateRegistration = () => _factory.RegisterGenerator(ETAMPType.Base, () => mockGenerator);

            var exception = Assert.Throws<ArgumentException>(duplicateRegistration);
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void UnregisterETAMPGenerator_RemovesGeneratorSuccessfully()
        {
            var mockGenerator = Mock.Of<IETAMPData>();
            _factory.RegisterGenerator(ETAMPType.Base, () => mockGenerator);

            var unregisterResult = _factory.UnregisterETAMPGenerator(ETAMPType.Base);

            Assert.True(unregisterResult);
            Assert.DoesNotContain(ETAMPType.Base, _factory.Factory.Keys);
        }
    }
}