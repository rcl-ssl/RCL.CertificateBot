#nullable disable

using Microsoft.Web.Administration;
using System.Security.Cryptography.X509Certificates;

namespace RCL.CertificateBot.Core
{
    internal class IISService : IIISService
    {
        public Site GetIISSite(string siteName)
        {
            try
            {
                using (ServerManager manager = new ServerManager())
                {
                    Site site = manager.Sites.Where(q => q.Name == siteName).FirstOrDefault();

                    if (site != null)
                    {
                        return site;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get site : {siteName} from IIS, {ex.Message}");
            }

            return null;
        }

        public void AddIISSiteBinding(string siteName, 
            string bindingInformation, 
            string certificateFilePath,
            string certificatePassword,
            StoreLocation storeLocation)
        {
            try
            {
                using (ServerManager manager = new ServerManager())
                {
                    X509Certificate2 certificate = new X509Certificate2(certificateFilePath, 
                        certificatePassword, X509KeyStorageFlags.MachineKeySet);

                    byte[] certHash = certificate.GetCertHash();
                    string storeName = string.Empty;

                    Site site = manager.Sites.Where(q => q.Name == siteName).FirstOrDefault();

                    using (X509Store store = new X509Store(StoreName.TrustedPeople, storeLocation))
                    {
                        storeName = store.Name;

                        store.Open(OpenFlags.ReadWrite);
                        store.Add(certificate);
                    }

                    site.Bindings.Add(bindingInformation, certHash, storeName);

                    manager.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not add binding: {bindingInformation} to IIS site: {siteName}, {ex.Message}");
            }
        }

        public void RemoveIISSiteBinding(string siteName, 
            string bindingInformation)
        {
            try
            {
                using (ServerManager manager = new ServerManager())
                {
                    Site site = manager.Sites.Where(q => q.Name == siteName).FirstOrDefault();

                    if (site != null)
                    {
                        if (site?.Bindings?.Count > 0)
                        {
                            var binding = site.Bindings
                                .Where(w => w.BindingInformation == bindingInformation && w.Protocol == "https")
                                .FirstOrDefault();

                            if (binding != null)
                            {
                                site.Bindings.Remove(binding);
                                manager.CommitChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not remove binding {bindingInformation} from IIS site : {siteName}, {ex.Message}");
            }
        }
    }
}
