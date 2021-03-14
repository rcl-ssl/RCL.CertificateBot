using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RCL.CertificateBot.Core;

namespace RCL.CertificateBot.WindowsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration Configuration = hostContext.Configuration;

                    services.AddAuthTokenService(options => Configuration.Bind("Auth", options));
                    services.AddRCLSDK(options => Configuration.Bind("RCLSDK", options));
                    services.AddCertificateBot(options => Configuration.Bind("CertificateBot", options));

                    services.AddHostedService<Worker>();
                });
    }
}
