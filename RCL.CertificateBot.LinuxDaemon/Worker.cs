using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCL.CertificateBot.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RCL.CertificateBot.LinuxDaemon
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICertificateBotServiceFactory _certificateBotFactory;
        private readonly IFileService _fileService;
        private readonly IOptions<CertificateBotOptions> _certificateBotOptions;

        public Worker(ILogger<Worker> logger,
            ICertificateBotServiceFactory certificateBotFactory,
            IFileService fileService,
            IOptions<CertificateBotOptions> certificateBotOptions)
        {
            _logger = logger;
            _certificateBotFactory = certificateBotFactory;
            _fileService = fileService;
            _certificateBotOptions = certificateBotOptions;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ICertificateBotService certificateBot = _certificateBotFactory
                .Create(CertificateBotServiceType.LinuxDaemon);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("CertificateBot running at: {time}", DateTimeOffset.Now);

                try
                {
                    MessageResponse messageResponse = await certificateBot
                        .SaveCertificatesInServerAndScheduleRenewalAsync();

                    _logger.LogInformation($"{DateTime.Now} {messageResponse.message}");
                    _fileService.WriteTextToFile("log.txt",
                        _certificateBotOptions.Value.saveCertificatePath,
                        $"{DateTime.Now} {messageResponse.message}");

                }
                catch (Exception ex)
                {
                    _logger.LogError($"{DateTime.Now} {ex.Message}");
                }

                await Task.Delay(4 * 24 * 60 * 60 * 1000, stoppingToken);
            }
        }
    }
}
