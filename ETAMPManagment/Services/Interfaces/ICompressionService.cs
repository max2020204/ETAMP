namespace ETAMPManagment.Services.Interfaces
{
    public interface ICompressionService
    {
        string CompressString(string data);

        string DecompressString(string base64CompressedData);
    }
}