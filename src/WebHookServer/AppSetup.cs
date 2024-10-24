using Serilog;
using WebHookServer.Endpoints;
using WebHookServer.Services;

namespace WebHookServer;

internal static class AppSetup
{
    public static WebApplicationBuilder ConfigureHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<WebHookService>();
        builder.Services.AddSerilog(sp =>
            sp.WriteTo.Console());

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

        app.MapTopicManagementEndpoints();

        await app.RunAsync();
    }
}