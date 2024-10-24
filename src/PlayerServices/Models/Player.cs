using System.Text.Json.Serialization;

namespace PlayerServices.Models;

internal record Player(string User, string UserType)
{
    [JsonPropertyName("user")]
    public string User { get; set; } = User;

    [JsonPropertyName("userType")]
    public string UserType { get; set; } = UserType;
}