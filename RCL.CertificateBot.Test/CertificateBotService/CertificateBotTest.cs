using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCL.CertificateBot.Core;
using RCL.SDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCL.CertificateBot.Test
{
    [TestClass]
    public class CertificateBotTest
    {
        private readonly ICertificateBotServiceFactory _certificateBotFactory;
        private readonly ICertificateBotService _certificateBot;

        public CertificateBotTest()
        {
            _certificateBotFactory = (ICertificateBotServiceFactory)DependencyResolver.
                ServiceProvider().GetService(typeof(ICertificateBotServiceFactory));

            _certificateBot = _certificateBotFactory.Create(CertificateBotServiceType.WindowsService);
        }

        [TestMethod]
        public async Task RenewAndSaveCertificateTest()
        {
            try
            {
                MessageResponse messageResponse = await _certificateBot.SaveCertificatesInServerAndScheduleRenewalAsync(); 

                Assert.IsNotNull(messageResponse);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Assert.AreEqual(1,0);
            }
        }

        [TestMethod]
        public void AddBindingsWithCertificatesToSiteTest()
        {
            CertificateResponse certificateResponse = new CertificateResponse
            {
                name = "shopeneur.com",
                pfxpwd = "pwd1234"
            };

            List<CertificateResponse> certificateResponses = new List<CertificateResponse>();
            certificateResponses.Add(certificateResponse);

            try
            {
                MessageResponse messageResponse = _certificateBot.AddBindingsWithCertificatesToSite(certificateResponses);
                Assert.IsNotNull(messageResponse);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Assert.AreEqual(1, 0);
            }
        }
    }
}
