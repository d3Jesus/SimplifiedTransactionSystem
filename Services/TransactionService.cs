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
}
