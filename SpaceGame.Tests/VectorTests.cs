using SpaceGame.Core.Domain;
using Xunit;

namespace SpaceGame.Tests;

public class VectorTests
{
    [Fact]
    public void Constructor_SetsXAndYPropertiesCorrectly()
    {
        var vector = new Vector(12, -5);

        Assert.Equal(12, vector.X);
        Assert.Equal(-5, vector.Y);
    }

    [Fact]
    public void AddOperator_ReturnsCorrectSum()
    {
        var v1 = new Vector(12, 5);
        var v2 = new Vector(-7, 3);

        Vector result = v1 + v2;

        Assert.Equal(new Vector(5, 8), result);
    }

    [Fact]
    public void AddOperator_OppositeVectors_ReturnsZeroVector()
    {
        var v1 = new Vector(4, -9);
        var v2 = new Vector(-4, 9);

        Vector result = v1 + v2;

        Assert.Equal(new Vector(0, 0), result);
    }

    [Fact]
    public void SubtractOperator_ReturnsCorrectDifference()
    {
        var v1 = new Vector(10, 4);
        var v2 = new Vector(3, -2);

        Vector result = v1 - v2;

        Assert.Equal(new Vector(7, 6), result);
    }

    [Fact]
    public void Equality_SameCoordinates_ReturnsTrue()
    {
        var v1 = new Vector(3, 7);
        var v2 = new Vector(3, 7);

        Assert.True(v1 == v2);
        Assert.False(v1 != v2);
        Assert.True(v1.Equals(v2));
        Assert.True(v1.Equals((object)v2));
        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentCoordinates_ReturnsFalse()
    {
        var v1 = new Vector(3, 7);
        var v2 = new Vector(7, 3);
        var v3 = new Vector(3, 8);

        Assert.False(v1 == v2);
        Assert.True(v1 != v2);
        Assert.False(v1.Equals(v2));
        Assert.False(v1.Equals(v3));
    }

    [Fact]
    public void Equals_WithNullOrDifferentType_ReturnsFalse()
    {
        var vector = new Vector(1, 2);

        Assert.False(vector.Equals(null));
        Assert.False(vector.Equals("строка вместо вектора"));
    }

    [Fact]
    public void Deconstruct_ExtractsXAndYCorrectly()
    {
        var vector = new Vector(15, -20);

        var (x, y) = vector;

        Assert.Equal(15, x);
        Assert.Equal(-20, y);
    }

    [Fact]
    public void ToString_ReturnsFormattedRepresentation()
    {
        var vector = new Vector(1, 2);

        string str = vector.ToString();

        Assert.Contains("1", str);
        Assert.Contains("2", str);
    }
}