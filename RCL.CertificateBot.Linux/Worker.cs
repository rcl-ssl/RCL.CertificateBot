using RCL.CertificateBot.Core;

namespace RCL.CertificateBot.Linux
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICertificateBotService _certificateBotService;

        public Worker(ILogger<Worker> logger,
            ICertificateBotService certificateBotService)
        {
            _logger = logger;
            _certificateBotService = certificateBotService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("CertificateBot running at: {time}", DateTime.Now);

                try
                {
                    string message = await _certificateBotService.InstallAndRenewCertificateAsync();

                    _logger.LogInformation(message);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"{DateTime.Now} {ex.Message}");
                }

                await Task.Delay(7 * 24 * 60 * 60 * 1000, stoppingToken);
            }
        }
    }
}