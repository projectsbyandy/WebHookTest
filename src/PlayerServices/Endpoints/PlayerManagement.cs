using PlayerServices.Services;
using ILogger = Serilog.ILogger;

namespace PlayerServices.Endpoints;

internal static class PlayerManagement
{
    public static void MapPlayerManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/player");
        
        group.MapPost("registration", RegisterUser).WithName(nameof(RegisterUser));
        group.MapPost("deactivate", DecommissionUser).WithName(nameof(DecommissionUser));
    }

    private static IResult RegisterUser(RegistrationService registrationService,  ILogger logger, object payload)
    {
        try
        {
            registrationService.RegisterPlayer(payload);
            
            return Results.Ok("Successfully Registered");
        }
        catch (Exception ex)
        {
            logger.Error("Unable to Register user due to {@Error}", ex.Message);
            
            return Results.Problem("Unable to Register user");
        }
    }

    private static IResult DecommissionUser(DeactivationService deactivationService,  ILogger logger, object payload)
    {
        try
        {
            deactivationService.RemovePlayer(payload);
            
            return Results.Ok("Successfully Deactivated");
        }
        catch (Exception ex)
        {
            logger.Error("Unable to Decommission user due to {@Error}", ex.Message);
            
            return Results.Problem("Unable to Decommission user");
        }
    }
}