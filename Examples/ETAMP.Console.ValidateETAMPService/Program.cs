using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Console.CreateETAMPService;
using ETAMP.Validation.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMP.Console.ValidateETAMPService;

internal class Program
{
    private static void Main(string[] args)
    {
        var provider = Service.ConfigureServices();
        var etamp = Service.CreateETAMP();

        var validator = provider.GetService<ETAMPValidatorBase>();
        var compression = provider.GetService<ICompressionServiceFactory>();
    }
}