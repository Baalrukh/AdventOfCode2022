namespace AdventOfCode2022.Test;

[TestFixture]
public class Day22Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "        ...#",
        "        .#..",
        "        #...",
        "        ....",
        "...#.......#",
        "........#...",
        "..#....#....",
        "..........#.",
        "        ...#....",
        "        .....#..",
        "        .#......",
        "        ......#.",
        "",
        "10R5L5R10L4R5L5",
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(6032, new Day22().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(-20, new Day22().ExecutePart2(_sampleLines));
    }
}
