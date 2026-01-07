namespace OrderManagement.Domain.ValueObjects;

public class Quantity : IEquatable<Quantity>
{
    public int Value { get; }

    public Quantity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(value));

        Value = value;
    }

    public bool Equals(Quantity? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Quantity quantity && Equals(quantity);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Quantity? left, Quantity? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Quantity? left, Quantity? right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
