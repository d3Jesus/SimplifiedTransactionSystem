namespace ImprovedPicpay.Entities;

public class Transaction
{
    public string Id { get; set; }
    public string SenderId { get; set; }
    public virtual User Sender { get; set; }
    public string ReceiverId { get; set; }
    public virtual User Receiver { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
