using ETAMP.Compares;
using ETAMP.Models;
using Xunit;

namespace ETAMPTests.Compares
{
    public class EtampCompareTests
    {
        private readonly ETAMPCompare _etampCompare;

        public EtampCompareTests()
        {
            _etampCompare = new ETAMPCompare();
        }

        [Fact]
        public void EqualsTest_WithSameModel_True()
        {
            ETAMPModel model = new()
            {
                Id = Guid.Empty,
                Version = 1,
                Token = "SomeToken",
                SignatureToken = "SomeSignatureToken",
                SignatureMessage = "SomeSignatureMessage"
            };
            ETAMPModel model1 = new()
            {
                Id = Guid.Empty,
                Version = 1,
                Token = "SomeToken",
                SignatureToken = "SomeSignatureToken",
                SignatureMessage = "SomeSignatureMessage"
            };
            var result = _etampCompare.Equals(model, model1);
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_WithDiffrentToken_True()
        {
            ETAMPModel model = new()
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Token = "SomeToken1",
                SignatureToken = "SomeSignatureToken",
                SignatureMessage = "SomeSignatureMessage"
            };
            ETAMPModel model1 = new()
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Token = "SomeToken",
                SignatureToken = "SomeSignatureToken",
                SignatureMessage = "SomeSignatureMessage"
            };
            var result = _etampCompare.Equals(model, model1);
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_BothNew_True()
        {
            ETAMPModel model = new();
            ETAMPModel model1 = new();
            var result = _etampCompare.Equals(model, model1);
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_BothNull_True()
        {
            var result = _etampCompare.Equals(null, null);
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_SecondNotNull_True()
        {
            var result = _etampCompare.Equals(null, new ETAMPModel());
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_FirstNotNull_True()
        {
            var result = _etampCompare.Equals(new ETAMPModel(), null);
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_IdenticalObjects_SameHashCode()
        {
            var model1 = new ETAMPModel
            {
                Id = Guid.NewGuid(),
                Version = 1.0,
                Token = "Token1",
                UpdateType = "Type1",
                SignatureToken = "SigToken1",
                SignatureMessage = "SigMessage1"
            };
            var model2 = new ETAMPModel
            {
                Id = model1.Id,
                Version = model1.Version,
                Token = model1.Token,
                UpdateType = model1.UpdateType,
                SignatureToken = model1.SignatureToken,
                SignatureMessage = model1.SignatureMessage
            };

            int hash1 = _etampCompare.GetHashCode(model1);
            int hash2 = _etampCompare.GetHashCode(model2);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_DifferentObjects_DifferentHashCode()
        {
            var model1 = new ETAMPModel { Id = Guid.NewGuid(), Version = 1.0, Token = "Token1", UpdateType = "Type1", SignatureToken = "SigToken1", SignatureMessage = "SigMessage1" };
            var model2 = new ETAMPModel { Id = Guid.NewGuid(), Version = 2.0, Token = "Token2", UpdateType = "Type2", SignatureToken = "SigToken2", SignatureMessage = "SigMessage2" };

            int hash1 = _etampCompare.GetHashCode(model1);
            int hash2 = _etampCompare.GetHashCode(model2);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_NullProperties_HandledGracefully()
        {
            var compare = new ETAMPCompare();
            var model = new ETAMPModel { Id = Guid.NewGuid(), Version = 0, Token = null, UpdateType = null, SignatureToken = null, SignatureMessage = null };

            Exception ex = Record.Exception(() => compare.GetHashCode(model));
            Assert.Null(ex);
        }
    }
}