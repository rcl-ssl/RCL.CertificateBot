using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace RCL.CertificateBot.Core
{
    public static class LetsEncryptServiceCollection
    {
        public static IServiceCollection AddCertificateBot(this IServiceCollection services,
            Action<CertificateBotOptions> setupAction)
        {
            services.TryAddTransient<IFileService, FileService>();
            services.TryAddTransient<IWindowsIISService, WindowsIISService>();
            services.TryAddTransient<ICertificateBotServiceFactory, CertificateBotServiceFactory>();

            services.Configure(setupAction);

            return services;
        }
    }
}
