using System.Net.Http.Json;
using PlayerServices;
using WebHookServer;

namespace IntegrationTests.Endpoints;

public class PlayerManagementTests : IClassFixture<ApplicationFactoryFixture<WebHookServiceProgram>>, IClassFixture<ApplicationFactoryFixture<PlayerServicesProgram>>
{
    private readonly ApplicationFactoryFixture<WebHookServiceProgram> _webHookServiceFactory;
    private readonly ApplicationFactoryFixture<PlayerServicesProgram> _playerServiceFactory;

    public PlayerManagementTests(ApplicationFactoryFixture<WebHookServiceProgram> webHookServiceFactory, ApplicationFactoryFixture<PlayerServicesProgram> playerServiceFactory)
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
        var response = await webHookServiceClient.PostAsJsonAsync("/Publish", new { topic = "event.playerRegistration", message = new { user = "Andrew", userType = "Slot Machine"} });
        response.EnsureSuccessStatusCode();
        
        // Assert
    }
};