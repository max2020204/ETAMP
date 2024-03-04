using Newtonsoft.Json;

namespace ETAMPManagment.Models
{
    /// <summary>
    /// Represents the base payload structure for a JSON Web Token (JWT).
    /// This class automatically initializes default values for JTI, IssuedAt, and Expires properties.
    /// When using default parameters, ensure to manually set 'Issuer' and 'Audience' as they are essential for token validation.
    /// </summary>
    public class BasePaylaod
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
        /// If defaultParameters is true, initializes the payload with default values, including a unique JTI, current issued at date and time, and an expiration time set to 1 hour from now.
        /// Note: When using default parameters, 'Issuer' and 'Audience' must be set manually after object initialization.
        /// </summary>
        /// <param name="defaultParameters">If true, initializes the payload with default values. Default is true.</param>
        /// <param name="expiresHour">The number of hours after which the token should expire. Default is 1 hour.</param>
        public BasePaylaod(bool defualtParametrs = true, int expiresHour = 1)
        {
            if (defualtParametrs)
            {
                JTI = Guid.NewGuid();
                IssuedAt = DateTime.Now.ToUniversalTime();
                Expires = DateTime.Now.AddHours(expiresHour);
            }
        }
    }
}