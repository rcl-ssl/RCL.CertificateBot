namespace RCL.CertificateBot.Core
{
    public class FileService : IFileService
    {
        public void CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not create a directory, {ex.Message}");
            }
        }

        public void SaveFile(string fileName, string path, Stream contentStream)
        {
            try
            {
                CreateDirectory(path);

                string fullPath = Path.Combine(path, fileName);

                using (FileStream outputFileStream = new FileStream(fullPath, FileMode.Create))
                {
                    contentStream.CopyTo(outputFileStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not save a file, {ex.Message}");
            }
        }

        public void WriteTextToFile(string fileName, string path, string text)
        {
            try
            {
                using (var stream = GenerateStreamFromString(text))
                {
                    SaveFile(fileName, path, stream);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not write text to file, : {ex.Message}");
            }
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
