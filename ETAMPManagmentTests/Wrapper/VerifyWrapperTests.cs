using System.Text;
using Xunit;

namespace ETAMPManagment.Wrapper.Tests
{
    public class VerifyWrapperTests
    {
        private readonly VerifyWrapper _verifyWrapper;
        private readonly string _data = "testdata";
        private byte[] sign;

        //TODO Make Valid tests
        public VerifyWrapperTests()
        {
        }

        [Fact]
        public void VerifyData_WithCorrectDataInBytesAndSignInBytes_ReturnTrue()
        {
            var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), sign);
            Assert.True(result);
        }

        [Fact]
        public void VerifyData_WithCorrectDataInStringAndSignInString_ReturnTrue()
        {
            var result = _verifyWrapper.VerifyData(_data, Convert.ToBase64String(sign));
            Assert.True(result);
        }

        [Fact]
        public void VerifyData_WithIncorrectSignature_ReturnsFalse()
        {
            var incorrectSignature = new byte[] { 1, 2, 3 };
            var result = _verifyWrapper.VerifyData(Encoding.UTF8.GetBytes(_data), incorrectSignature);
            Assert.False(result);
        }
    }
}