using Newtonsoft.Json;

namespace ETAMPManagment.Models
{
    /// <summary>
    /// Represents the base payload structure for a JSON Web Token (JWT).
    /// This class automatically initializes default values for JTI, IssuedAt, and Expires properties.
    /// When using default parameters, ensure to manually set 'Issuer' and 'Audience' as they are essential for token validation.
    /// </summary>
    public class BasePayload
    {
        /// <summary>
        /// Gets or sets the JWT ID (JTI), a unique identifier for the token.
        /// </summary>
        [JsonProperty("jti")]
        public Guid JTI { get; set; }

        /// <summary>
        /// Gets or sets the issued at date and time for the token (IAT).
        /// </summary>
        [JsonProperty("iat")]
        public DateTimeOffset IssuedAt { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the token (ISS). This is a critical property that identifies the principal that issued the JWT.
        /// </summary>
        [JsonProperty("iss")]
        public string? Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience for the token (AUD), indicating who the token is intended for.
        /// This property identifies the recipients that the JWT is intended for.
        /// </summary>
        [JsonProperty("aud")]
        public string? Audience { get; set; }

        /// <summary>
        /// Gets or sets the expiration time (EXP) for the token.
        /// </summary>
        [JsonProperty("exp")]
        public DateTimeOffset Expires { get; set; }

        /// <summary>
        /// Initializes a new instance of the BasePayload class with optional default parameters.
        /// This constructor allows for the automatic setting of essential token metadata such as JTI (JWT ID), 'IssuedAt', and 'Expires'.
        /// </summary>
        /// <remarks>
        /// When 'defaultParameters' is set to true, the constructor initializes the payload with default values:
        /// a unique JTI, the current timestamp for 'IssuedAt', and an 'Expires' time set to a specified number of hours from the current time.
        /// The 'Issuer' and 'Audience' properties must be manually set after initialization as they are essential for token validation but not auto-generated.
        /// </remarks>
        /// <param name="defaultParameters">If true, initializes the payload with default values for JTI, IssuedAt, and Expires.
        /// If false, these properties must be manually set after object creation. Default is true.</param>
        /// <param name="expiresHour">The number of hours from the current time when the token should expire.
        /// This parameter is used to calculate the 'Expires' property. Default is 1 hour.</param>
        public BasePayload(bool defaultParameters = true, int expiresHour = 1)
        {
            if (defaultParameters)
            {
                JTI = Guid.NewGuid();
                IssuedAt = DateTimeOffset.UtcNow;
                Expires = DateTimeOffset.UtcNow.AddHours(expiresHour);
            }
        }

        /// <summary>
        /// Initializes a new instance of the BasePayload class using specific values for JTI, issued at date and time, and expiration time.
        /// </summary>
        /// <remarks>
        /// This constructor is suitable for scenarios where explicit control over the token's metadata is required, allowing the caller to specify
        /// exact values for the JTI, 'IssuedAt', and 'Expires' properties. It's important to ensure that these values are correctly set to maintain
        /// the validity and integrity of the token.
        /// </remarks>
        /// <param name="jti">The JWT ID (JTI), a unique identifier for the token. This value should be a unique GUID.</param>
        /// <param name="issuedAt">The date and time when the token is issued (IAT). This should represent the time of token creation in UTC.</param>
        /// <param name="expires">The expiration time (EXP) for the token. This should be a future time in UTC when the token will no longer be valid.</param>
        public BasePayload(Guid jti, DateTime issuedAt, DateTime expires)
        {
            JTI = jti;
            IssuedAt = issuedAt.ToUniversalTime();
            Expires = expires.ToUniversalTime();
        }
    }
}