using ImprovedPicpay.Models;
using ImprovedPicpay.ViewModels.Transaction;

namespace ImprovedPicpay.Mappers;

public class TransactionMapper
{
    /// <summary>
    /// Maps from a list of Transactions object to list of GetTransactionsViewModel object
    /// </summary>
    /// <param name="transaction">List of Transaction object</param>
    /// <returns>Mapped list to GetTransactionsViewModel</returns>
    public static List<GetTransactionsViewModel> MapToViewModel(IEnumerable<Transaction> transaction)
    {
        return transaction.Select(tr => new GetTransactionsViewModel
        {
            id = tr.Id,
            senderId = tr.From,
            receiverId = tr.To,
            amount = tr.Amount,
            timestamp = tr.Timestamp
        }).ToList();
    }

    /// <summary>
    /// Maps from a view model object to Transaction object
    /// </summary>
    /// <param name="viewModel">View Model object</param>
    /// <returns>Mapped object to Transaction/returns>
    public static Transaction MapToUser(AddTransactionViewModel viewModel)
    {
        return new Transaction
        {
            From = viewModel.From,
            To = viewModel.To,
            Amount= viewModel.Amount
        };
    }
}
