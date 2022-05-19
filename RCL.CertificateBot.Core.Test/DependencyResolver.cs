using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RCL.CertificateBot.Core.Test
{
    public static class DependencyResolver
    {
        public static ServiceProvider ServiceProvider()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddUserSecrets<TestProject>();
            IConfiguration configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            services.AddRCLSDKService(options => configuration.Bind("RCLSDK", options));
            services.AddCertificateBotService(options => configuration.Bind("CertificateBot",options));

            return services.BuildServiceProvider();
        }
    }

    public class TestProject
    {
    }
}
