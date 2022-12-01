namespace AdventOfCode2022.Test;

[TestFixture]
public class Day01Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "1000",
        "2000",
        "3000",
        "",
        "4000",
        "",
        "5000",
        "6000",
        "",
        "7000",
        "8000",
        "9000",
        "",
        "10000",
    };

    [Test]
    public void TestBaseSample()
    {
        Assert.AreEqual(24000, new Day01().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestAdvancedSample()
    {
        Assert.AreEqual(45000, new Day01().ExecutePart2(_sampleLines));
    }
}