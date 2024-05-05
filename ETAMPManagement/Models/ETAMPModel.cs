#region

using Newtonsoft.Json;

#endregion

namespace ETAMPManagement.Models;

/// <summary>
///     Represents the model for the ETAMP (Encrypted Token And Message Protocol) structure.
///     This model is designed to facilitate secure and efficient exchanges of messages and transactions within a network,
///     by leveraging the ETAMP framework which encapsulates data in a secure and structured manner.
/// </summary>
public class ETAMPModel
{
    /// <summary>
    ///     Gets or sets the unique identifier for the ETAMP model instance.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the version of the ETAMP protocol. This information is used to ensure compatibility across different
    ///     versions of the protocol.
    /// </summary>
    public double Version { get; set; }

    /// <summary>
    ///     Gets or sets the token in JWT (JSON Web Token) format. This token serves as a versatile container for encapsulating
    ///     data,
    ///     which can be encrypted using ECIES with AES encryption method or simply encoded in Base64.
    ///     This approach provides a robust mechanism for ensuring the confidentiality and integrity of the data within the
    ///     ETAMP framework.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     Gets or sets the type of update this model represents, providing context for the transaction or message exchange.
    /// </summary>
    public string? UpdateType { get; set; }

    /// <summary>
    ///     Gets or sets the signature for the token, which serves to verify the authenticity and integrity of the token,
    ///     ensuring that it has not been tampered with.
    /// </summary>
    public string? SignatureToken { get; set; }

    /// <summary>
    ///     Gets or sets the signature for the message, adding an additional layer of security by safeguarding the integrity of
    ///     the message within the ETAMP structure.
    /// </summary>
    public string? SignatureMessage { get; set; }

    /// <summary>
    ///     Converts the ETAMP model instance to its JSON string representation.
    /// </summary>
    /// <returns>A JSON string representing the current state of the ETAMPModel instance.</returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }


    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    ///     <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is ETAMPModel other)
            return Id == other.Id
                   && Version == other.Version
                   && UpdateType == other.UpdateType
                   && Token == other.Token
                   && SignatureToken == other.SignatureToken
                   && SignatureMessage == other.SignatureMessage;

        return false;
    }

    /// <summary>
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Version, Token, UpdateType, SignatureToken, SignatureMessage);
    }
}