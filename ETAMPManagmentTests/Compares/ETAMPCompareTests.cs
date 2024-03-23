using ETAMPManagment.Models;
using Xunit;

namespace ETAMPManagment.Compares.Tests
{
    public class EtampCompareTests
    {
        private readonly ETAMPCompare _etampCompare;

        public EtampCompareTests()
        {
            _etampCompare = new ETAMPCompare();
        }

        [Fact]
        public void Equals_WithSameModel_ReturnsTrue()
        {
            var model = CreateModel(Guid.Empty, 1, "SomeToken", "SomeSignatureToken", "SomeSignatureMessage");
            var model1 = CreateModel(Guid.Empty, 1, "SomeToken", "SomeSignatureToken", "SomeSignatureMessage");

            var result = _etampCompare.Equals(model, model1);

            Assert.True(result);
        }

        [Fact]
        public void Equals_WithDifferentToken_ReturnsFalse()
        {
            var model = CreateModel(Guid.NewGuid(), 1, "SomeToken1", "SomeSignatureToken", "SomeSignatureMessage");
            var model1 = CreateModel(Guid.NewGuid(), 1, "SomeToken", "SomeSignatureToken", "SomeSignatureMessage");

            var result = _etampCompare.Equals(model, model1);

            Assert.False(result);
        }

        [Fact]
        public void Equals_BothNew_ReturnsTrue()
        {
            var model = new ETAMPModel();
            var model1 = new ETAMPModel();

            var result = _etampCompare.Equals(model, model1);

            Assert.True(result);
        }

        [Fact]
        public void Equals_BothNull_ReturnsTrue()
        {
            var result = _etampCompare.Equals(null, null);

            Assert.True(result);
        }

        [Fact]
        public void Equals_SecondNotNull_ReturnsFalse()
        {
            var result = _etampCompare.Equals(null, new ETAMPModel());

            Assert.False(result);
        }

        [Fact]
        public void Equals_FirstNotNull_ReturnsFalse()
        {
            var result = _etampCompare.Equals(new ETAMPModel(), null);

            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_IdenticalObjects_ReturnsSameHashCode()
        {
            var id = Guid.NewGuid();
            var model1 = CreateModel(id, 1.0, "Token1", "SigToken1", "SigMessage1");
            var model2 = CreateModel(id, 1.0, "Token1", "SigToken1", "SigMessage1");

            var hash1 = _etampCompare.GetHashCode(model1);
            var hash2 = _etampCompare.GetHashCode(model2);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_DifferentObjects_ReturnsDifferentHashCode()
        {
            var model1 = CreateModel(Guid.NewGuid(), 1.0, "Token1", "SigToken1", "SigMessage1");
            var model2 = CreateModel(Guid.NewGuid(), 2.0, "Token2", "SigToken2", "SigMessage2");

            var hash1 = _etampCompare.GetHashCode(model1);
            var hash2 = _etampCompare.GetHashCode(model2);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_NullProperties_HandledGracefully()
        {
            var model = new ETAMPModel { Id = Guid.NewGuid() };

            Exception exception = Record.Exception(() => _etampCompare.GetHashCode(model));

            Assert.Null(exception);
        }

        private ETAMPModel CreateModel(Guid id, double version, string token, string signatureToken, string signatureMessage)
        {
            return new ETAMPModel
            {
                Id = id,
                Version = version,
                Token = token,
                SignatureToken = signatureToken,
                SignatureMessage = signatureMessage
            };
        }
    }
}