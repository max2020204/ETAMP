using ETAMP.Wrapper;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace ETAMPTests.Wrapper
{
    public class VerifyWrapperTests
    {
        private readonly VerifyWrapper _verifyWrapper;
        private readonly string _data = "testdata";
        private byte[] sign;

        public VerifyWrapperTests()
        {
            _verifyWrapper = new VerifyWrapper(new EcdsaWrapper(), HashAlgorithmName.SHA256);
            ECDsa ecdsa = _verifyWrapper.ECDsa;
            sign = ecdsa.SignData(Encoding.UTF8.GetBytes(_data), HashAlgorithmName.SHA256);
        }

        [Fact]
        public void VerifyData_WithCorrectDataInBytesAndSignInBytes_ReturnTrue()
        {
            var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), sign);
            Assert.True(result);
        }

        [Fact]
        public void VerifyData_WithCorrectDataInStringAndSignInBytes_ReturnTrue()
        {
            var result = _verifyWrapper.VerifyData(_data, sign);
            Assert.True(result);
        }

        [Fact]
        public void VerifyData_WithCorrectDataInStringAndSignInString_ReturnTrue()
        {
            var result = _verifyWrapper.VerifyData(_data, Convert.ToBase64String(sign));
            Assert.True(result);
        }

        [Fact]
        public void VerifyData_WithCorrectDataInBytesAndSignInString_ReturnTrue()
        {
            var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), Convert.ToBase64String(sign));
            Assert.True(result);
        }

        [Fact]
        public void VerifyData_WithIncorrectSignature_ReturnsFalse()
        {
            var incorrectSignature = new byte[] { 1, 2, 3 }; // Пример неверной подписи
            var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), incorrectSignature);
            Assert.False(result);
        }

        [Fact]
        public void VerifyWrapper_WithCustomCurve_ReturnCorrect()
        {
            VerifyWrapper verifyWrapper = new VerifyWrapper(new EcdsaWrapper(), ECCurve.NamedCurves.nistP521, HashAlgorithmName.SHA256);
            Assert.NotNull(verifyWrapper);
            Assert.Equal(521, verifyWrapper.ECDsa.KeySize);
        }

        [Fact]
        public void VerifyWrapper_WithPublickeyAndCustomCurve_ReturnCorrect()
        {
            ECDsa ecdsa = ECDsa.Create();
            string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                   .Replace("-----END PUBLIC KEY-----", "")
                                                                   .Replace("\n", "");
            VerifyWrapper verifyWrapper = new VerifyWrapper(new EcdsaWrapper(), publicKey, ECCurve.NamedCurves.nistP521, HashAlgorithmName.SHA256);
            Assert.NotNull(verifyWrapper);
            Assert.Equal(521, verifyWrapper.ECDsa.KeySize);
        }

        [Fact]
        public void VerifyWrapper_WithPublickeyInBytesAndCustomCurve_ReturnCorrect()
        {
            ECDsa ecdsa = ECDsa.Create();
            string publicKey = ecdsa.ExportSubjectPublicKeyInfoPem().Replace("-----BEGIN PUBLIC KEY-----", "")
                                                                   .Replace("-----END PUBLIC KEY-----", "")
                                                                   .Replace("\n", "");
            VerifyWrapper verifyWrapper = new VerifyWrapper(new EcdsaWrapper(), Convert.FromBase64String(publicKey), ECCurve.NamedCurves.nistP521, HashAlgorithmName.SHA256);
            Assert.NotNull(verifyWrapper);
            Assert.Equal(521, verifyWrapper.ECDsa.KeySize);
        }
    }
}