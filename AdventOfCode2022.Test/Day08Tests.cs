using AdventOfCode2022.Utils;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day08Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "30373",
        "25512",
        "65332",
        "33549",
        "35390",
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(21, new Day08().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(8, new Day08().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestGetVisibleTreeCountInDirection_Border()
    {
        var map = Day08.ParseTreeHeights(_sampleLines);
        Assert.AreEqual(1, Day08.GetVisibleTreeCountInDirection(map, new IntVector2(2, 1), new IntVector2(0, -1)));
    }

    [Test]
    public void TestGetVisibleTreeCountInDirection_Height()
    {
        var map = Day08.ParseTreeHeights(_sampleLines);
        Assert.AreEqual(1, Day08.GetVisibleTreeCountInDirection(map, new IntVector2(2, 1), new IntVector2(-1, 0)));
        Assert.AreEqual(2, Day08.GetVisibleTreeCountInDirection(map, new IntVector2(2, 3), new IntVector2(0, -1)));
    }

    [Test]
    public void TestGetVisibleTreeCount()
    {
        var map = Day08.ParseTreeHeights(_sampleLines);
        Assert.AreEqual(4, Day08.GetTreeVisibilityScore(map, new IntVector2(2, 1)));
        Assert.AreEqual(8, Day08.GetTreeVisibilityScore(map, new IntVector2(2, 3)));
    }
}
