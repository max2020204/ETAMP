#region

using ETAMP.Console.CreateETAMPService.Models;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper.Base;
using Microsoft.Extensions.DependencyInjection;

#endregion

internal class CreateSignETAMPService
{
    private static ServiceProvider _provider;

    private static void Main(string[] args)
    {
        _provider = CreateETAMPService.ConfigureServices();
        Console.WriteLine(SignETAMP(_provider).ToJson());
    }

    public static ETAMPModel<TokenModel> SignETAMP(ServiceProvider provider)
    {
        var sign = provider.GetService<SignWrapperBase>();

        var etamp = CreateETAMPService.CreateETAMP(_provider);
        etamp.Sign(sign);
        return etamp;
    }
}