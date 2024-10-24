using WebHookServer;

var app = AppSetup.ConfigureHostBuilder(args).Build();
await AppSetup.ConfigureAppAsync(app);

namespace WebHookServer
{
    public partial class Program;
}