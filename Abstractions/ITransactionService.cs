namespace ImprovedPicpay.Abstractions;

public interface ITransactionService
{
    Task<bool> IsAuthorized();
}
