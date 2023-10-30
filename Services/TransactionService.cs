using ImprovedPicpay.Abstractions;
using ImprovedPicpay.Helpers;
using Newtonsoft.Json;

namespace ImprovedPicpay.Services;

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;
    private const string RequestAuthorization = "https://run.mocky.io/v3/8fafdd68-a090-496f-8c9a-3442cf30dae6";
    private readonly ILogger<NotificationService> _logger;

    public TransactionService(HttpClient httpClient, ILogger<NotificationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ServiceResponse<bool>> IsAuthorized()
    {
        using (_httpClient)
        {
            try
            {
                //Sending request to find web api REST service resource using HttpClient
                var serviceResponse = await _httpClient.GetAsync(RequestAuthorization);
                //Checking the response is successful or not which is sent using HttpClient
                if (serviceResponse.IsSuccessStatusCode)
                {
                    //Storing the response details received from web api
                    var response = serviceResponse.Content.ReadAsStringAsync().Result;
                    //Deserializing the response received from web api
                    ServiceAuthResponse authResponse = JsonConvert.DeserializeObject<ServiceAuthResponse>(response);

                    return authResponse.Message.Equals("Autorizado");
                }

                return ServiceResponse.Failure<bool>(new Error("GetAuthorization.Error", "An error occurred while getting authorization."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException.Message, ex.Message);

                return ServiceResponse.Failure<bool>(new Error("GetAuthorization", ex.Message));
            }
        }
    }
}