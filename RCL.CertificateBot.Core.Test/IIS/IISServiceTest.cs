using Microsoft.Web.Administration;
using System.Security.Cryptography.X509Certificates;

namespace RCL.CertificateBot.Core.Test
{
    [TestClass]
    public class IISServiceTest
    {
        private const string _siteName = "Home";
        private const string _bindingInformation = "*:443:shopeneur.com";
        private const string _certificateFilePath = @"C:\test\crt\certificate.pfx";
        private const string _certificatePassword = "pwd1234";
        private const StoreLocation _storeLocation = StoreLocation.LocalMachine;

        private readonly IIISService _IISService;
      
        public IISServiceTest()
        {
            _IISService = (IIISService)DependencyResolver
                .ServiceProvider().GetService(typeof(IIISService));
        }

        [TestMethod]
        public void GetIISSiteTest()
        {
            try
            {
                Site site = GetIISSite();
                Assert.IsNotNull(site);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.AreEqual(1, 0);
            }
        }

        [TestMethod]
        public void AddIISSiteBinding()
        {
            try
            {
                _IISService.AddIISSiteBinding(_siteName, _bindingInformation,
                    _certificateFilePath, _certificatePassword, _storeLocation);
                Assert.AreEqual(1, 1);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.AreEqual(1, 0);
            }
        }

        [TestMethod]
        public void RemoveIISSiteBinding()
        {
            try
            {
                _IISService.RemoveIISSiteBinding(_siteName, _bindingInformation);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Assert.AreEqual(1, 0);
            }
        }

        private Site GetIISSite()
        {
            try
            {
                Site site = _IISService.GetIISSite("Home");
                return site;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
