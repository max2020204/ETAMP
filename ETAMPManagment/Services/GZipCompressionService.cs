using ETAMPManagment.Services.Interfaces;
using System.IO.Compression;
using System.Text;

namespace ETAMPManagment.Services
{
    public class GZipCompressionService : ICompressionService
    {
        public virtual string CompressString(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        public string DecompressString(string base64CompressedData)
        {
            byte[] compressedBytes = Convert.FromBase64String(base64CompressedData);

            using (var inputStream = new MemoryStream(compressedBytes))
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gzipStream.CopyTo(outputStream);
                byte[] decompressedBytes = outputStream.ToArray();
                return Encoding.UTF8.GetString(decompressedBytes);
            }
        }
    }
}