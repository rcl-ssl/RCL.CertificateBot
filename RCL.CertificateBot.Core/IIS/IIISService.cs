using Microsoft.Web.Administration;
using System.Security.Cryptography.X509Certificates;

namespace RCL.CertificateBot.Core
{
    public interface IIISService
    {
        Site GetIISSite(string siteName);

        void AddIISSiteBinding(string siteName,
            string bindingInformation,
            string certificateFilePath,
            string certificatePassword,
            StoreLocation storeLocation);

        void RemoveIISSiteBinding(string siteName, 
            string bindingInformation);
    }
}
