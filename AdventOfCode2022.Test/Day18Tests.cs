namespace AdventOfCode2022.Test;

[TestFixture]
public class Day18Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "2,2,2",
        "1,2,2",
        "3,2,2",
        "2,1,2",
        "2,3,2",
        "2,2,1",
        "2,2,3",
        "2,2,4",
        "2,2,6",
        "1,2,5",
        "3,2,5",
        "2,1,5",
        "2,3,5"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(64, new Day18().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(58, new Day18().ExecutePart2(_sampleLines));
    }
}
