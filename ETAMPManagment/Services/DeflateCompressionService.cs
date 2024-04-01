using ETAMPManagment.Services.Interfaces;
using System.IO.Compression;
using System.Text;

namespace ETAMPManagment.Services
{
    public class DeflateCompressionService : ICompressionService
    {
        public virtual string CompressString(string data)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(data);

            using var output = new MemoryStream();
            using DeflateStream compressor = new(output, CompressionMode.Compress);
            compressor.Write(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(output.ToArray());
        }

        public virtual string DecompressString(string base64CompressedData)
        {
            byte[] inputBytes = Convert.FromBase64String(base64CompressedData);

            using var inputStream = new MemoryStream(inputBytes);
            using var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress);
            using var outputStream = new MemoryStream();
            decompressor.CopyTo(outputStream);
            byte[] outputBytes = outputStream.ToArray();

            return Encoding.UTF8.GetString(outputBytes);
        }
    }
}