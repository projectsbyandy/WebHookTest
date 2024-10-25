using System.Net.Http.Json;
using Moq;
using WebHookServerApp = WebHookServer.Program;
using PlayerServicesApp = PlayerServices.Program;

namespace IntegrationTests.Endpoints;

public class PlayerManagementTests(
    ApplicationFactoryFixture<WebHookServerApp> webHookServiceFactory,
    ApplicationFactoryFixture<PlayerServicesApp> playerServiceFactory)
    : IClassFixture<ApplicationFactoryFixture<WebHookServerApp>>,
        IClassFixture<ApplicationFactoryFixture<PlayerServicesApp>>
{
    [Fact]
    public async Task Verify_a_Valid_Player_Registration_Event_Is_Processed()
    {
        // Arrange
        webHookServiceFactory.HostUrl = "http://localhost:5076";
        playerServiceFactory.HostUrl = "http://localhost:5054";
        
        var webHookServiceClient = webHookServiceFactory.CreateClient();
        playerServiceFactory.CreateClient();
        
        // Act
        var response = await webHookServiceClient.PostAsJsonAsync("/api/topic/publish", 
            new { topic = "event.playerRegistration", message = new { user = "Andrew", userType = "Slot Machine"} });
        
        // Assert
        response.EnsureSuccessStatusCode();
        playerServiceFactory.MockLogger.Verify(x => x.Information("User Registered: {User}", "Andrew"), Times.Once);
    }
};