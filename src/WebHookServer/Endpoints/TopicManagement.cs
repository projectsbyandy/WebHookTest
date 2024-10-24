using WebHookServer.Models;
using WebHookServer.Services;
using ILogger = Serilog.ILogger;

namespace WebHookServer.Endpoints;

internal static class TopicManagement
{
     public static void MapTopicManagementEndpoints(this IEndpointRouteBuilder app)
     {
          var group = app.MapGroup("api/topic");
          
          group.MapPost("subscribe", SubscribeToTopic).WithName(nameof(SubscribeToTopic));
          group.MapPost("publish", PublishTopicAsync).WithName(nameof(PublishTopicAsync));
     }

     private static IResult SubscribeToTopic(WebHookService webHookService, Subscription subscription, ILogger logger)
     {
          try 
          {
               webHookService.Subscribe(subscription);
               return Results.Ok("Subscribed to topic");
          }
          catch (Exception ex)
          {
               logger.Error("Unable to Subscribe to topic due to {@Error}", ex.Message);
               return Results.Problem("Unable to Register user");
          }
     }
     
     private static async Task<IResult> PublishTopicAsync(WebHookService webHookService, PublishRequest publishRequest, ILogger logger)
     {
          try 
          {
               await webHookService.PublishEventAsync(publishRequest.Topic, publishRequest.Message);
               return Results.Ok("Published event to topic");
          }
          catch (Exception ex)
          {
               logger.Error("Unable to publish event to topic due to {@Error}", ex.Message);
               return Results.Problem("Unable to publish event to topic");
          }
     }
}