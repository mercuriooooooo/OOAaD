namespace SpaceGame.Core.Domain;

public readonly record struct Angle
{
    public int Direction { get; }
    public int DirectionsCount { get; }

    public Angle(int direction, int directionsCount = 8)
    {
        if (directionsCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(directionsCount), "Количество направлений должно быть положительным.");

        DirectionsCount = directionsCount;
        Direction = ((direction % directionsCount) + directionsCount) % directionsCount;
    }

    public static Angle operator +(Angle a, Angle b)
    {
        if (a.DirectionsCount != b.DirectionsCount)
            throw new ArgumentException("Несовпадение размерности углов (DirectionsCount).");

        return new Angle(a.Direction + b.Direction, a.DirectionsCount);
    }

    public double ToRadians() => 2.0 * Math.PI * Direction / DirectionsCount;
}