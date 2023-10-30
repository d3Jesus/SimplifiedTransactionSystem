using System.Text.Json.Serialization;

namespace ImprovedPicpay.Helpers;

public class ServiceAuthResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
