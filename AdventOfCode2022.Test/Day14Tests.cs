using AdventOfCode2022.Utils;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day14Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "498,4 -> 498,6 -> 496,6",
        "503,4 -> 502,4 -> 502,9 -> 494,9"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(24, new Day14().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(93, new Day14().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestParseLine()
    {
        var filledPositions = new HashSet<IntVector2>();
        Day14.ParseLine(_sampleLines[0], filledPositions);
        CollectionAssert.AreEqual(new IntVector2[] {new(498, 4), new(498, 5), new(498, 6), new(497, 6), new(496, 6)}, filledPositions);
    }

    [Test]
    public void TestUpdateFallStep_belowEmpty()
    {
        var filledPositions = new HashSet<IntVector2>();
        IntVector2 particlePosition = new IntVector2(0, 0);

        Assert.AreEqual(true, Day14.UpdateFallStep(ref particlePosition, filledPositions));
        Assert.AreEqual(new IntVector2(0, 1), particlePosition);
    }

    [Test]
    public void TestUpdateFallStep_fallLeft()
    {
        var filledPositions = new HashSet<IntVector2>() {new (0, 1)};
        IntVector2 particlePosition = new IntVector2(0, 0);

        Assert.AreEqual(true, Day14.UpdateFallStep(ref particlePosition, filledPositions));
        Assert.AreEqual(new IntVector2(-1, 1), particlePosition);
    }

    [Test]
    public void TestUpdateFallStep_fallRight()
    {
        var filledPositions = new HashSet<IntVector2>() {new (0, 1), new (-1, 1)};
        IntVector2 particlePosition = new IntVector2(0, 0);

        Assert.AreEqual(true, Day14.UpdateFallStep(ref particlePosition, filledPositions));
        Assert.AreEqual(new IntVector2(1, 1), particlePosition);
    }

    [Test]
    public void TestUpdateFallStep_stop()
    {
        var filledPositions = new HashSet<IntVector2>() {new (0, 1), new (-1, 1), new (1, 1)};
        IntVector2 particlePosition = new IntVector2(0, 0);

        Assert.AreEqual(false, Day14.UpdateFallStep(ref particlePosition, filledPositions));
        Assert.AreEqual(new IntVector2(0, 0), particlePosition);
    }
}
