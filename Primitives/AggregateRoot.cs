using ImprovedPicpay.Abstractions;

namespace ImprovedPicpay.Primitives;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _events = new();

    public AggregateRoot(string id) : base(id)
    {
    }

    protected void RaiseEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }
}
