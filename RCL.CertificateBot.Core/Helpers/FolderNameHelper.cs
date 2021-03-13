using System;
using System.IO;

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
                    string fp = $"{folderName.Split(',')[0]}";
                    string sp = $"{folderName.Split(',')[1]}";

                    folderName = $"{fp}-san";

                    if (sp.StartsWith("www"))
                        folderName = $"{folderName}-www";

                    if (sp.StartsWith('*'))
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
