namespace PlayerServices.Services;

internal class WebhookService(IHttpClientFactory factory)
{
    public async Task RegisterEventTopicsAsync()
    {
        var client = factory.CreateClient("WebHookServer");
        await client.PostAsJsonAsync("/Subscribe", new { topic = "event.playerRegistration", callback = "http://localhost:5054/api/player/registration" });
        await client.PostAsJsonAsync("/Subscribe", new { topic = "event.playerDeactivate", callback = "http://localhost:5054/api/player/deactivate" });
    }
}