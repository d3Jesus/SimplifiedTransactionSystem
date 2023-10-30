using ImprovedPicpay.Abstractions;
using ImprovedPicpay.Helpers;
using Newtonsoft.Json;

namespace ImprovedPicpay.Services;

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;
    private const string RequestAuthorization = "https://run.mocky.io/v3/8fafdd68-a090-496f-8c9a-3442cf30dae6";

    public TransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsAuthorized()
    {
        using (_httpClient)
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

            return false;
        }
    }
}