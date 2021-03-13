using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCL.CertificateBot.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RCL.CertificateBot.WindowsService
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
                .Create(CertificateBotServiceType.WindowsService);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("CertificateBot running at: {time}", DateTimeOffset.Now);

                string message = string.Empty;

                try
                {
                    MessageResponse messageResponse = await certificateBot
                        .SaveCertificatesInServerAndScheduleRenewalAsync();

                    message = messageResponse.message;

                    List<BindingInformation> bindings = _certificateBotOptions.Value.bindings;
                    if (bindings?.Count > 0)
                    {
                        MessageResponse messageResponseIIS = certificateBot
                            .AddBindingsWithCertificatesToSite(messageResponse.certificateResponses);

                        message = $"{message} {messageResponseIIS.message}";
                    }

                    _logger.LogInformation($"{DateTime.Now} {message}");
                    
                    _fileService.WriteTextToFile("log.txt",
                        _certificateBotOptions.Value.saveCertificatePath, 
                        $"{DateTime.Now} {message}");

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
