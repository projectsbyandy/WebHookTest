using PlayerServices;

var app = AppSetup.ConfigureHostBuilder(args).Build();
await AppSetup.ConfigureAppAsync(app);

namespace PlayerServices
{
    public partial class Program;
}
