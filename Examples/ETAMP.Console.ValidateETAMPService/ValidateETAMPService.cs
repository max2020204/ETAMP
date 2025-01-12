#region

using ETAMP.Compression.Interfaces.Factory;
using ETAMP.Validation.Base;
using Microsoft.Extensions.DependencyInjection;

#endregion

internal class ValidateETAMPService
{
    private static void Main(string[] args)
    {
        var provider = CreateETAMPService.ConfigureServices();

        var validator = provider.GetService<ETAMPValidatorBase>();
        var compression = provider.GetService<ICompressionServiceFactory>();


        var etamp = CreateETAMPService.CreateETAMP(provider);
    }
}