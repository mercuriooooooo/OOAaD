using SpaceGame.Core.Domain;

namespace SpaceBattle.Lib;

public static class VectorExtensions
{
    public static double DistanceTo(this Vector a, Vector b) =>
        Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
}
