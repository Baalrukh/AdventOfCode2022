namespace AdventOfCode2022.Test;

[TestFixture]
public class Day20Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "1",
        "2",
        "-3",
        "3",
        "-2",
        "0",
        "4"
    };

    [Test]
    public void TestPart1()
    {
        CollectionAssert.AreEqual(new [] {4, -3, 2}, new Day20().GetCoordinates(_sampleLines));
        Assert.AreEqual(3, new Day20().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        CollectionAssert.AreEqual(new[] {811589153L, 2434767459L, -1623178306L}, new Day20().GetCoordinates_Part2(_sampleLines));
        Assert.AreEqual(1623178306L, new Day20().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestShiftNumber_Forward_Simple()
    {
        var sequence = Day20.ParseNumbers("4, 5, 6, 1, 7, 8, 9");
        sequence.ShiftNumberAtIndex(3);
        Assert.AreEqual("4, 5, 6, 7, 1, 8, 9", sequence.ToString());
    }

    [Test]
    public void TestShiftNumber_Forward_Wrapping()
    {
        var sequence = Day20.ParseNumbers("4, 5, 6, 1, 7, 8, 9");
        // "4, 5, 1, 6, 7, 8, 9"
        // "4, 5, 1, 7, 6, 8, 9"
        // "4, 5, 1, 7, 8, 6, 9"
        // "4, 5, 1, 7, 8, 9, 6"
        // "4, 6, 5, 1, 7, 8, 9"
        // "4, 5, 6, 1, 7, 8, 9"
        sequence.ShiftNumberAtIndex(2);
        Assert.AreEqual("4, 5, 6, 1, 7, 8, 9", sequence.ToString());
    }

    [Test]
    public void TestShiftNumber_Forward_WrappingMultipleTimes()
    {
        var sequence = Day20.ParseNumbers("4, 5, 19, 1, 7, 8, 9");
        sequence.ShiftNumberAtIndex(2);
        Assert.AreEqual("4, 5, 1, 19, 7, 8, 9", sequence.ToString());
    }

    [Test]
    public void TestShiftNumber_Backward_Simple()
    {
        var sequence = Day20.ParseNumbers("4, 5, 6, -1, 7, 8, 9");
        sequence.ShiftNumberAtIndex(3);
        Assert.AreEqual("4, 5, -1, 6, 7, 8, 9", sequence.ToString());
    }

    [Test]
    public void TestShiftNumber_Backward_Wrapping()
    {
        var sequence = Day20.ParseNumbers("4, -2, 5, 6, 7, 8, 9");
        sequence.ShiftNumberAtIndex(1);
        Assert.AreEqual("4, 5, 6, 7, 8, -2, 9", sequence.ToString());
    }

    [Test]
    public void TestUnMixSequence()
    {
        var sequence = Day20.ParseNumbers("1, 2, -3, 3, -2, 0, 4");
        sequence.UnMix();
        Assert.AreEqual("1, 2, -3, 4, 0, 3, -2", sequence.ToString());
    }

    [Test]
    public void TestFindNthElement()
    {
        var sequence = Day20.ParseNumbers("1, 2, -3, 4, 0, 3, -2");
        Assert.AreEqual(4, sequence.FindNthElementAfter0(1000).Value);
        Assert.AreEqual(-3, sequence.FindNthElementAfter0(2000).Value);
        Assert.AreEqual(2, sequence.FindNthElementAfter0(3000).Value);
    }
}
