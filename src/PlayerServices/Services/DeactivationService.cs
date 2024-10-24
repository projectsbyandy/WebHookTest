using System.Text.Json;
using PlayerServices.Models;
using PlayerServices.Repositories;
using ILogger = Serilog.ILogger;

namespace PlayerServices.Services;

internal class DeactivationService(ILogger logger)
{
    public void RemovePlayer(object payload)
    {
        logger.Information("Received Request to Deactivate Player payload: {Payload}", payload);
        
        var stringPayload = payload as string;
        ArgumentNullException.ThrowIfNull(stringPayload);
        
        var playerToDeactivate = JsonSerializer.Deserialize<Player>(stringPayload);
        
        ArgumentNullException.ThrowIfNull(playerToDeactivate);
        
        if (PlayerRepository.Players.Exists(p => p.User.Equals(playerToDeactivate.User)))
        {
            PlayerRepository.Players.Remove(playerToDeactivate);
            logger.Information("User Deactivated");
        }
        else
        {
            throw new ArgumentException("Player does not exist");   
        }
    }
}