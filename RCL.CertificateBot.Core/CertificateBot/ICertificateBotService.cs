namespace RCL.CertificateBot.Core
{
    public interface ICertificateBotService
    {
        Task<string> InstallAndRenewCertificateAsync();
    }
}
