using ImprovedPicpay.Abstractions;

namespace ImprovedPicpay.Services;

public class NotificationService : INotificationService
{
    private const string SERVICE = "http://o4d9z.mocklab.io/notify";
    private readonly ILogger<NotificationService> _logger;
    private readonly HttpClient _httpClient;

    public NotificationService(ILogger<NotificationService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task NotifyAsync(string email, string message, CancellationToken cancellationToken)
    {
        using (_httpClient)
        {
            try
            {
                var notification = new NotificationResponse(email, message);
                //Sending request to find web api REST service resource using HttpClient
                var Res = await _httpClient.PostAsJsonAsync(SERVICE, notification, cancellationToken);
                //Checking the response is successful or not which is sent using HttpClient
                if (!Res.IsSuccessStatusCode)
                {
                    _logger.LogError(" -- An error occurred in notification service. -- ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, " -- An error occurred in notification service. -- ");
            }
        }
    }
}

public sealed record NotificationResponse(string email, string message);
