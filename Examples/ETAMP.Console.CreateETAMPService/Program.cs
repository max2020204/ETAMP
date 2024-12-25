using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Console.CreateETAMPService.Models;
using ETAMP.Core.Interfaces;
using ETAMP.Core.Management;
using ETAMP.Extension.Builder;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

public static class Service
{
    private static ServiceProvider _provider;

    private static void Main(string[] args)
    {
        _provider = ConfigureServices();
        Console.WriteLine(CreateETAMP());
    }

    public static string CreateETAMP()
    {
        var etampBase = _provider.GetService<IETAMPBase>();
        var compression = _provider.GetService<ICompressionServiceFactory>();
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

        var model = etampBase.CreateETAMPModel("Message", tokenModel, CompressionNames.GZip);
        return model.Build(compression);
    }

    public static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddETAMPServices();
        return serviceCollection.BuildServiceProvider();
    }
}