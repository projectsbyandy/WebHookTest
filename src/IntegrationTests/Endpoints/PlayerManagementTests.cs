using System.Net.Http.Json;
using WebHookServerApp = WebHookServer.Program;
using PlayerServicesApp = PlayerServices.Program;

namespace IntegrationTests.Endpoints;

public class PlayerManagementTests : IClassFixture<ApplicationFactoryFixture<WebHookServerApp>>, IClassFixture<ApplicationFactoryFixture<PlayerServicesApp>>
{
    private readonly ApplicationFactoryFixture<WebHookServerApp> _webHookServiceFactory;
    private readonly ApplicationFactoryFixture<PlayerServicesApp> _playerServiceFactory;

    public PlayerManagementTests(ApplicationFactoryFixture<WebHookServerApp> webHookServiceFactory, ApplicationFactoryFixture<PlayerServicesApp> playerServiceFactory)
    {
        _webHookServiceFactory = webHookServiceFactory;
        _playerServiceFactory = playerServiceFactory;
    }

    [Fact]
    public async Task Verify_a_Valid_Player_Registration_Event_Is_Processed()
    {
        // Arrange
        _webHookServiceFactory.HostUrl = "http://localhost:5076";
        _playerServiceFactory.HostUrl = "http://localhost:5054";
        
        var webHookServiceClient = _webHookServiceFactory.CreateClient();
        _playerServiceFactory.CreateClient();
        
        // Act
        var response = await webHookServiceClient.PostAsJsonAsync("/api/topic/publish", new { topic = "event.playerRegistration", message = new { user = "Andrew", userType = "Slot Machine"} });
        response.EnsureSuccessStatusCode();
        
        // Assert
    }
};