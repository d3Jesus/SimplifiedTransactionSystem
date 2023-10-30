namespace ImprovedPicpay.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(string id)
    {
        Id = id;
    }

    public string Id { get; private init; }

    public bool Equals(Entity other)
    {
        if (other is null) return false;

        if (other.GetType() != GetType()) return false;

        return true;
    }
}
