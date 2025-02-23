using System.Buffers;
using System.IO.Pipelines;
using System.Security.Cryptography;
using System.Text;
using ETAMP.Core.Extensions;
using ETAMP.Core.Models;
using ETAMP.Core.Utils;
using ETAMP.Provider.Interfaces;

namespace ETAMP.Provider;

/// <summary>
///     Signs data using Elliptic Curve Digital Signature Algorithm (ECDsa).
/// </summary>
public sealed class ECDsaSignatureProvider : IECDsaSignatureProvider
{
    private HashAlgorithmName _algorithmName;
    private ECDsa? _ecdsa;


    /// <summary>
    ///     Signs the specified ETAMP model of type T, generating a signature message.
    /// </summary>
    /// <param name="etamp">The ETAMP model to be signed, containing the token and metadata.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type of the token contained within the ETAMP model.</typeparam>
    /// <returns>The signed ETAMP model including the generated signature message.</returns>
    public async Task<ETAMPModel<T>> SignEtampModel<T>(ETAMPModel<T> etamp,
        CancellationToken cancellationToken = default) where T : Token
    {
        ArgumentNullException.ThrowIfNull(etamp.Token, nameof(etamp.Token));

        Pipe pipe = new();

        StringBuilder sb = new();
        sb.Append(etamp.Id);
        sb.Append(etamp.Version);

        await using (var writer = new StreamWriter(pipe.Writer.AsStream(), Encoding.UTF8))
        {
            await writer.WriteAsync(sb.ToString());
            await writer.WriteAsync(await etamp.Token.ToJsonAsync(cancellationToken));
            await writer.WriteAsync(etamp.UpdateType);
            await writer.WriteAsync(etamp.CompressionType);

            await writer.FlushAsync(cancellationToken);
        }

        var result = await pipe.Reader.ReadAsync(cancellationToken);
        pipe.Reader.AdvanceTo(result.Buffer.End);
        var signature = Sign(result.Buffer.ToArray());
        etamp.SignatureMessage = Base64UrlEncoder.Encode(signature);
        return etamp;
    }

    /// <summary>
    ///     Initializes the signing provider with the specified ECDsa and hash algorithm.
    /// </summary>
    /// <param name="provider">The ECDsa provider to perform signing operations.</param>
    /// <param name="algorithmName">The name of the hash algorithm to use for signing.</param>
    public void Initialize(ECDsa? provider, HashAlgorithmName algorithmName)
    {
        _ecdsa = provider;
        _algorithmName = algorithmName;
    }

    /// <summary>
    ///     Releases all resources used by the current instance of the ECDsaSignatureProvider class.
    /// </summary>
    public void Dispose()
    {
        _ecdsa?.Dispose();
    }

    /// <summary>
    ///     Signs the provided data using the initialized ECDsa provider and hash algorithm.
    /// </summary>
    /// <param name="data">The byte array representing the data to be signed.</param>
    /// <returns>A byte array containing the generated digital signature.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ECDsa provider is not initialized.</exception>
    private byte[] Sign(ReadOnlySpan<byte> data)
    {
        return _ecdsa!.SignData(data, _algorithmName);
    }
}