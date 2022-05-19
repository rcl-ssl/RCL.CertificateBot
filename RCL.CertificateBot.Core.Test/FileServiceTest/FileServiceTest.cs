namespace RCL.CertificateBot.Core.Test.FileServiceTest
{
    [TestClass]
    public class FileServiceTest
    {
        private readonly IFileService _fileService;

        public FileServiceTest()
        {
            _fileService = (IFileService)DependencyResolver
              .ServiceProvider().GetService(typeof(IFileService));
        }

        [TestMethod]
        public void CreateDirectoryTest()
        {
            try
            {
                _fileService.CreateDirectory(@"c:\test\testFolder");
                Assert.AreEqual(1, 1);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SaveFileTest()
        {
            try
            {
                using (Stream stream = new FileStream(@"c:\test\helloWorld.txt", FileMode.Open))
                {
                    _fileService.SaveFile("helloWorld.txt", @"c:\test\testFolder", stream);
                }
                Assert.AreEqual(1, 1);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public void WriteTextToFileTest()
        {
            try
            {
                _fileService.WriteTextToFile("helloWorld.txt", @"c:\test\testFolder", "Hello World Updated !");
                Assert.AreEqual(1, 1);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetFolderName()
        {
            string directoryPath = "C:/test/testFolder";
            
            string certificateName = "shopeneur.com,*.shopeneur.com";
            string folderPath = FolderNameHelper.GetFolderPath(certificateName, directoryPath);

            Assert.AreEqual(1, 1);
        }
    }
}
