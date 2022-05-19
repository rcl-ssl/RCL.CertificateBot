namespace RCL.CertificateBot.Core
{
    public static class FolderNameHelper
    {
        public static string GetFolderPath(string certificateName, string directoryPath)
        {
            try
            {
                string folderName = certificateName;

                if (folderName.Contains(','))
                {
                    string firstPart = $"{folderName.Split(',')[0]}";
                    string secondPart = $"{folderName.Split(',')[1]}";

                    folderName = $"{firstPart}-san";

                    if (secondPart.StartsWith("www"))
                        folderName = $"{folderName}-www";

                    if (secondPart.StartsWith('*'))
                        folderName = $"{folderName}-wcard";
                }

                if (folderName.StartsWith("*."))
                {
                  folderName =  folderName.Replace("*.", "wcard-");
                }

                folderName = folderName.Replace(".", "-");

                string folderPath = Path.Combine(directoryPath, folderName);

                return folderPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get folder path, {ex.Message}");
            }
        }
    }
}
