using System.Text;

namespace ETAMP.Core.Models;

/// <summary>
///     Represents a model used in the ETAMP protocol with generic support for token types.
/// </summary>
/// <typeparam name="T">The type of token associated with the model, constrained to inherit from Token.</typeparam>
public record ETAMPModel<T> where T : Token
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
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Version, Token, UpdateType, CompressionType, SignatureMessage);
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("Id: ").Append(Id)
            .Append(", Version: ").Append(Version)
            .Append(", UpdateType: ").Append(UpdateType)
            .Append(", Token: ").Append(Token)
            .Append("CompressionType: ").Append(CompressionType)
            .Append(", SignatureMessage: ").Append(SignatureMessage);
        return sb.ToString();
    }
}