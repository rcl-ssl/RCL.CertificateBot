using Microsoft.Extensions.Options;
using RCL.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace RCL.CertificateBot.Core
{
    internal class CertificateBotWindowsService : CertificateBotBase, ICertificateBotService
    {
        private readonly IWindowsIISService _windowsIISService;

        public CertificateBotWindowsService(ICertificateService certificateService,
            IOptions<CertificateBotOptions> certificateBotOptions,
            IFileService fileService,
            IWindowsIISService windowsIISService)
            : base(certificateService, certificateBotOptions, fileService)
        {
            _windowsIISService = windowsIISService;
        }

        public override MessageResponse AddBindingsWithCertificatesToSite(List<CertificateResponse> certificateResponses)
        {
            MessageResponse messageResponse = new MessageResponse
            {
                message = "Did not add any updated bindings to IIS sites."
            };

            List<CertificateResponse> certResponses = new List<CertificateResponse>();

            if (certificateResponses?.Count > 0)
            {
                List<BindingInformation> bindings = _certificateBotOptions.Value.bindings;

                foreach (CertificateResponse certificateResponse in certificateResponses)
                {
                    if (bindings?.Count > 0)
                    {
                        List<string> lstBinding = new List<string>();

                        foreach (BindingInformation binding in bindings)
                        {
                            if (binding.certificateName == certificateResponse.name)
                            {
                                // Remove old site binding
                                _windowsIISService.RemoveIISSiteBinding(binding.siteName, binding.GetBindingInformation());

                                string folderPath = FolderNameHelper.GetFolderPath(binding.certificateName, _certificateBotOptions.Value.saveCertificatePath);
                                string certFilePath = Path.Combine(folderPath, _pfxCertificateFileName);

                                // Add new site binding
                                _windowsIISService.AddIISSiteBinding(binding.siteName, binding.GetBindingInformation(),
                                 certFilePath, certificateResponse.pfxpwd, StoreLocation.LocalMachine);

                                lstBinding.Add($"{binding.GetBindingInformation()} >> site:{binding.siteName} >> certificate:{binding.certificateName}");
                                certResponses.Add(certificateResponse);
                            }
                        }

                        if (lstBinding?.Count > 0)
                        {
                            messageResponse.message = $"Added the renewed bindings : {String.Join(",", lstBinding)} in the IIS web server.";
                        }
                    }
                }
            }

            messageResponse.certificateResponses = certificateResponses;
            return messageResponse;
        }
    }
}
