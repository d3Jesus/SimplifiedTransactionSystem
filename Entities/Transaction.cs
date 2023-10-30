namespace ImprovedPicpay.Entities;

public class Transaction
{
    internal Transaction(string id, string senderId, string receiverId, decimal amount)
    {
        Id = id;
        SenderId = senderId;
        ReceiverId = receiverId;
        Amount = amount;
        Timestamp = DateTime.UtcNow;
    }

    public string Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
