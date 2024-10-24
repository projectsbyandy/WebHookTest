using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests;

public class ApplicationFactoryFixture<TApplication> : WebApplicationFactory<TApplication> where TApplication : class
{
    public string HostUrl { get; set; } = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls(HostUrl);
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var dummyHost = builder.Build();
        
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
        var host = builder.Build();
        host.Start();

        var server = host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();
        
        ArgumentNullException.ThrowIfNull(addresses);
        ClientOptions.BaseAddress = addresses.Addresses  
            .Select(x => new Uri(x))  
            .Last();  
        
        dummyHost.Start();
        return dummyHost;
    }
}