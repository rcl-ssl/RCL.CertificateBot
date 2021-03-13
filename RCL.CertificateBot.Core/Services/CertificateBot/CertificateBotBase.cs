using Microsoft.Extensions.Options;
using RCL.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RCL.CertificateBot.Core
{
    internal abstract class CertificateBotBase : ICertificateBotService
    {
        private readonly ICertificateService _certificateService;
        private readonly IFileService _fileService;
        protected readonly IOptions<CertificateBotOptions> _certificateBotOptions;
        private static readonly HttpClient _httpClient;

        protected const string _pfxCertificateFileName = "certificate.pfx";
        private const string _privateKeyFileName = "privateKey.key";
        private const string _primaryCertificateFileName = "primaryCertificate.crt";
        private const string _caBundleFileName = "caBundle.crt";
        private const string _fullChainCertificateFileName = "fullChainCertificate.crt";

        private List<CertificateResponse> certificateResponses = new List<CertificateResponse>();
        private List<string> lstCertNames = new List<string>();

        static CertificateBotBase()
        {
            _httpClient = new HttpClient();
        }

        public CertificateBotBase(ICertificateService certificateService,
            IOptions<CertificateBotOptions> certificateBotOptions,
            IFileService fileService)
        {
            _certificateService = certificateService;
            _certificateBotOptions = certificateBotOptions;
            _fileService = fileService;
        }

        public async Task<MessageResponse> SaveCertificatesInServerAndScheduleRenewalAsync()
        {
            MessageResponse messageResponse = new MessageResponse
            {
                message = string.Empty
            };

            List<CertificateResponse> certificatesList =
                await GetCertificateListAsync();

            if (certificatesList?.Count > 0)
            {
                foreach (var cert in certificatesList)
                {
                    if (!string.IsNullOrEmpty(cert?.remoteCreate))
                    {
                        string[] flaggedServers = TextHelper.GetStringArray(cert.remoteCreate, ',');

                        if (!flaggedServers.Contains(_certificateBotOptions.Value.serverIdentifier))
                        {
                            await SaveCertificateFileAndFlagServer(cert);
                        }
                    }
                    else
                    {
                        await SaveCertificateFileAndFlagServer(cert);
                    }
                }

                if (lstCertNames?.Count > 0)
                {
                    messageResponse.message = $"Saved the following certificate(s): {String.Join(",", lstCertNames)} in the server in the folder : {_certificateBotOptions.Value.saveCertificatePath}.";
                }
                else
                {
                    messageResponse.message = $"Did not find any certificates to save in the server.";
                }

                List<CertificateResponse> scheduledCertificateRenewals =
                    await RenewCertificatesAsync();

                if (scheduledCertificateRenewals?.Count > 0)
                {
                    List<string> lstCertToRenew = new List<string>();

                    foreach (var certToRenew in scheduledCertificateRenewals)
                    {
                        lstCertToRenew.Add(certToRenew.name);
                    }

                    messageResponse.message = $"{messageResponse.message} The following certificate(s) were scheduled for renewal : {String.Join(",", lstCertToRenew)}.";
                }
                else
                {
                    messageResponse.message = $"{messageResponse.message} There were no certificates found to be renewed.";
                }
            }
            else
            {
                messageResponse.message = $"There were no certificates found to save to the server.";
            }

            messageResponse.certificateResponses = certificateResponses;
            return messageResponse;
        }

        public abstract MessageResponse AddBindingsWithCertificatesToSite(List<CertificateResponse> certificateResponses);

        private async Task<List<CertificateResponse>> RenewCertificatesAsync()
        {
            try
            {
                List<CertificateResponse> certificateResponses = await
                    _certificateService.PostCertificateRenewalAsync(true);

                return certificateResponses;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not renew certificates, {ex.Message}");
            }
        }

        private async Task<List<CertificateResponse>> GetCertificateListAsync()
        {
            List<CertificateResponse> certificateResponses = new List<CertificateResponse>();

            try
            {
                List<CertificateResponse> allCerts = await
                   _certificateService.GetCertificatesListAsync();

                string[] includedCerts = _certificateBotOptions.Value.includeCertificates;

                if (!includedCerts.Contains("all"))
                {
                    if (allCerts?.Count > 0)
                    {
                        foreach (CertificateResponse cert in allCerts)
                        {
                            if (Array.Exists(includedCerts, s => s == cert.name))
                            {
                                certificateResponses.Add(cert);
                            }
                        }
                    }
                }
                else
                {
                    if (allCerts?.Count > 0)
                    {
                        certificateResponses = allCerts;
                    }
                }

                return certificateResponses;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get the certificate list from the subscription, {ex.Message}");
            }
        }

        private async Task SaveCertificateFileAndFlagServer(CertificateResponse cert)
        {
            string folderPath = FolderNameHelper.GetFolderPath(cert.name, _certificateBotOptions.Value.saveCertificatePath);

            await SaveFileAsync(_pfxCertificateFileName, folderPath, cert.pfxUri);
            await SaveFileAsync(_privateKeyFileName, folderPath, cert.privateKeyUri);
            await SaveFileAsync(_primaryCertificateFileName, folderPath, cert.certificateUri);
            await SaveFileAsync(_caBundleFileName, folderPath, cert.intermediateCertificateUri);
            await SaveFileAsync(_fullChainCertificateFileName, folderPath, cert.fullChainCertificateUri);

            CertificateResponse certUpdated = await FlagServerAsync(cert);

            lstCertNames.Add(certUpdated.name);
            certificateResponses.Add(certUpdated);
        }

        private async Task<CertificateResponse> FlagServerAsync(CertificateResponse certificateResponse)
        {
            try
            {
                certificateResponse.remoteCreateDate = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(certificateResponse?.remoteCreate))
                {
                    certificateResponse.remoteCreate = $"{certificateResponse.remoteCreate},{_certificateBotOptions.Value.serverIdentifier}";
                }
                else
                {
                    certificateResponse.remoteCreate = _certificateBotOptions.Value.serverIdentifier;
                }

                CertificateResponse updatedCert = await _certificateService
                    .PostCertificateAsync(certificateResponse);

                return updatedCert;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not flag server for locally saved certificate, {ex.Message}");
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
    }
}
