#region

using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

#endregion

public static class CreateETAMP
{
    private static ServiceProvider _provider;

    private static async Task Main(string[] args)
    {
        _provider = ConfigureServices();
        var compression = _provider.GetService<ICompressionServiceFactory>();
        var etampModel = InitializeEtampModel(_provider);
        Console.WriteLine(await etampModel.BuildAsync(compression));
    }

    public static ETAMPModel<TokenModel> InitializeEtampModel(IServiceProvider provider)
    {
        var etampBase = provider.GetService<IETAMPBase>();

        var tokenModel = new TokenModel
        {
            Message = "Hello World!",
            Email = "<EMAIL>",
            Data = "Some data",
            IsEncrypted = false,
            LastName = "Last",
            Name = "Name",
            Phone = "+1234567890"
        };

        return etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip);
    }

    public static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddETAMPServices();
        return serviceCollection.BuildServiceProvider();
    }
}