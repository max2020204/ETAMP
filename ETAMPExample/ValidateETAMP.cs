using ETAMPManagement.Extensions.Builder;
using ETAMPManagement.Factory.Interfaces;
using ETAMPManagement.Models;
using ETAMPManagement.Validators.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ETAMPExample;

/// <summary>
///     Represents a class used for validating ETAMP.
/// </summary>
public class ValidateETAMP
{
    /// <summary>
    ///     Represents the provider used in the ValidateETAMP class.
    /// </summary>
    private readonly ServiceProvider _provider;

    /// <summary>
    ///     The ValidateETAMP class is responsible for validating an ETAMP message.
    ///     It uses the ETAMPValidatorBase class for validation, the ICompressionServiceFactory interface for decompression,
    ///     and the CreateSignETAMP class for creating and signing the ETAMP message.
    /// </summary>
    public ValidateETAMP(ServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    ///     Validates an ETAMP model.
    /// </summary>
    /// <returns>True if the ETAMP model is valid, False otherwise.</returns>
    public bool Validate()
    {
        var validator = _provider.GetService<ETAMPValidatorBase>();
        var compression = _provider.GetService<ICompressionServiceFactory>();
        var createSignETAMP = new CreateSignETAMP(_provider);

        var etamp = createSignETAMP.CreateAndSignETAMPMessage();
        var model = etamp.DeconstructETAMP<Token>(compression);

        validator?.Initialize(createSignETAMP.ECDsaProvider, createSignETAMP.HashAlgorithm);
        var result = validator?.ValidateETAMP(model, false);

        return result.IsValid;
    }
}