namespace ImprovedPicpay.Features.Transactions;

public record TransactionResponse(string Id,
                                  string SenderId,
                                  string ReceiverId,
                                  decimal Amount,
                                  DateTime Timestamp);
