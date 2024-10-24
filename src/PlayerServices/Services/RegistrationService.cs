using System.Text.Json;
using PlayerServices.Models;
using PlayerServices.Repositories;
using ILogger = Serilog.ILogger;

namespace PlayerServices.Services;

internal class RegistrationService(ILogger logger)
{
    public void RegisterPlayer(object payload)
    {
        logger.Information("Received Request to Register Player payload: {Payload}", payload);
        
        var stringPayload = payload.ToString();
        ArgumentNullException.ThrowIfNull(stringPayload);

        var player = JsonSerializer.Deserialize<Player>(stringPayload);
        
        if (PlayerRepository.Players.Exists(p => p.User.Equals(player?.User)))
            throw new ArgumentException("Player with same id already exists.");
            
        ArgumentNullException.ThrowIfNull(player);
        PlayerRepository.Players.Add(player);
        
        Task.Delay(2000).Wait();
        logger.Information("User Registered: {User}", player.User);
    }
}