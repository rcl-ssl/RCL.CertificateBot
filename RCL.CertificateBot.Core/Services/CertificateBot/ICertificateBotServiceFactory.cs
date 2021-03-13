namespace RCL.CertificateBot.Core
{
    public interface ICertificateBotServiceFactory
    {
        ICertificateBotService Create(CertificateBotServiceType certificateBotServiceType);
    }
}
