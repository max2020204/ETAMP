using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography;
using Xunit;

namespace ETAMPManagment.Services.Tests
{
    public class ECDsaSigningCredentialsProviderTests
    {
        [Fact]
        public void CreateSigningCredentials_ReturnsSigningCredentialsWithCorrectAlgorithm()
        {
            using var ecdsa = ECDsa.Create();
            string algorithm = "ES256";
            var provider = new ECDsaSigningCredentialsProvider(ecdsa, algorithm);

            var signingCredentials = provider.CreateSigningCredentials();
            Assert.NotNull(signingCredentials);
            Assert.IsType<ECDsaSecurityKey>(signingCredentials.Key);
            Assert.Equal(algorithm, signingCredentials.Algorithm);
            Assert.Same(ecdsa, ((ECDsaSecurityKey)signingCredentials.Key).ECDsa);
        }

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            var curve = ECCurve.NamedCurves.nistP256;
            var securityAlgorithm = SecurityAlgorithms.EcdsaSha256Signature;

            var provider = new ECDsaSigningCredentialsProvider(curve, securityAlgorithm);

            Assert.NotNull(provider);
            var ecdsaFieldValue = GetPrivateFieldValue<ECDsa>(provider, "_ecdsa");
            Assert.NotNull(ecdsaFieldValue);
            Assert.Equal(securityAlgorithm, GetPrivateFieldValue<string>(provider, "_securityAlgorithm"));

            Assert.Equal(curve.Oid.FriendlyName, ecdsaFieldValue.ExportParameters(false).Curve.Oid.FriendlyName);
        }

        private T GetPrivateFieldValue<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return (T)field.GetValue(obj);
            }

            throw new InvalidOperationException($"Field '{fieldName}' not found in object of type {obj.GetType().FullName}.");
        }
    }
}