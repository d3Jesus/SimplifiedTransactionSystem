
namespace ImprovedPicpay.ViewModels.Transaction;

public record struct GetTransactionsViewModel(string id,
                                              string senderId,
                                              string receiverId,
                                              decimal amount,
                                              DateTime timestamp);
