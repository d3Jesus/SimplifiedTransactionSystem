using ImprovedPicpay.Features.Users;

namespace ImprovedPicpay.Models;

public class Transaction
{
    public string Id { get; set; }
    public string From { get; set; }
    public virtual User Sender { get; set; }
    public string To { get; set; }
    public virtual User Receiver { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
