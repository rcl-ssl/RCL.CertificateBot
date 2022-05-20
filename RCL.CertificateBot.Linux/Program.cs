using RCL.CertificateBot.Linux;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration Configuration = hostContext.Configuration;

        services.AddRCLSDKService(options => Configuration.Bind("RCLSDK", options));
        services.AddCertificateBotService(options => Configuration.Bind("CertificateBot", options));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
