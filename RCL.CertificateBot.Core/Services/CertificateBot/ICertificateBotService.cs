using RCL.SDK;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCL.CertificateBot.Core
{
    public interface ICertificateBotService
    {
        Task<MessageResponse> SaveCertificatesInServerAndScheduleRenewalAsync();
        MessageResponse AddBindingsWithCertificatesToSite(List<CertificateResponse> certificateResponses);
    }
}
