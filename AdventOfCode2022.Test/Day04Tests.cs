namespace AdventOfCode2022.Test;

[TestFixture]
public class Day04Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "2-4,6-8",
        "2-3,4-5",
        "5-7,7-9",
        "2-8,3-7",
        "6-6,4-6",
        "2-6,4-8"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(2, new Day04().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(4, new Day04().ExecutePart2(_sampleLines));
    }
}
