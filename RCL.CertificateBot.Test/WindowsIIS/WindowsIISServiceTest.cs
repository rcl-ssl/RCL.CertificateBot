using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Web.Administration;
using RCL.CertificateBot.Core;
using System;
using System.Security.Cryptography.X509Certificates;

namespace RCL.CertificateBot.Test.IIS
{
    [TestClass]
    public class WindowsIISServiceTest
    {
        private const string _siteName = "Home";
        private const string _bindingInformation = "*:443:shopeneur.com";
        private const string _certificateFilePath = @"C:\test\crt\certificate.pfx";
        private const string _certificatePassword = "pwd1234";
        private const StoreLocation _storeLocation = StoreLocation.LocalMachine;

        private readonly IWindowsIISService _windowsIISService;
      
        public WindowsIISServiceTest()
        {
            _windowsIISService = (IWindowsIISService)DependencyResolver
                .ServiceProvider().GetService(typeof(IWindowsIISService));
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
                _windowsIISService.AddIISSiteBinding(_siteName, _bindingInformation,
                    _certificateFilePath, _certificatePassword, _storeLocation);
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
                _windowsIISService.RemoveIISSiteBinding(_siteName, _bindingInformation);
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
                Site site = _windowsIISService.GetIISSite("Home");
                return site;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
