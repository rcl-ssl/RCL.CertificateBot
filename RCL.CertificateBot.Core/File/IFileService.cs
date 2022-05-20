namespace RCL.CertificateBot.Core
{
    public interface IFileService
    {
        void CreateDirectory(string path);
        int GetNumberFilesInDirectory(string path);
        void SaveFile(string fileName, string path, Stream contentStream);
        void WriteTextToFile(string fileName, string path, string text);
    }
}
