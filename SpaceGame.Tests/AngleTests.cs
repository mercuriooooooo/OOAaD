using SpaceGame.Core.Domain;
using Xunit;

namespace SpaceGame.Tests;

public class AngleTests
{
    [Fact]
    public void Constructor_NormalizesPositiveDirectionModuloDirectionsCount()
    {
        var a1 = new Angle(15); 
        var a2 = new Angle(23); 

        Assert.Equal(7, a1.Direction);
        Assert.Equal(8, a1.DirectionsCount);
        Assert.Equal(a1, a2);
    }

    [Fact]
    public void Constructor_NormalizesNegativeDirectionCorrectly()
    {
        var angle = new Angle(-3, 8); // (-3 % 8 + 8) % 8 = 5

        Assert.Equal(5, angle.Direction);
        Assert.Equal(8, angle.DirectionsCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-8)]
    public void Constructor_ThrowsArgumentOutOfRangeException_WhenDirectionsCountIsZeroOrNegative(int invalidDirectionsCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Angle(1, invalidDirectionsCount));
    }

    [Fact]
    public void AddOperator_SumsAnglesWithWrapAround()
    {
        var a1 = new Angle(5, 8);
        var a2 = new Angle(7, 8);

        Angle result = a1 + a2; 

        Assert.Equal(4, result.Direction);
        Assert.Equal(8, result.DirectionsCount);
    }

    [Fact]
    public void AddOperator_ThrowsArgumentException_WhenDirectionsCountMismatch()
    {
        var a1 = new Angle(3, 8);
        var a2 = new Angle(3, 16);

        Assert.Throws<ArgumentException>(() => a1 + a2);
        Assert.Throws<ArgumentException>(() => a2 + a1);
    }

    [Theory]
    [InlineData(0, 8, 0.0)]
    [InlineData(2, 8, Math.PI / 2.0)]
    [InlineData(4, 8, Math.PI)]
    [InlineData(6, 8, 3.0 * Math.PI / 2.0)]
    public void ToRadians_CalculatesCorrectRadianValue(int direction, int directionsCount, double expectedRadians)
    {
        var angle = new Angle(direction, directionsCount);

        double actualRadians = angle.ToRadians();

        Assert.Equal(expectedRadians, actualRadians, precision: 6);
    }

    [Fact]
    public void Equality_SameDirectionAndDirectionsCount_ReturnsTrue()
    {
        var a1 = new Angle(15, 8);
        var a2 = new Angle(7, 8);

        Assert.True(a1 == a2);
        Assert.False(a1 != a2);
        Assert.True(a1.Equals(a2));
        Assert.True(a1.Equals((object)a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentDirectionOrDirectionsCount_ReturnsFalse()
    {
        var a1 = new Angle(1, 8);
        var a2 = new Angle(2, 8);
        var a3 = new Angle(1, 16);

        Assert.False(a1 == a2);
        Assert.True(a1 != a2);
        Assert.False(a1.Equals(a2));
        Assert.False(a1.Equals(a3));
    }

    [Fact]
    public void Equals_WithNullOrDifferentType_ReturnsFalse()
    {
        var angle = new Angle(1);

        Assert.False(angle.Equals(null));
        Assert.False(angle.Equals("не угол"));
    }

    [Fact]
    public void ToString_ReturnsFormattedRepresentation()
    {
        var angle = new Angle(3, 8);

        string str = angle.ToString();

        Assert.Contains("3", str);
        Assert.Contains("8", str);
    }
}