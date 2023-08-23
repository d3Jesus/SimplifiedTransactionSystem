
namespace ImprovedPicpay.ViewModels.Transaction;

public record struct GetTransactionsViewModel(string id,
                                              string senderName,
                                              string receiverName,
                                              decimal amount,
                                              DateTime timestamp);
