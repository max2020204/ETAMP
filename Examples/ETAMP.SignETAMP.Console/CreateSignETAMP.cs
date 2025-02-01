#region

using System.Security.Cryptography;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Models;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class CreateSignETAMP
{
    private static ECDsa? _ecdsaInstance;
    public static ECDKeyModelProvider? KeyModelProvider { get; private set; }
    public static ETAMPModel<TokenModel> Etamp { get; private set; }

    public static async Task Main()
    {
        _ecdsaInstance = ECDsa.Create();
        await using var provider = CreateETAMP.ConfigureServices();
        Etamp = await SignETAMP(provider);
        Console.WriteLine(await Etamp.ToJsonAsync());
    }

    private static async Task<ETAMPModel<TokenModel>> SignETAMP(IServiceProvider provider)
    {
        var sign = provider.GetService<ISignWrapper>();
        InitializeSigning(sign);
        var etampModel = CreateETAMP.InitializeEtampModel(provider);
        return await etampModel.Sign(sign);
    }

    private static void InitializeSigning(ISignWrapper? sign)
    {
        KeyModelProvider = new ECDKeyModelProvider
        {
            PublicKey = ECDKeyModelProvider.ClearPemFormatting(_ecdsaInstance.ExportSubjectPublicKeyInfoPem())
        };
        sign?.Initialize(_ecdsaInstance, HashAlgorithmName.SHA512);
    }
}