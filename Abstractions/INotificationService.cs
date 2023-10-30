namespace ImprovedPicpay.Abstractions
{
    public interface INotificationService
    {
        Task NotifyAsync(string email, string message, CancellationToken cancellationToken = default);
    }
}
