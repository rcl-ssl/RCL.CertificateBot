using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCL.CertificateBot.Core;
using System;
using System.IO;

namespace RCL.CertificateBot.Test.FileService
{
    [TestClass]
    public class FileServiceTest
    {
        private readonly IFileService _fileService;
        private const string sourcePath = @"c:\test\helloWorld.txt";
        private const string path = @"c:\test\testFolder";
        private const string fileName = "helloWorld.txt";


        public FileServiceTest()
        {
            _fileService = (IFileService)DependencyResolver
                .ServiceProvider().GetService(typeof(IFileService));
        }

        [TestMethod]
        public void SaveFileTest()
        {
            try
            {
                using (Stream stream = new FileStream(sourcePath,FileMode.Open))
                {
                    _fileService.SaveFile(fileName, path, stream);
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public void WriteTextToFile()
        {
            try
            {
                string text = "Hello world, again";
                _fileService.WriteTextToFile(fileName, path, text);
            }
            catch (Exception ex)
            {
               string err =  ex.Message;
                Assert.Fail();
            }
        }
    }
}

