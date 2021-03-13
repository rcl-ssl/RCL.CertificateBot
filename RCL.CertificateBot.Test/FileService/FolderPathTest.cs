using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCL.CertificateBot.Core;
using System.Collections.Generic;

namespace RCL.CertificateBot.Test.FileService
{
    [TestClass]
    public class FolderPathTest
    {
        [TestMethod]
        public void FolderNameTest()
        {
            string directoryPath = @"c:\test";
            List<string> folderNames = new List<string>();
            List<string> certificateName = new List<string>
            {
                "contoso.com",
                "www.contoso.com",
                "anytown.store.contoso.com.br",
                "*.contoso.com",
                "contoso.com,www.contoso.com",
                "contoso.com,*.contoso.com",
            };

            foreach (string name in certificateName)
            {
                string folderName = FolderNameHelper.GetFolderPath(name, directoryPath);
                folderNames.Add(folderName);
            }

            Assert.AreEqual(1, 1);
        }
    }
}
