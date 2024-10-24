namespace WebHookServer.Models;

internal record PublishRequest(string Topic, object Message);