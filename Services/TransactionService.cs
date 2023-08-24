using ImprovedPicpay.Helpers;
using ImprovedPicpay.Mappers;
using ImprovedPicpay.Repositories;
using ImprovedPicpay.ViewModels.Transaction;

namespace ImprovedPicpay.Services;

public class TransactionService
{
    private readonly TransactionRepository _repository;

    public TransactionService(TransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<bool>> CreateAsync(AddTransactionViewModel model)
    {
        return await _repository.CreateAsync(TransactionMapper.MapToUser(model));
    }

    public async Task<List<GetTransactionsViewModel>> GetAllAsync()
    {
        var transactions = await _repository.GetAsync();
        return TransactionMapper.MapToViewModel(transactions);
    }

    public async Task<List<GetTransactionsViewModel>> GetByAsync(string senderId)
    {
        var transactions = await _repository.GetByAsync(senderId);
        return TransactionMapper.MapToViewModel(transactions);
    }
}
