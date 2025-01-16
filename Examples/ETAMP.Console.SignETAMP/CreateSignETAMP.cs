#region

using System.Security.Cryptography;
using ETAMP.Console.CreateETAMP.Models;
using ETAMP.Core.Models;
using ETAMP.Encryption.Base;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper.Base;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class CreateSignETAMP
{
    private static ServiceProvider _provider;

    private static ECDsa _ecdsaInstance;

    public static ECDsa Ecdsa
    {
        get
        {
            if (_ecdsaInstance == null) _ecdsaInstance = ECDsa.Create();
            return _ecdsaInstance;
        }
        set => _ecdsaInstance =
            value ?? throw new ArgumentNullException(nameof(value), "ECDsa instance cannot be null.");
    }

    public static string PublicKey { get; private set; }
    public static string ETAMPSigned { get; private set; }

    private static void Main(string[] args)
    {
        _ecdsaInstance = ECDsa.Create();
        _provider = CreateETAMP.ConfigureServices();
        SignETAMP(_provider);
        Console.WriteLine(ETAMPSigned);
    }

    public static ETAMPModel<TokenModel> SignETAMP(IServiceProvider provider)
    {
        var (sign, ecdsaProviderBase, pemCleaner) = GetServices(provider);

        InitializeSigning(sign, ecdsaProviderBase, pemCleaner);

        var etampModel = CreateETAMP.InitializeEtampModel(provider);
        etampModel.Sign(sign);
        ETAMPSigned = etampModel.ToJson();
        return etampModel;
    }

    private static (SignWrapperBase?, ECDsaProviderBase?, IPemKeyCleaner?) GetServices(IServiceProvider provider)
    {
        return (
            provider.GetService<SignWrapperBase>(),
            provider.GetService<ECDsaProviderBase>(),
            provider.GetService<IPemKeyCleaner>()
        );
    }

    private static void InitializeSigning(SignWrapperBase sign, ECDsaProviderBase ecdsaProviderBase,
        IPemKeyCleaner pemCleaner)
    {
        _ecdsaInstance ??= ECDsa.Create();
        ecdsaProviderBase.SetECDsa(_ecdsaInstance);
        var publicKeyPem = pemCleaner.ClearPemPublicKey(_ecdsaInstance.ExportSubjectPublicKeyInfoPem());
        PublicKey = publicKeyPem.KeyModelProvider.PublicKey;

        sign.Initialize(ecdsaProviderBase, HashAlgorithmName.SHA512);
    }
}