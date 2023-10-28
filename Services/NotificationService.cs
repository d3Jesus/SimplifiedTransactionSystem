using ImprovedPicpay.Helpers;
using ImprovedPicpay.Entities;
using ImprovedPicpay.ViewModels.Transaction;

namespace ImprovedPicpay.Services;

public class NotificationService
{
    private const string SERVICE = "http://o4d9z.mocklab.io/notify";
    private readonly ILogger<NotificationService> _logger;
    private readonly HttpClient _httpClient;

    public NotificationService(ILogger<NotificationService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<ServiceResponse<bool>> NotifyAsync(User user, string message)
    {
        ServiceResponse<bool> serviceResponse = new();
        using (_httpClient)
        {
            try
            {
                NotificationViewModel notification = new()
                {
                    email = user.Email,
                    message = message
                };
                //Sending request to find web api REST service resource using HttpClient
                var Res = await _httpClient.PostAsJsonAsync(SERVICE, notification);
                //Checking the response is successful or not which is sent using HttpClient
                if (!Res.IsSuccessStatusCode)
                {
                    serviceResponse.Succeeded = false;
                    serviceResponse.Message = "-- Connection error! --";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, " -- An error occored in notification service. --");
                serviceResponse.Succeeded = false;
                serviceResponse.Message = "-- Notification Service Error: Internal error! --";
            }
            return serviceResponse;
        }
    }
}
