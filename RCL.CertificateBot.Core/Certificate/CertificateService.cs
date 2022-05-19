using RCL.SDK;

namespace RCL.CertificateBot.Core
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRequestService _certificateRequestService;

        public CertificateService(ICertificateRequestService certificateRequestService)
        {
            _certificateRequestService = certificateRequestService;
        }

        public async Task<Certificate> GetCertificateAsync(Certificate certificate)
        {
            Certificate cert = new Certificate();

            try
            {
                cert = await _certificateRequestService.GetCertificateAsync(certificate);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cert;
        }

        public async Task<List<Certificate>> GetCertificatesToRenewAsync()
        {
            List<Certificate> certs = new List<Certificate>();

            try
            {
                certs = await _certificateRequestService.GetCertificatesToRenewAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return certs;
        }

        public async Task GetTestAsync()
        {
            try
            {
                await _certificateRequestService.GetTestAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RenewCertificateAsync(Certificate certificate)
        {
            try
            {
                await _certificateRequestService.RenewCertificateAsync(certificate);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
