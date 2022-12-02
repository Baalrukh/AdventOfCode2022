namespace AdventOfCode2022.Test;

[TestFixture]
public class Day2Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "A Y",
        "B X",
        "C Z"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(15, new Day02().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(12, new Day02().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestMatchVictoryPoints()
    {
        Day02 day02 = new Day02();
        Assert.AreEqual(0, day02.GetMatchVictoryPoints(Day02.RPSObject.Rock, Day02.RPSObject.Scissors));
        Assert.AreEqual(3, day02.GetMatchVictoryPoints(Day02.RPSObject.Rock, Day02.RPSObject.Rock));
        Assert.AreEqual(6, day02.GetMatchVictoryPoints(Day02.RPSObject.Rock, Day02.RPSObject.Paper));
    }

    [Test]
    public void TestGetMatchupResultString()
    {
        Day02 day02 = new Day02();
        Assert.AreEqual("A X", day02.GetMatchupResultString((Day02.RPSObject.Rock, Day02.RPSObject.Scissors)));
        Assert.AreEqual("B Y", day02.GetMatchupResultString((Day02.RPSObject.Paper, Day02.RPSObject.Paper)));
        Assert.AreEqual("C Z", day02.GetMatchupResultString((Day02.RPSObject.Scissors, Day02.RPSObject.Rock)));
    }
}
