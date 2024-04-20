using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Validators.Tests
{
    public class JwtValidatorTests
    {
        private JwtValidator _jwtValidator;

        public JwtValidatorTests()
        {
            _jwtValidator = new JwtValidator();
        }

        [Fact]
        public void IsValidJwtToken_WithValidToken_ReturnsTrue()
        {
            var token = "eyJ0eXAiOiJFVEFNUCJ9..";

            var result = _jwtValidator.IsValidJwtToken(token);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsValidJwtToken_WithNullOrEmptyToken_ThrowsArgumentException()
        {
            var result = _jwtValidator.IsValidJwtToken("");

            Assert.False(result.IsValid);
            Assert.Equal("The token cannot be null or empty", result.ErrorMessage);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidStructure_ThrowsFormatException()
        {
            var invalidToken = "invalid.jwt";

            var result = _jwtValidator.IsValidJwtToken(invalidToken);
            Assert.False(result.IsValid);
            Assert.Contains("JWT must consist of three parts: header, payload, and signature", result.ErrorMessage);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidHeader_ThrowsInvalidOperationException()
        {
            var invalidHeaderToken = "eyJ0eXAiOiJ3cm9uZyJ9..";

            var result = _jwtValidator.IsValidJwtToken(invalidHeaderToken);
            Assert.Contains("The JWT header is invalid or the expected type 'ETAMP' is missing", result.ErrorMessage);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidJsonHeader_ThrowsInvalidOperationExceptionForJsonException()
        {
            var invalidJsonToken = $"eyJhbGciOiJIUzI1NiIsInR5CI6IkVUQU1QIn0.payload.signature";

            var exception = _jwtValidator.IsValidJwtToken(invalidJsonToken);
            Assert.Contains("Error deserializing the JWT header", exception.ErrorMessage);
        }

        [Fact]
        public void IsValidJwtToken_WithInvalidBase64Url_ThrowsInvalidOperationExceptionForFormatException()
        {
            var invalidBase64UrlToken = "notBase64.header.payload";

            var result = _jwtValidator.IsValidJwtToken(invalidBase64UrlToken);
            Assert.Contains("Format error decoding from Base64Url", result.ErrorMessage);
        }

        [Fact()]
        public async Task ValidateLifeTime_InvalidInputData_ReturnFalse()
        {
            var result = await _jwtValidator.ValidateLifeTime("", null);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task ValidateToken_InvalidInputData_ReturnFalse()
        {
            var result = await _jwtValidator.ValidateToken("", "", "", null);
            Assert.False(result.IsValid);
        }
    }
}