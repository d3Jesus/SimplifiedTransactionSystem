using ImprovedPicpay.Abstractions;
using MediatR;

namespace ImprovedPicpay.Events;

public sealed class TransactionCompletedSuccessfulyDomainEventHandler
    : INotificationHandler<TransactionCompletedSuccessfulyDomainEvent>
{
    private readonly INotificationService _notificationService;

    public async Task Handle(TransactionCompletedSuccessfulyDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notificationService.NotifyAsync(notification.receiverEmail, $"{notification.senderName} just sent you {notification.amount}.");
    }
}

public sealed record TransactionCompletedSuccessfulyDomainEvent(string receiverEmail, string senderName, decimal amount, string message) : IDomainEvent { }
