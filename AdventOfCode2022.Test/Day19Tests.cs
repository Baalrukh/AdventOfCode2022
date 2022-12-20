namespace AdventOfCode2022.Test;

[TestFixture]
public class Day19Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
        "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian."
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(33, new Day19().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(-20, new Day19().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestParseBlueprint()
    {
        Assert.AreEqual(new Day19.Blueprint(2, new Day19.Cost(2, 0, 0), new Day19.Cost(3, 0, 0), new Day19.Cost(3, 8, 0), new Day19.Cost(3, 0, 12)),
            Day19.ParseBlueprint(_sampleLines[1]));
    }

    [Test]
    public void TestGetCollectedGeodeCount1()
    {
        var blueprint = Day19.ParseBlueprint(_sampleLines[0]);
        Assert.AreEqual(9, blueprint.GetCollectedGeodeCount(24));
    }

    [Test]
    public void TestGetCollectedGeodeCount2()
    {
        var blueprint = Day19.ParseBlueprint(_sampleLines[1]);
        Assert.AreEqual(12, blueprint.GetCollectedGeodeCount(24));
    }
}
