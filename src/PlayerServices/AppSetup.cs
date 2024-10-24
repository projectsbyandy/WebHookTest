using PlayerServices.Endpoints;
using PlayerServices.Services;
using Serilog;

namespace PlayerServices;

internal static class AppSetup
{
    public static WebApplicationBuilder ConfigureHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSerilog(sp =>
            sp.WriteTo.Console());

        builder.Services.AddHttpClient("WebHookServer", client => client.BaseAddress = new Uri("http://localhost:5076"));
        builder.Services.AddSingleton<WebhookService>();
        builder.Services.AddSingleton<RegistrationService>();
        builder.Services.AddSingleton<DeactivationService>();
        
        builder.Services.AddLogging();

        return builder;

    }

    public static async Task ConfigureAppAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapPlayerManagementEndpoint();
        
        await RegisterWebHooksAsync(app);

        await app.RunAsync();
    }

    private static async Task RegisterWebHooksAsync(WebApplication app)
    {
        var webHookService = app.Services.GetService<WebhookService>();
        ArgumentNullException.ThrowIfNull(webHookService);
        await webHookService.RegisterEventTopicsAsync();
    }
}