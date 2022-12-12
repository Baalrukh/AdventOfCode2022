namespace AdventOfCode2022.Test;

[TestFixture]
public class Day12Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "Sabqponm",
        "abcryxxl",
        "accszExk",
        "acctuvwj",
        "abdefghi"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(31, new Day12().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(29, new Day12().ExecutePart2(_sampleLines));
    }
}
