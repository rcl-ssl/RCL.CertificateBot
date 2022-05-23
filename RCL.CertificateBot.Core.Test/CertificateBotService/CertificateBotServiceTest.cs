namespace RCL.CertificateBot.Core.Test.CertificateBotService
{
    [TestClass]
    public class CertificateBotServiceTest
    {
        private readonly ICertificateBotService _certificateBotService;

        public CertificateBotServiceTest()
        {
            _certificateBotService = (ICertificateBotService)DependencyResolver
                   .ServiceProvider().GetService(typeof(ICertificateBotService));
        }

        [TestMethod]
        public async Task InstallAndRenewCertificateTest()
        {
            try
            {
                await _certificateBotService.InstallAndRenewCertificateAsync();
                Assert.AreEqual(1, 1);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task InstallAndRenewCertificateInIISTest()
        {
            try
            {
                await _certificateBotService.InstallAndRenewCertificateInIISAsync();
                Assert.AreEqual(1, 1);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }
    }
}
