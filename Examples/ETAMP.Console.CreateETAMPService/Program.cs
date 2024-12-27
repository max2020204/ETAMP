using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Console.CreateETAMPService.Models;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

public static class Service
{
    private static ServiceProvider _provider;

    private static void Main(string[] args)
    {
        _provider = ConfigureServices();
        var compression = _provider.GetService<ICompressionServiceFactory>();
        Console.WriteLine(CreateETAMP(_provider).Build(compression));
    }

    public static ETAMPModel<TokenModel> CreateETAMP(ServiceProvider provider)
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