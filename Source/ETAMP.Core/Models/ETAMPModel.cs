#region

using System.Text.Json;

#endregion

namespace ETAMP.Core.Models;

/// <summary>
///     Represents the model for the ETAMP (Encrypted Token And Message Protocol) structure.
///     This model is designed to facilitate secure and efficient exchanges of messages and transactions within a network,
///     by leveraging the ETAMP framework which encapsulates data in a secure and structured manner.
/// </summary>
public class ETAMPModel<T> where T : Token
{
    /// <summary>
    ///     Gets or sets the unique identifier for the ETAMP model instance.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Gets or sets the version of the ETAMP protocol. This information is used to ensure compatibility across different
    ///     versions of the protocol.
    /// </summary>
    public double Version { get; init; }

    /// <summary>
    ///     Represents a token used in the ETAMP model.
    /// </summary>
    /// <typeparam name="T">The type of the token.</typeparam>
    public T? Token { get; init; }

    /// <summary>
    ///     Gets or sets the type of update this model represents, providing context for the transaction or message exchange.
    /// </summary>
    public string? UpdateType { get; init; }

    /// <summary>
    ///     Gets or sets the compression type used for the ETAMP (Encrypted Token And Message Protocol) structure.
    ///     The compression type determines the algorithm used to compress the data within the ETAMP structure,
    ///     allowing for efficient and compact representation of the information being exchanged.
    /// </summary>
    public string? CompressionType { get; set; }


    /// <summary>
    ///     Gets or sets the signature for the message, adding a layer of security by safeguarding the integrity of
    ///     the message within the ETAMP structure.
    /// </summary>
    public string? SignatureMessage { get; set; }


    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    ///     <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is ETAMPModel<T> other)
            return Id == other.Id
                   && Version.Equals(other.Version)
                   && UpdateType == other.UpdateType
                   && Token == other.Token
                   && CompressionType == other.CompressionType
                   && SignatureMessage == other.SignatureMessage;

        return false;
    }


    /// <summary>
    ///     Converts the ETAMPModel object to a JSON string.
    /// </summary>
    /// <returns>
    ///     The JSON string representation of the ETAMPModel object.
    /// </returns>
    public string ToJson()
    {
        var temp = new ETAMPModelBuilder
        {
            Id = Id,
            Version = Version,
            Token = Token?.ToJson(),
            UpdateType = UpdateType,
            CompressionType = CompressionType,
            SignatureMessage = SignatureMessage
        };
        return JsonSerializer.Serialize(temp);
    }

    /// <summary>
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Version, Token, UpdateType, CompressionType, SignatureMessage);
    }

    /// <summary>
    ///     Returns a string that represents the current ETAMPModel object.
    /// </summary>
    /// <returns>
    ///     A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
        return ToJson();
    }
}