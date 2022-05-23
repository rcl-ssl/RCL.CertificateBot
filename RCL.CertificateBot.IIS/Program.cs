using RCL.CertificateBot.IIS;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureLogging((_, logging) => logging.AddEventLog())
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration Configuration = hostContext.Configuration;

        services.AddRCLSDKService(options => Configuration.Bind("RCLSDK", options));
        services.AddCertificateBotService(options => Configuration.Bind("CertificateBot", options));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
