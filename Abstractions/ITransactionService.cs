using ImprovedPicpay.Helpers;

namespace ImprovedPicpay.Abstractions;

public interface ITransactionService
{
    Task<ServiceResponse<bool>> IsAuthorized();
}
