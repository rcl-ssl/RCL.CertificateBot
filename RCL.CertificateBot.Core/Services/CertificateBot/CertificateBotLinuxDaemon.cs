using Microsoft.Extensions.Options;
using RCL.SDK;
using System.Collections.Generic;

namespace RCL.CertificateBot.Core
{
    internal class CertificateBotLinuxDaemon : CertificateBotBase,ICertificateBotService
    {
        public CertificateBotLinuxDaemon(ICertificateService certificateService,
            IOptions<CertificateBotOptions> certificateBotOptions,
            IFileService fileService)
            :base(certificateService,certificateBotOptions,fileService)
        {
        }

        public override MessageResponse AddBindingsWithCertificatesToSite(List<CertificateResponse> certificateResponses)
        {
            return new MessageResponse
            {
                message = "Not required"
            };
        }
    }
}
