using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using ILogger = Serilog.ILogger;

namespace IntegrationTests;

public class ApplicationFactoryFixture<TApplication> : WebApplicationFactory<TApplication> where TApplication : class
{
    public string HostUrl { get; set; } = string.Empty;
    public Mock<ILogger> MockLogger { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls(HostUrl);
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var dummyHost = builder.Build();

        var host = builder
            .ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel())
            .ConfigureServices(services => services.AddSingleton(MockLogger.Object))
            .Build();
        
        host.Start();

        dummyHost.Start();
        
        return dummyHost;
    }
}