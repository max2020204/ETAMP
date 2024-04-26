using Newtonsoft.Json;

namespace ETAMPManagment.Models;

/// <summary>
///     Represents the base structure for a JWT payload, initializing default values for essential properties.
/// </summary>
public class BasePayload
{
    /// <summary>
    ///     Initializes a new instance with optional default values for JTI, IssuedAt, and Expires.
    /// </summary>
    /// <param name="defaultParameters">If true, sets default values; otherwise, manual setting is required.</param>
    /// <param name="expiresHour">The number of hours until expiration, used when defaultParameters is true.</param>
    public BasePayload(bool defaultParameters = true, int expiresHour = 1)
    {
        if (!defaultParameters) return;
        JTI = Guid.NewGuid();
        IssuedAt = DateTimeOffset.UtcNow;
        Expires = DateTimeOffset.UtcNow.AddHours(expiresHour);
    }

    /// <summary>
    ///     Initializes a new instance with specific values for JTI, IssuedAt, and Expires.
    /// </summary>
    /// <param name="jti">A unique identifier for the token.</param>
    /// <param name="issuedAt">The issuance time of the token.</param>
    /// <param name="expires">The expiration time of the token.</param>
    public BasePayload(Guid jti, DateTime issuedAt, DateTime expires)
    {
        JTI = jti;
        IssuedAt = issuedAt.ToUniversalTime();
        Expires = expires.ToUniversalTime();
    }

    /// <summary>
    ///     Gets or sets the JWT ID (JTI), a unique identifier for the token.
    /// </summary>
    [JsonProperty("jti")]
    public Guid JTI { get; set; }

    /// <summary>
    ///     Gets or sets the issued at date and time for the token (IAT).
    /// </summary>
    [JsonProperty("iat")]
    public DateTimeOffset IssuedAt { get; set; }

    /// <summary>
    ///     Gets or sets the issuer of the token (ISS). This is a critical property that identifies the principal that issued
    ///     the JWT.
    /// </summary>
    [JsonProperty("iss")]
    public string? Issuer { get; set; }

    /// <summary>
    ///     Gets or sets the audience for the token (AUD), indicating who the token is intended for.
    ///     This property identifies the recipients that the JWT is intended for.
    /// </summary>
    [JsonProperty("aud")]
    public string? Audience { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time (EXP) for the token.
    /// </summary>
    [JsonProperty("exp")]
    public DateTimeOffset Expires { get; set; }
}