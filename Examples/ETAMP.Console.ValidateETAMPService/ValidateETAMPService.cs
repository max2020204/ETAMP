#region

using System.Security.Cryptography;
using ETAMP.Encryption.Base;
using ETAMP.Validation.Base;
using Microsoft.Extensions.DependencyInjection;

#endregion

internal class ETAMPValidationRunner
{
    private static readonly HashAlgorithmName DefaultHashAlgorithm = HashAlgorithmName.SHA512;

    private static void Main(string[] args)
    {
        var provider = CreateETAMPService.ConfigureServices();
        var etampValidator = provider.GetService<ETAMPValidatorBase>();
        var ecdsaProvider = provider.GetService<ECDsaProviderBase>();
        var etamp = CreateSignETAMPService.SignETAMP(provider);

        // Initialize and set up ECDsa
        var publicKeyBytes = Convert.FromBase64String(CreateSignETAMPService.PublicKey);
        var initializedEcdsa = CreateInitializedEcdsa(publicKeyBytes);
        ecdsaProvider.SetECDsa(initializedEcdsa);

        // Configure validator and validate ETAMP
        etampValidator.Initialize(ecdsaProvider, DefaultHashAlgorithm);
        var validationResult = etampValidator.ValidateETAMP(etamp, false);

        Console.WriteLine(validationResult.IsValid);
    }

    private static ECDsa CreateInitializedEcdsa(byte[] publicKey)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        return ecdsa;
    }
}