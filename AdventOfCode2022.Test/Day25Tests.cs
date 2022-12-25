namespace AdventOfCode2022.Test;

[TestFixture]
public class Day25Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "1=-0-2",
        "12111",
        "2=0=",
        "21",
        "2=01",
        "111",
        "20012",
        "112",
        "1=-1=",
        "1-12",
        "12",
        "1=",
        "122",
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual("2=-1=0", Day25.GetSNAFUSum(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(-20, new Day25().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestParseSNAFUNumber()
    {
        int[] exepectedValues = new[] { 1747, 906, 198, 11, 201, 31, 1257, 32, 353, 107, 7, 3, 37 };
        for (int i = 0; i < _sampleLines.Length; i++)
        {
            Assert.AreEqual(exepectedValues[i], new Day25.SNAFUNumber(_sampleLines[i]).LongValue);
        }
    }

    [Test]
    public void TestFromInt()
    {
        int[] values = new[] { 1747, 906, 198, 11, 201, 31, 1257, 32, 353, 107, 7, 3, 37 };
        for (int i = 0; i < _sampleLines.Length; i++)
        {
            Assert.AreEqual(_sampleLines[i], Day25.SNAFUNumber.FromLong(values[i]).TextValue);
        }
        
    }
}
