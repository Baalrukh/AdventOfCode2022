namespace AdventOfCode2022.Test;

[TestFixture]
public class Day16Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
        "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
        "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
        "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
        "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
        "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
        "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
        "Valve HH has flow rate=22; tunnel leads to valve GG",
        "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
        "Valve JJ has flow rate=21; tunnel leads to valve II"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(1651, new Day16().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(-20, new Day16().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestParseValve()
    {
        Assert.AreEqual(new Day16.Valve("BB", 13, new[] {2, 0}), Day16.Parse(_sampleLines[1]));
    }
}
