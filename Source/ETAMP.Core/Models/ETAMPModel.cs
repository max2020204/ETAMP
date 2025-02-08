using System.Text;
using System.Text.Json;

namespace ETAMP.Core.Models;

/// <summary>
///     Represents the model for the ETAMP (Encrypted Token And Message Protocol) structure.
///     This model is designed to facilitate secure and efficient exchanges of messages and transactions within a network,
///     by leveraging the ETAMP framework which encapsulates data in a secure and structured manner.
/// </summary>
public struct ETAMPModel<T> where T : Token
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


    public async Task<Stream> ToJsonStreamAsync()
    {
        var stream = new MemoryStream();
        await using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Indented = false
        });

        await WriteJson(writer);
        stream.Position = 0;
        await writer.FlushAsync();
        return stream;
    }

    public async Task<string> ToJsonAsync()
    {
        var stream = await ToJsonStreamAsync();
        using StreamReader reader = new(stream);
        return await reader.ReadToEndAsync();
    }

    private async Task WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(Id), Id.ToString());
        writer.WriteNumber(nameof(Version), Version);


        if (Token != null)
        {
            writer.WritePropertyName(nameof(Token));
            writer.WriteRawValue(await Token.ToJsonAsync(), true);
        }

        if (!string.IsNullOrEmpty(UpdateType)) writer.WriteString(nameof(UpdateType), UpdateType);

        if (!string.IsNullOrEmpty(CompressionType)) writer.WriteString(nameof(CompressionType), CompressionType);

        if (!string.IsNullOrEmpty(SignatureMessage)) writer.WriteString(nameof(SignatureMessage), SignatureMessage);

        writer.WriteEndObject();
    }

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
            .Append(", CompressionType: ")
            .Append(CompressionType)
            .Append(", SignatureMessage: ").Append(SignatureMessage);
        return sb.ToString();
    }
}