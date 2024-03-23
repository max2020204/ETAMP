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
        private readonly string _validToken;
        private readonly ECDsaSecurityKey _securityKey;
        private readonly Mock<JwtSecurityTokenHandler> _tokenHandlerMock;

        public JwtValidatorTests()
        {
            _jwtValidator = new JwtValidator(new JwtSecurityTokenHandler());
            _validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.zv3tcTciqfMfOcIkGfnKLm7siFr9UHiuuOocxmEuPiQ";
            _securityKey = new ECDsaSecurityKey(ECDsa.Create());
            _tokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
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
            _tokenHandlerMock.Setup(th => th.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
                            .ReturnsAsync(new TokenValidationResult { IsValid = true });

            var jwtValidator = new JwtValidator(_tokenHandlerMock.Object);

            var result = await jwtValidator.ValidateLifeTime(_validToken, _securityKey);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateToken_WithValidClaims_ReturnsTrue()
        {
            _tokenHandlerMock.Setup(th => th.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
                            .ReturnsAsync(new TokenValidationResult { IsValid = true });

            var jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await jwtValidator.ValidateToken(_validToken, "", "", _securityKey);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsValidJwtToken_WithNullOrEmptyToken_ThrowsArgumentException()
        {
            var result = _jwtValidator.IsValidJwtToken(null);

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

        [Fact]
        public async Task ValidateLifeTime_WhenTokenIsExpired_ThrowsSecurityTokenExpiredException()
        {
            string expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            _tokenHandlerMock.Setup(j => j.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
                .ThrowsAsync(new SecurityTokenExpiredException("Token has expired"));
            _jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await _jwtValidator.ValidateLifeTime(expiredToken, new ECDsaSecurityKey(ECDsa.Create()));

            Assert.False(result.IsValid);
            Assert.Contains("Token has expired", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateToken_WhenTokenSignatureIsInvalid_ThrowsSecurityTokenInvalidSignatureException()
        {
            string invalidSignatureToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            _tokenHandlerMock.Setup(j => j.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
                .ThrowsAsync(new SecurityTokenInvalidSignatureException("Invalid token signature"));
            _jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await _jwtValidator.ValidateToken(invalidSignatureToken, "audience", "issuer", new ECDsaSecurityKey(ECDsa.Create()));

            Assert.False(result.IsValid);
            Assert.Contains("Invalid token signature", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateLifeTime_WithNotYetValidToken_ReturnsFalseWithNotYetValidMessage()
        {
            var jwtSecurityTokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            jwtSecurityTokenHandlerMock
                .Setup(x => x.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                .ThrowsAsync(new SecurityTokenNotYetValidException("Token is not yet valid."));

            var jwtValidator = new JwtValidator(jwtSecurityTokenHandlerMock.Object);

            var result = await jwtValidator.ValidateLifeTime(token, securityKey);

            Assert.False(result.IsValid);
            Assert.Contains("Token is not yet valid", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateLifeTime_WithUnexpectedException_ReturnsFalseWithValidationFailedMessage()
        {
            var jwtSecurityTokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            jwtSecurityTokenHandlerMock
                .Setup(x => x.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                .ThrowsAsync(new Exception("Unexpected error occurred."));

            var jwtValidator = new JwtValidator(jwtSecurityTokenHandlerMock.Object);

            var result = await jwtValidator.ValidateLifeTime(token, securityKey);

            Assert.False(result.IsValid);
            Assert.Contains("Token validation failed", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateToken_WithTokenExpired_ThrowsSecurityTokenExpiredException()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            _tokenHandlerMock.Setup(handler => handler.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                                        .ThrowsAsync(new SecurityTokenExpiredException("Token is expired"));
            _jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await _jwtValidator.ValidateToken(token, "audience", "issuer", securityKey);

            Assert.False(result.IsValid);
            Assert.Contains("Token is expired", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateToken_WithTokenNotYetValid_ThrowsSecurityTokenNotYetValidException()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            _tokenHandlerMock.Setup(handler => handler.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                                        .ThrowsAsync(new SecurityTokenNotYetValidException("Token is not yet valid"));
            _jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await _jwtValidator.ValidateToken(token, "audience", "issuer", securityKey);

            Assert.False(result.IsValid);
            Assert.Contains("Token is not yet valid", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateToken_WithInvalidSignature_ThrowsSecurityTokenInvalidSignatureException()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            _tokenHandlerMock.Setup(handler => handler.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                                        .ThrowsAsync(new SecurityTokenInvalidSignatureException("Invalid token signature"));
            _jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await _jwtValidator.ValidateToken(token, "audience", "issuer", securityKey);

            Assert.False(result.IsValid);
            Assert.Contains("Invalid token signature", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateToken_WithGenericException_ReturnsFalse()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkVUQU1QIn0..";
            var securityKey = new ECDsaSecurityKey(ECDsa.Create());

            _tokenHandlerMock.Setup(handler => handler.ValidateTokenAsync(token, It.IsAny<TokenValidationParameters>()))
                                        .ThrowsAsync(new Exception("Generic error occurred"));
            _jwtValidator = new JwtValidator(_tokenHandlerMock.Object);
            var result = await _jwtValidator.ValidateToken(token, "audience", "issuer", securityKey);

            Assert.False(result.IsValid);
            Assert.Contains("Token validation failed: Generic error occurred", result.ErrorMessage);
        }
    }
}