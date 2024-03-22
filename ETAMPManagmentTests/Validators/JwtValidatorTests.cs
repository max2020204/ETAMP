using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Validators.Tests
{
    public class JwtValidatorTests
    {
        private readonly JwtValidator _jwtValidator;

        public JwtValidatorTests()
        {
            _jwtValidator = new JwtValidator(new JwtSecurityTokenHandler());
        }

        [Fact]
        public void IsValidJwtToken_WithValidToken_ReturnsTrue()
        {
            var token = "eyJ0eXAiOiJFVEFNUCJ9..";

            var result = _jwtValidator.IsValidJwtToken(token);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateLifeTime_WithValidToken_ReturnsTrue()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.zv3tcTciqfMfOcIkGfnKLm7siFr9UHiuuOocxmEuPiQ";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            var tokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
            tokenHandlerMock.Setup(th => th.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
                            .ReturnsAsync(new TokenValidationResult { IsValid = true });

            var jwtValidator = new JwtValidator(tokenHandlerMock.Object);

            var result = await jwtValidator.ValidateLifeTime(token, securityKey);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateToken_WithValidClaims_ReturnsTrue()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.zv3tcTciqfMfOcIkGfnKLm7siFr9UHiuuOocxmEuPiQ";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            var tokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
            tokenHandlerMock.Setup(th => th.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
                            .ReturnsAsync(new TokenValidationResult { IsValid = true });

            var jwtValidator = new JwtValidator(tokenHandlerMock.Object);
            var result = await jwtValidator.ValidateToken(token, "", "", securityKey);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsValidJwtToken_WithNullOrEmptyToken_ThrowsArgumentException()
        {
            var jwtValidator = new JwtValidator();

            var result = jwtValidator.IsValidJwtToken(null);

            Assert.False(result.IsValid);
            Assert.Equal("The token cannot be null or empty", result.ErrorMessage);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidStructure_ThrowsFormatException()
        {
            var jwtValidator = new JwtValidator();
            var invalidToken = "invalid.jwt";

            var result = jwtValidator.IsValidJwtToken(invalidToken);
            Assert.False(result.IsValid);
            Assert.Contains("JWT must consist of three parts: header, payload, and signature", result.ErrorMessage);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidHeader_ThrowsInvalidOperationException()
        {
            var jwtValidator = new JwtValidator();

            var invalidHeaderToken = "eyJ0eXAiOiJ3cm9uZyJ9..";

            var result = jwtValidator.IsValidJwtToken(invalidHeaderToken);
            Assert.Contains("The JWT header is invalid or the expected type 'ETAMP' is missing", result.ErrorMessage);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidJsonHeader_ThrowsInvalidOperationExceptionForJsonException()
        {
            var jwtValidator = new JwtValidator();

            var invalidJsonToken = $"eyJhbGciOiJIUzI1NiIsInR5CI6IkVUQU1QIn0.payload.signature";

            var exception = jwtValidator.IsValidJwtToken(invalidJsonToken);
            Assert.Contains("Error deserializing the JWT header", exception.ErrorMessage);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidBase64Url_ThrowsInvalidOperationExceptionForFormatException()
        {
            var jwtValidator = new JwtValidator();
            var invalidBase64UrlToken = "notBase64.header.payload";

            var result = jwtValidator.IsValidJwtToken(invalidBase64UrlToken);
            Assert.Contains("Format error decoding from Base64Url", result.ErrorMessage);
        }

        [Fact()]
        public async Task ValidateLifeTime_InvalidInputData_ReturnFalse()
        {
            var result = await _jwtValidator.ValidateLifeTime("", null);
            Assert.False(result.IsValid);
        }

        [Fact()]
        public async Task ValidateToken_InvalidInputData_ReturnFalse()
        {
            var result = await _jwtValidator.ValidateToken("", "", "", null);
            Assert.False(result.IsValid);
        }
    }
}