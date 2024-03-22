using ETAMPManagment.Encryption.Interfaces;
using ETAMPManagment.Models;
using ETAMPManagment.Validators.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ETAMPManagment.ETAMP.Encrypted.Tests
{
    public class EncryptTokenTests
    {
        [Fact]
        public void EncryptETAMP_WithValidToken_ReturnsEncryptedModel()
        {
            // Arrange
            var mockStructureValidator = new Mock<IStructureValidator>();
            var mockEciesEncryptionService = new Mock<IEciesEncryptionService>();
            string jsonEtamp = "{\"token\":\"example\"}";
            ETAMPModel expectedModel = new ETAMPModel { Token = "encryptedToken" };

            mockStructureValidator.Setup(m => m.IsValidEtampFormat(jsonEtamp))
                                  .Returns(new ETAMPModel { Token = "example" });
            mockEciesEncryptionService.Setup(m => m.Encrypt(It.IsAny<string>()))
                                      .Returns("encryptedToken");

            var encryptToken = new EncryptToken(mockStructureValidator.Object, mockEciesEncryptionService.Object);

            // Act
            var result = encryptToken.EncryptETAMP(jsonEtamp);

            // Assert
            Assert.Equal(expectedModel.Token, result.Token);
        }

        //[Fact]
        //public void EncryptETAMP_WithInvalidToken_ThrowsInvalidOperationException()
        //{
        //    // Arrange
        //    var mockStructureValidator = new Mock<IStructureValidator>();
        //    var mockEciesEncryptionService = new Mock<IEciesEncryptionService>();
        //    string jsonEtamp = "{\"invalid\":\"data\"}";

        //    mockStructureValidator.Setup(m => m.IsValidEtampFormat(jsonEtamp))
        //                          .Returns(null);

        //    var encryptToken = new EncryptToken(mockStructureValidator.Object, mockEciesEncryptionService.Object);

        //    // Act & Assert
        //    Assert.Throws<InvalidOperationException>(() => encryptToken.EncryptETAMP(jsonEtamp));
        //}

        [Fact]
        public void EncryptETAMPToken_WithValidToken_ReturnsEncryptedString()
        {
            var mockStructureValidator = new Mock<IStructureValidator>();
            var mockEciesEncryptionService = new Mock<IEciesEncryptionService>();
            string jsonEtamp = "{\"token\":\"example\"}";
            string expectedEncryptedToken = "{\"Token\":\"encryptedToken\"}";

            mockStructureValidator.Setup(m => m.IsValidEtampFormat(jsonEtamp))
                                  .Returns(new ETAMPModel { Token = "example" });
            mockEciesEncryptionService.Setup(m => m.Encrypt(It.IsAny<string>()))
                                      .Returns("encryptedToken");

            var encryptToken = new EncryptToken(mockStructureValidator.Object, mockEciesEncryptionService.Object);

            // Act
            var result = JsonConvert.DeserializeObject<ETAMPModel>(encryptToken.EncryptETAMPToken(jsonEtamp));

            // Assert
            Assert.Equal("encryptedToken", result.Token);
        }
    }
}