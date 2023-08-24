using ImprovedPicpay.Data;
using ImprovedPicpay.Enums;
using ImprovedPicpay.Helpers;
using ImprovedPicpay.Models;
using ImprovedPicpay.Services;
using Newtonsoft.Json;

namespace ImprovedPicpay.Repositories;

public class TransactionRepository
{
    /// <summary>
    /// Determines if the user is authorized or not to perform a transaction.
    /// </summary>
    private const string GetAuthorization = "https://run.mocky.io/v3/8fafdd68-a090-496f-8c9a-3442cf30dae6";
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TransactionRepository> _logger;
    private readonly UserRepository _userRepository;
    private readonly HttpClient _httpClient;
    private readonly NotificationService _notificationService;

    public TransactionRepository(ApplicationDbContext context,
                                 ILogger<TransactionRepository> logger,
                                 UserRepository userRepository,
                                 HttpClient httpClient,
                                 NotificationService notificationService)
    {
        _context = context;
        _logger = logger;
        _userRepository = userRepository;
        _httpClient = httpClient;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Register a transaction.
    /// </summary>
    /// <param name="transaction">Transaction object.</param>
    /// <returns>Service response of type boolean</returns>
    public async Task<ServiceResponse<bool>> CreateAsync(Transaction transaction)
    {
        using var contextTransaction = _context.Database.BeginTransaction();
        try
        {
            User sender = await _userRepository.GetByAsync(transaction.From);
            User receiver = await _userRepository.GetByAsync(transaction.To);

            if (!(await IsAuthorized()))
                return new ServiceResponse<bool>
                {
                    Succeeded = false,
                    Message = "You are not authorized."
                };

            if (!(await IsCommonUser(sender.Id)))
                return new ServiceResponse<bool>
                {
                    Succeeded = false,
                    Message = "You can only receive money."
                };

            if (!(await HaveEnoughBalance(sender.Id, sender.Balance)))
                return new ServiceResponse<bool>
                {
                    Succeeded = false,
                    Message = "You don't have enough balance to perform this operation."
                };

            sender.Balance -= transaction.Amount;
            receiver.Balance += transaction.Amount;

            transaction.Id = Guid.NewGuid().ToString();
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            await _notificationService.NotifyAsync(sender, $"You sent {transaction.Amount} to {receiver.UserFirstName}.");
            await _notificationService.NotifyAsync(receiver, $"{sender.UserFirstName} just sent you {transaction.Amount}.");

            await contextTransaction.CommitAsync();

            return new ServiceResponse<bool>
            {
                Message = "Transaction completed successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.InnerException?.Message, ex.Message, "-- Error while creating a new transaction. --", DateTime.Now);
            await contextTransaction.RollbackAsync();

            return new ServiceResponse<bool>
            {
                Succeeded = false,
                Message = "An error occorred during transaction."
            };
        }
    }

    /// <summary>
    /// Verify if a user(sender) with the given ID has enough balance to perform this operation.
    /// </summary>
    /// <param name="senderId">Sender Id</param>
    /// <param name="amount">Amount that wants to send.</param>
    /// <returns>True if the user has enough balance and false otherwise.</returns>
    private async Task<bool> HaveEnoughBalance(string senderId, decimal amount)
    {
        var user = await _userRepository.GetByAsync(senderId);
        if (user.Balance >= amount)
            return true;

        return false;
    }

    /// <summary>
    /// Verify if a user(sender) with the given ID is a accepted type to perform this operation.
    /// Only common users can send money. Shopkeepers can only receive.
    /// </summary>
    /// <param name="senderId">User Id</param>
    /// <returns>True if the user has enough balance and false otherwise.</returns>
    private async Task<bool> IsCommonUser(string senderId)
    {
        var user = await _userRepository.GetByAsync(senderId);
        if (user.UserType.Equals(UserTypes.Common.ToString()))
            return true;

        return false;
    }

    /// <summary>
    /// Verify if the user is authorized or not to perform a transaction
    /// </summary>
    private async Task<bool> IsAuthorized()
    {
        using (_httpClient)
        {
            //Sending request to find web api REST service resource using HttpClient
            var serviceResponse = await _httpClient.GetAsync(GetAuthorization);
            //Checking the response is successful or not which is sent using HttpClient
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details received from web api
                var response = serviceResponse.Content.ReadAsStringAsync().Result;
                //Deserializing the response received from web api
                ServiceAuthResponse authResponse = JsonConvert.DeserializeObject<ServiceAuthResponse>(response);

                if (authResponse.message.Equals("Autorizado"))
                    return true;
            }
        }

        return false;
    }
}
