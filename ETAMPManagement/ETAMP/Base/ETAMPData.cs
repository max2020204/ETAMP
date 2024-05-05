#region

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ETAMPManagement.ETAMP.Base.Interfaces;
using ETAMPManagement.Models;
using ETAMPManagement.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

#endregion

namespace ETAMPManagement.ETAMP.Base;

/// <summary>
///     Implements the ETAMP data processing logic, providing a method to create a digitally signed ETAMP token.
///     This class uses a signing credentials provider to sign the data, ensuring the integrity and authenticity of the
///     ETAMP tokens.
/// </summary>
public class ETAMPData : IETAMPData
{
    private readonly ISigningCredentialsProvider _signingCredentialsProvider;

    /// <summary>
    ///     Initializes a new instance of the ETAMPData class with a default signing credentials provider.
    /// </summary>
    /// <param name="signingCredentialsProvider">The default provider used to create signing credentials.</param>
    public ETAMPData(ISigningCredentialsProvider signingCredentialsProvider)
    {
        _signingCredentialsProvider = signingCredentialsProvider ??
                                      throw new ArgumentNullException(nameof(signingCredentialsProvider));
    }

    /// <summary>
    ///     Creates ETAMP token data with a digital signature for the specified payload and message ID.
    ///     An optional custom signing credentials provider can be specified, otherwise, the default provider is used.
    /// </summary>
    /// <typeparam name="T">The type of the payload to be included in the ETAMP token.</typeparam>
    /// <param name="messageId">The message identifier, uniquely identifying the ETAMP token.</param>
    /// <param name="payload">The payload to include in the ETAMP token. This data will be serialized and signed.</param>
    /// <param name="provider">
    ///     Optional. A custom provider for signing credentials that can be specified for custom signature
    ///     requirements. If null, the default provider is used.
    /// </param>
    /// <returns>A string representing the serialized and signed ETAMP token data.</returns>
    public string CreateEtampData<T>(string messageId, T payload, ISigningCredentialsProvider? provider = null)
        where T : BasePayload
    {
        var credentialsProvider = provider ?? _signingCredentialsProvider;
        return CreateETAMP(messageId, payload, credentialsProvider.CreateSigningCredentials());
    }

    /// <summary>
    ///     Creates a digitally signed ETAMP token based on the given payload, message ID, and signing credentials.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    /// <param name="messageId">The unique identifier for the message.</param>
    /// <param name="payload">The payload to be included in the token.</param>
    /// <param name="provider">The signing credentials provider used to sign the token.</param>
    /// <returns>The serialized JWT token as a string.</returns>
    private string CreateETAMP<T>(string messageId, T payload, SigningCredentials signing) where T : BasePayload
    {
        var handler = new JwtSecurityTokenHandler();
        var json = JObject.FromObject(payload);
        var claimsDictionary = json.ToObject<Dictionary<string, object>>();

        var descriptor = new SecurityTokenDescriptor
        {
            TokenType = "ETAMP",
            Claims = claimsDictionary,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("MessageId", messageId),
                new Claim("Timestamp", DateTimeOffset.UtcNow.ToString())
            }),
            SigningCredentials = signing
        };

        return handler.WriteToken(handler.CreateToken(descriptor));
    }
}