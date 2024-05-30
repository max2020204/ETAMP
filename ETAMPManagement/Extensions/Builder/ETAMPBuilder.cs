using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Models;
using Newtonsoft.Json;

namespace ETAMPManagement.Extensions.Builder;

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
        var temp = new TempETAMPModel
        {
            Id = model.Id,
            Version = model.Version,
            Token = compressionService.CompressString(model.Token.ToJson()),
            UpdateType = model.UpdateType,
            CompressionType = model.CompressionType,
            SignatureMessage = model.SignatureMessage
        };
        return JsonConvert.SerializeObject(temp);
    }

    public static ETAMPModel<T> DeconstructETAMP<T>(this string? jsonEtamp,
        ICompressionServiceFactory compressionServiceFactory) where T : Token
    {
        ArgumentNullException.ThrowIfNull(compressionServiceFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(jsonEtamp);

        if (!jsonEtamp.Contains('{') && !jsonEtamp.Contains('}'))
            throw new ArgumentException("This string is not JSON");

        var tempModel = JsonConvert.DeserializeObject<TempETAMPModel>(jsonEtamp);
        var compressionService = compressionServiceFactory.Create(tempModel.CompressionType);
        var token = compressionService.DecompressString(tempModel.Token);

        return new ETAMPModel<T>
        {
            Id = tempModel.Id,
            Version = tempModel.Version,
            Token = JsonConvert.DeserializeObject<T>(token),
            UpdateType = tempModel.UpdateType,
            CompressionType = tempModel.CompressionType,
            SignatureMessage = tempModel.SignatureMessage
        };
    }
}