using System.Security.Cryptography;
using ETAMP.Console.CreateETAMP;
using ETAMP.Core.Models;
using ETAMP.Encryption.ECDsaManager;
using ETAMP.Encryption.Interfaces.ECDSAManager;
using ETAMP.Extension.Builder;
using ETAMP.Wrapper;
using ETAMP.Wrapper.Base;

internal static class ETAMPSignProgram
{
    private static IECDsaProvider? _ecdsaProvider;
    private static HashAlgorithmName _hashAlgorithm;
    private static SignWrapperBase _signWrapperBase;

    static void Main(string[] args)
    {
        Initialize(ref _signWrapperBase);
        
        //Sign
        var etamp = SignETAMP();
        //comrpress
        Console.WriteLine(etamp.ToJson());
    }

    private static void Initialize(ref SignWrapperBase sign)
    {
        sign = new SignWrapper();
        var ecDsaProvider = new ECDsaProvider();
        _ecdsaProvider = ecDsaProvider;
        var creator = new ECDsaCreator(ecDsaProvider);
        creator.CreateECDsa();
        _hashAlgorithm = HashAlgorithmName.SHA512;
        sign.Initialize(_ecdsaProvider, _hashAlgorithm);
    }

    public static ETAMPModel<TokenModel> SignETAMP()
    {
        var create = new Create();
        return create.CreateETAMP().Sign(_signWrapperBase);
    }
}