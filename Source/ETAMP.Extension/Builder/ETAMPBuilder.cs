#region

using System.Text.Json;
using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Core.Models;

#endregion

namespace ETAMP.Extension.Builder;

public static class ETAMPBuilder
{
    public static string? Build<T>(this ETAMPModel<T> model, ICompressionServiceFactory compressionServiceFactory)
        where T : Token
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(model.Token);
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(model.CompressionType);
        var compressionService = compressionServiceFactory.Create(model.CompressionType);
        var temp = new ETAMPModelBuilder
        {
            Id = model.Id,
            Version = model.Version,
            Token = compressionService.CompressString(model.Token.ToJson()),
            UpdateType = model.UpdateType,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
        return JsonSerializer.Serialize(temp);
    }

    public static ETAMPModel<T> DeconstructETAMP<T>(this string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory) where T : Token
    {
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonEtamp);

        if (!jsonEtamp.Contains('{') && !jsonEtamp.Contains('}'))
            throw new ArgumentException("This string is not JSON");

        var tempModel = JsonSerializer.Deserialize<ETAMPModelBuilder>(jsonEtamp);
        var compressionService = compressionServiceFactory.Create(tempModel.CompressionType);
        var token = compressionService.DecompressString(tempModel.Token);

        return new ETAMPModel<T>
        {
            Id = tempModel.Id,
            Version = tempModel.Version,
            Token = JsonSerializer.Deserialize<T>(token),
            UpdateType = tempModel.UpdateType,
            CompressionType = tempModel.CompressionType,
            SignatureMessage = tempModel.SignatureMessage
        };
    }
}