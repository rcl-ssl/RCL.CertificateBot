using RCL.CertificateBot.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CertificateBotExtension
    {
        public static IServiceCollection AddCertificateBotService(this IServiceCollection services,
            Action<CertificateBotOptions> setupAction)
        {
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<ICertificateBotService, CertificateBotService>();
            services.AddTransient<IIISService, IISService>();
            services.Configure(setupAction);

            return services;
        }
    }
}
