using WebHookServer.Models;
using ILogger = Serilog.ILogger;

namespace WebHookServer.Services;

internal class WebHookService(ILogger logger, HttpClient httpClient)
{
    private readonly object _lockObject = new();
    
    private static readonly List<Subscription> Subscriptions = new();
    
    public void Subscribe(Subscription subscription)
    {
        lock (_lockObject)
        {
            if (Subscriptions.Contains(subscription))
            {
                logger.Warning("Topic: {@Topic} is already registered", subscription.Topic);
            }
            else
            {
                Subscriptions.Add(subscription);
                logger.Information("Subscription count: {@Count}", Subscriptions.Count);
                logger.Information("Subscription list updated: {@Subscriptions}", Subscriptions);   
            }   
        }
    }

    public async Task PublishMessageAsync(string topic, object message)
    {
        var subscribedWebHooks = Subscriptions.Where(webHook => webHook.Topic.Equals(topic));

        foreach (var webHook in subscribedWebHooks)
        {
            try
            {
                await httpClient.PostAsJsonAsync(webHook.CallBack, message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to subscribe web hook");
                throw;
            }
        }
    }
}