using RCL.SDK;

namespace RCL.CertificateBot.Core
{
    public interface ICertificateService
    {
        Task GetTestAsync();
        Task<Certificate> GetCertificateAsync(Certificate certificate);
        Task<List<Certificate>> GetCertificatesToRenewAsync();
        Task RenewCertificateAsync(Certificate certificate);
    }
}
