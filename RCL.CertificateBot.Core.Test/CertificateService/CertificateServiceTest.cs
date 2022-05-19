using RCL.SDK;

namespace RCL.CertificateBot.Core.Test
{
    [TestClass]
    public class CertificateServiceTest
    {
        private readonly ICertificateService _certificateService;

        public CertificateServiceTest()
        {
            _certificateService = (ICertificateService)DependencyResolver
                 .ServiceProvider().GetService(typeof(ICertificateService));
        }

        [TestMethod]
        public async Task GetTest()
        {
            try
            {
                await _certificateService.GetTestAsync();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task GetCertificateTest()
        {
            try
            {
                Certificate certificate = new Certificate
                {
                    certificateName = "shopeneur.com,*.shopeneur.com"
                };

                Certificate _certificate = await _certificateService.GetCertificateAsync(certificate);
                Assert.AreNotEqual(string.Empty, _certificate?.certificateName ?? string.Empty);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task GetCertificatesToRenewTest()
        {
            try
            {
                List<Certificate> certificates = await _certificateService.GetCertificatesToRenewAsync();
                Assert.AreNotEqual(0, certificates.Count);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task RenewCertificateTest()
        {
            try
            {
                Certificate certificate = new Certificate
                {
                    certificateName = "shopeneur.com"
                };

                await _certificateService.RenewCertificateAsync(certificate);
                Assert.AreEqual(1,1);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.Fail();
            }
        }
    }
}
