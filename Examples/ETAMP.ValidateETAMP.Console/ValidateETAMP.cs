#region

using System.Security.Cryptography;
using ETAMP.Validation.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

internal class ETAMPValidationRunner
{
    private static readonly HashAlgorithmName DefaultHashAlgorithm = HashAlgorithmName.SHA512;

    private static async Task Main(string[] args)
    {
        var provider = CreateETAMP.ConfigureServices();
        var etampValidator = provider.GetService<IETAMPValidator>();

        CreateSignETAMP.Main();

        var publicKeyBytes = Convert.FromBase64String(CreateSignETAMP.KeyModelProvider.PublicKey);
        var initializedEcdsa = CreateInitializedEcdsa(publicKeyBytes);


        etampValidator.Initialize(initializedEcdsa, DefaultHashAlgorithm);
        var validationResult = await etampValidator.ValidateETAMPAsync(CreateSignETAMP.ETAMP, false);

        Console.WriteLine(validationResult.IsValid);
    }

    private static ECDsa CreateInitializedEcdsa(byte[] publicKey)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        return ecdsa;
    }
}