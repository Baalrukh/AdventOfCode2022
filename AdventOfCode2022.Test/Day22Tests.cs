using AdventOfCode2022.Utils;

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
        Assert.AreEqual(5031, new Day22().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestParse_Ranges()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(Interval.FromToIncluded(8, 11), world.HorizontalRange[0]);
        Assert.AreEqual(Interval.FromToIncluded(8, 15), world.HorizontalRange[8]);
        Assert.AreEqual(Interval.FromToIncluded(4, 7), world.VerticalRange[0]);
        Assert.AreEqual(Interval.FromToIncluded(0, 11), world.VerticalRange[8]);
    }

    [Test]
    public void TestAdvanceIfNecessary_Distance0()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(8, 0), world.AdvanceIfNecessaryWithWrapping(new IntVector2(8, 0), Day22.Direction.Right, 0));
    }

    [Test]
    public void TestAdvanceIfNecessary_Simple_Right()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(10, 0), world.AdvanceIfNecessaryWithWrapping(new IntVector2(8, 0), Day22.Direction.Right, 2));
    }

    [Test]
    public void TestAdvanceIfNecessary_Simple_Bottom()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(8, 1), world.AdvanceIfNecessaryWithWrapping(new IntVector2(8, 0), Day22.Direction.Bottom, 1));
    }

    [Test]
    public void TestAdvanceIfNecessary_Wall()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(10, 0), world.AdvanceIfNecessaryWithWrapping(new IntVector2(8, 0), Day22.Direction.Right, 10));
    }

    [Test]
    public void TestAdvanceIfNecessary_WarpingRight()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(0, 5), world.AdvanceIfNecessaryWithWrapping(new IntVector2(11, 5), Day22.Direction.Right, 1));
    }
    
    [Test]
    public void TestAdvanceIfNecessary_WarpingRightIntoWall()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(11, 2), world.AdvanceIfNecessaryWithWrapping(new IntVector2(9, 2), Day22.Direction.Right, 10));
    }

    [Test]
    public void TestAdvanceIfNecessary_WarpingLeft()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(11, 5), world.AdvanceIfNecessaryWithWrapping(new IntVector2(0, 5), Day22.Direction.Left, 1));
    }

    [Test]
    public void TestAdvanceIfNecessary_WarpingTop()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(5, 7), world.AdvanceIfNecessaryWithWrapping(new IntVector2(5, 4), Day22.Direction.Top, 1));
    }

    [Test]
    public void TestAdvanceIfNecessary_WarpingBottom()
    {
        Day22.World world = Day22.World.Parse(_sampleLines.Take(_sampleLines.Length - 2).ToArray(), 4);
        Assert.AreEqual(new IntVector2(5, 4), world.AdvanceIfNecessaryWithWrapping(new IntVector2(5, 7), Day22.Direction.Bottom, 1));
    }

}
