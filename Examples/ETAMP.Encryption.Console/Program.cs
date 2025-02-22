using System.Buffers;
using System.Buffers.Text;
using System.IO.Pipelines;
using System.Security.Cryptography;
using System.Text;
using ETAMP.Core.Utils;
using ETAMP.Encryption.Interfaces;
using ETAMP.Extension.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static IServiceProvider _provider;
    private static IECIESEncryptionService _ecies;

    private static async Task Main(string[] args)
    {
        Setup();

        var data = "String to encrypt";

        var dataPipe = new Pipe();
        var outputPipe = new Pipe();

        Base64Url.EncodeToUtf8(Encoding.UTF8.GetBytes(data), outputPipe.Writer.GetSpan());


        await dataPipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(data));
        await dataPipe.Writer.CompleteAsync();

        var person1 = ECDiffieHellman.Create();
        var person2 = ECDiffieHellman.Create();


        await _ecies.EncryptAsync(dataPipe.Reader, outputPipe.Writer, person1, person2.PublicKey);

        var result = await outputPipe.Reader.ReadAsync();
        var encryptedData = Base64UrlEncoder.Encode(result.Buffer.ToArray());
        outputPipe.Reader.AdvanceTo(result.Buffer.End);
        await outputPipe.Reader.CompleteAsync();
        Console.WriteLine(encryptedData);

        var decryptionPipe = new Pipe();
        var decryptionOutputPipe = new Pipe();
        await decryptionPipe.Writer.WriteAsync(Base64UrlEncoder.DecodeBytes(encryptedData));
        await decryptionPipe.Writer.CompleteAsync();

        await _ecies.DecryptAsync(decryptionPipe.Reader, decryptionOutputPipe.Writer, person2, person1.PublicKey);
        var result2 = await decryptionOutputPipe.Reader.ReadAsync();
        var decryptedData = result2.Buffer.ToArray();
        decryptionOutputPipe.Reader.AdvanceTo(result2.Buffer.End);
        await decryptionOutputPipe.Reader.CompleteAsync();
        Console.WriteLine(Encoding.UTF8.GetString(decryptedData));
    }

    private static void Setup()
    {
        ServiceCollection services = new();
        services.AddEncryptionServices();
        services.AddBaseServices();
        var provider = services.BuildServiceProvider();

        _ecies = provider.GetRequiredService<IECIESEncryptionService>();
    }
}