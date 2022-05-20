#nullable disable

using Microsoft.Extensions.Options;
using RCL.SDK;

namespace RCL.CertificateBot.Core
{
    public class CertificateBotService : ICertificateBotService
    {
        private readonly IOptions<CertificateBotOptions> _options;
        private readonly ICertificateService _certificateService;
        private readonly IFileService _fileService;
        private static readonly HttpClient _httpClient;

        private const string _pfxCertificateFileName = "certificate.pfx";
        private const string _privateKeyFileName = "privateKey.key";
        private const string _primaryCertificateFileName = "primaryCertificate.crt";
        private const string _caBundleFileName = "caBundle.crt";
        private const string _fullChainCertificateFileName = "fullChainCertificate.crt";

        static CertificateBotService()
        {
            _httpClient = new HttpClient();
        }

        public CertificateBotService(IOptions<CertificateBotOptions> options,
            ICertificateService certificateService,
            IFileService fileService)
        {
            _options = options;
            _certificateService = certificateService;
            _fileService = fileService;
        }

        public async Task<string> InstallAndRenewCertificateAsync()
        {
            string message = $"Message received at : {DateTime.Now}. ";

            try
            {
                List<Certificate> certificates = await GetIncludedCertificatesAsync();

                if (certificates?.Count > 0)
                {
                    message = $"Found {certificates.Count} certificate(s) to save locally. ";

                    foreach (Certificate cert in certificates)
                    {
                        await SaveCertificateToFileAsync(cert);

                        message = $"{message} Successfully saved : {cert.certificateName}. ";
                    }
                }
                else
                {
                    message = "Did not find any certificates to save locally. ";
                }

                List<Certificate> certificatesToRenew = await GetCertificatesToRenewAsync();

                if (certificatesToRenew?.Count > 0)
                {
                    message = $"{message} Found {certificatesToRenew?.Count} certificate(s) to renew. ";

                    foreach(Certificate cert in certificatesToRenew)
                    {
                        await _certificateService.RenewCertificateAsync(cert);

                        message = $"{message} Scheduled {cert.certificateName} for renewal. ";
                    }
                }
                else
                {
                    message = $"{message} Did not find any certificates to renew. ";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return message;
        }

        private async Task<List<Certificate>> GetIncludedCertificatesAsync()
        {
            List<Certificate> certificates = new List<Certificate>();

            try
            {
                List<string> certificateNames = _options.Value.IncludeCertificates;

                if (certificateNames?.Count > 0)
                {
                    foreach (string certificateName in certificateNames)
                    {
                        Certificate certificateToRetrieve = new Certificate
                        {
                            certificateName = certificateName
                        };

                        Certificate certificateRetrieved = await _certificateService.GetCertificateAsync(certificateToRetrieve);

                        if (!string.IsNullOrEmpty(certificateRetrieved?.certificateName ?? string.Empty))
                        {
                            if (certificateRetrieved.renewal.ToLower() == "automatic")
                            {
                                certificates.Add(certificateRetrieved);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get Included Certificates. {ex.Message}");
            }

            return certificates;
        }

        private async Task SaveCertificateToFileAsync(Certificate certificate)
        {
            try
            {
                string folderPath = FolderNameHelper.GetFolderPath(certificate.certificateName, _options.Value.SaveCertificatePath);

                int files = _fileService.GetNumberFilesInDirectory(folderPath);
                int age = (DateTime.Now.Date - certificate.issueDate.Date).Days;

                if (files < 1 || age < 9)
                {
                    await SaveFileAsync(_pfxCertificateFileName, folderPath, certificate.certificateDownloadUrl.pfxUrl);
                    await SaveFileAsync(_privateKeyFileName, folderPath, certificate.certificateDownloadUrl.keyUrl);
                    await SaveFileAsync(_primaryCertificateFileName, folderPath, certificate.certificateDownloadUrl.certCrtUrl);
                    await SaveFileAsync(_caBundleFileName, folderPath, certificate.certificateDownloadUrl.cabundleCrtUrl);
                    await SaveFileAsync(_fullChainCertificateFileName, folderPath, certificate.certificateDownloadUrl.fullchainCrtUrl);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SaveFileAsync(string fileName, string folderPath, string fileUri)
        {
            try
            {
                var response = await _httpClient.GetAsync(fileUri);

                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {
                    _fileService.SaveFile(fileName, folderPath, contentStream);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Could not save file locally, {ex.Message}");
            }
        }

        private async Task<List<Certificate>> GetCertificatesToRenewAsync()
        {
            List<Certificate> certificates = new List<Certificate>();

            try
            {
                List<Certificate> certsToRenew = await _certificateService.GetCertificatesToRenewAsync();

                if (certsToRenew?.Count > 0)
                {
                    List<string> certNames = _options.Value.IncludeCertificates;

                    if (certNames?.Count > 0)
                    {
                        foreach (Certificate cert in certsToRenew)
                        {
                            if(certNames.Contains(cert.certificateName))
                            {
                                certificates.Add(cert);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return certificates;
        }
    }
}
