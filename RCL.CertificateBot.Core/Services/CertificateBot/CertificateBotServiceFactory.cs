using Microsoft.Extensions.Options;
using RCL.SDK;

namespace RCL.CertificateBot.Core
{
    internal class CertificateBotServiceFactory : ICertificateBotServiceFactory
    {
        private readonly ICertificateService _certificateService;
        private readonly IFileService _fileService;
        private readonly IOptions<CertificateBotOptions> _certificateBotOptions;
        private readonly IWindowsIISService _windowsIISService;

        public CertificateBotServiceFactory(ICertificateService certificateService,
            IOptions<CertificateBotOptions> certificateBotOptions,
            IFileService fileService,
            IWindowsIISService windowsIISService)
        {
            _certificateService = certificateService;
            _certificateBotOptions = certificateBotOptions;
            _fileService = fileService;
            _windowsIISService = windowsIISService;
        }

        public ICertificateBotService Create(CertificateBotServiceType certificateBotServiceType)
        {
            if(certificateBotServiceType == CertificateBotServiceType.LinuxDaemon)
            {
                return new CertificateBotLinuxDaemon(_certificateService,
                    _certificateBotOptions, _fileService);
            }
            else if(certificateBotServiceType == CertificateBotServiceType.WindowsService)
            {
                return new CertificateBotWindowsService(_certificateService,
                    _certificateBotOptions, _fileService, _windowsIISService);
            }
            else
            {
                return new CertificateBotLinuxDaemon(_certificateService,
                    _certificateBotOptions, _fileService);
            }
        }
    }
}
