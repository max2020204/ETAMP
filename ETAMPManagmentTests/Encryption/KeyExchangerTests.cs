#region

using System.Security.Cryptography;
using ETAMPManagment.Encryption.Interfaces;
using Moq;
using Xunit;

#endregion

namespace ETAMPManagment.Encryption.Tests;

public class KeyExchangerTests
{
    private readonly Mock<ECDiffieHellman> _eCDiffieHellmanMock;
    private readonly KeyExchanger _exchanger;
    private readonly Mock<IKeyPairProvider> _keyProviderMock;

    public KeyExchangerTests()
    {
        _keyProviderMock = new Mock<IKeyPairProvider>();
        _eCDiffieHellmanMock = new Mock<ECDiffieHellman>();
        _keyProviderMock.Setup(x => x.GetECDiffieHellman()).Returns(_eCDiffieHellmanMock.Object);
        _exchanger = new KeyExchanger();
        _exchanger.Initialize(_keyProviderMock.Object);
    }

    [Fact]
    public void DeriveKey_ThrowsArgumentNullException_WhenPublicKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _exchanger.DeriveKey(publicKey: null));
    }

    [Fact]
    public void DeriveKey_WithValidPublicKey_ReturnsExpectedDerivedKey()
    {
        var data = new byte[] { 1, 2, 3 };
        var publicKeyMock = new Mock<ECDiffieHellmanPublicKey>();
        var publicKey = publicKeyMock.Object;

        _eCDiffieHellmanMock.Setup(x => x.DeriveKeyMaterial(publicKey)).Returns(data);

        var result = _exchanger.DeriveKey(publicKey);

        Assert.Equal(data, result);
    }

    [Fact]
    public void DeriveKey_WithValidPublicKeyArray_CallsDeriveKeyMaterial()
    {
        var expectedDerivedKeyMaterial = new byte[] { 1, 2, 3, 4 };
        var ecdh = ECDiffieHellman.Create();
        var rawPublicKey = Convert.FromBase64String(ecdh.ExportSubjectPublicKeyInfoPem()
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", ""));

        _eCDiffieHellmanMock.Setup(ecdh => ecdh.DeriveKeyMaterial(It.IsAny<ECDiffieHellmanPublicKey>()))
            .Returns(expectedDerivedKeyMaterial);

        var derivedKeyMaterial = _exchanger.DeriveKey(rawPublicKey);

        Assert.Equal(expectedDerivedKeyMaterial, derivedKeyMaterial);
    }

    [Fact]
    public void DeriveKeyHash_ThrowsArgumentNullException_WhenPublicKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _exchanger.DeriveKeyHash(null, HashAlgorithmName.SHA256, null, null));
    }

    [Fact]
    public void DeriveKeyHash_ReturnsCorrectDerivedKey()
    {
        var dummyPublicKey = new Mock<ECDiffieHellmanPublicKey>().Object;
        var expectedKey = new byte[] { 1, 2, 3, 4 };
        var prependData = new byte[] { 5, 6 };
        var appendData = new byte[] { 7, 8 };
        var hashAlgorithm = HashAlgorithmName.SHA256;

        _eCDiffieHellmanMock.Setup(m => m.DeriveKeyFromHash(dummyPublicKey, hashAlgorithm, prependData, appendData))
            .Returns(expectedKey);

        var result = _exchanger.DeriveKeyHash(dummyPublicKey, hashAlgorithm, prependData, appendData);

        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void DeriveKeyHmac_ThrowsArgumentNullException_WhenPublicKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _exchanger.DeriveKeyHmac(null, HashAlgorithmName.SHA256, new byte[0], null, null));
    }

    [Fact]
    public void DeriveKeyHmac_ReturnsCorrectDerivedKey()
    {
        var dummyPublicKey = new Mock<ECDiffieHellmanPublicKey>().Object;
        var expectedKey = new byte[] { 9, 10, 11, 12 };
        var hmacKey = new byte[] { 13, 14 };
        var prependData = new byte[] { 15, 16 };
        var appendData = new byte[] { 17, 18 };
        var hashAlgorithm = HashAlgorithmName.SHA256;

        _eCDiffieHellmanMock.Setup(m =>
                m.DeriveKeyFromHmac(dummyPublicKey, hashAlgorithm, hmacKey, prependData, appendData))
            .Returns(expectedKey);

        var result = _exchanger.DeriveKeyHmac(dummyPublicKey, hashAlgorithm, hmacKey, prependData, appendData);

        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void GetSharedSecret_ReturnsExpectedSecret_AfterKeyDerivation()
    {
        var expectedSecret = new byte[] { 19, 20, 21, 22 };
        var dummyPublicKey = new Mock<ECDiffieHellmanPublicKey>().Object;

        _eCDiffieHellmanMock.Setup(m => m.DeriveKeyMaterial(dummyPublicKey)).Returns(expectedSecret);

        _exchanger.DeriveKey(dummyPublicKey);

        var sharedSecret = _exchanger.GetSharedSecret();

        Assert.Equal(expectedSecret, sharedSecret);
    }

    [Fact]
    public void DeriveKey_UpdatesSharedSecret()
    {
        var publicKeyMock = new Mock<ECDiffieHellmanPublicKey>();
        var publicKey = publicKeyMock.Object;
        var newSecret = new byte[] { 23, 24, 25, 26 };

        _eCDiffieHellmanMock.Setup(x => x.DeriveKeyMaterial(publicKey)).Returns(newSecret);

        var initialSecret = _exchanger.GetSharedSecret();
        _exchanger.DeriveKey(publicKey);
        var updatedSecret = _exchanger.GetSharedSecret();

        Assert.NotEqual(initialSecret, updatedSecret);
        Assert.Equal(newSecret, updatedSecret);
    }
}