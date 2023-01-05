using AdventOfCode2022.Utils;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day17Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(3068, new Day17().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(1514285714288L, new Day17().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestPart2_debug()
    {
        Assert.AreEqual(1514285714288L, new Day17().ExecutePart2_NotWorking(_sampleLines));
    }

    [Test]
    public void Test1RockFall()
    {
        Day17.TetrisWorld world = new Day17.TetrisWorld(7);
        var jetsEnumerator = new Day17.CountingLoopingEnumerator<char>(_sampleLines[0].ToList());

        world.MakeBlockFall(Day17.PIECES[0], Day17.PIECES_SIZE[0], jetsEnumerator);
        var map = world.PrintMap();
        Assert.AreEqual("|..####.|\n", map);
    }

    [Test]
    public void Test2RockFall()
    {
        Day17.TetrisWorld world = new Day17.TetrisWorld(7);
        var jetsEnumerator = new LoopingEnumerator<char>(_sampleLines[0].ToList());

        for (int i = 0; i < 2; i++)
        {
            world.MakeBlockFall(Day17.PIECES[i], Day17.PIECES_SIZE[i], jetsEnumerator);

        }
        var map = world.PrintMap();
        Assert.AreEqual(
@"|...#...|
|..###..|
|...#...|
|..####.|\n".Replace("\r\n", "\n"), map);
    }

    [Test]
    public void TestWorldSpawnPiece()
    {
        Day17.TetrisWorld world = new(7);
        Assert.AreEqual(new IntVector2(2, 5), world.GetSpawnPosition(Day17.PIECES_SIZE[1].Y));
        world.MaxHeight = 5;
        Assert.AreEqual(new IntVector2(2, 10), world.GetSpawnPosition(Day17.PIECES_SIZE[1].Y));
    }

    [Test]
    public void TestWorldCanFall_NoObstacle()
    {
        Day17.TetrisWorld world = new(7);
        Assert.IsTrue(world.CanFall(new IntVector2(2, 5), Day17.PIECES[1], Day17.PIECES_SIZE[1].Y));
    }

    [Test]
    public void TestWorldCanFall_Bottom()
    {
        Day17.TetrisWorld world = new(7);
        Assert.IsFalse(world.CanFall(new IntVector2(2, 2), Day17.PIECES[1], Day17.PIECES_SIZE[1].Y));
    }

    [Test]
    public void TestWorldCanFall_Obstacle()
    {
        Day17.TetrisWorld world = new(7);
        world.MaxHeight = 1;
        world.Blocks.Add(new IntVector2(2, 0));
        Assert.IsTrue(world.CanFall(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].Y));
        
        world.Blocks.Add(new IntVector2(3, 0));
        Assert.IsFalse(world.CanFall(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].Y));
    }

    [Test]
    public void TestWorldCanPushLeft_Bounds()
    {
        Day17.TetrisWorld world = new(7);
        Assert.IsTrue(world.CanPush(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '<', out _));
        
        Assert.IsFalse(world.CanPush(new IntVector2(0, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '<', out _));
    }

    [Test]
    public void TestWorldCanPushLeft_Obstacle()
    {
        Day17.TetrisWorld world = new(7);
        world.Blocks.Add(new IntVector2(1, 3));
        Assert.IsTrue(world.CanPush(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '<', out _));
        
        world.Blocks.Add(new IntVector2(1, 2));
        Assert.IsFalse(world.CanPush(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '<', out _));
    }

    [Test]
    public void TestWorldCanPushRight_Bounds()
    {
        Day17.TetrisWorld world = new(7);
        Assert.IsTrue(world.CanPush(new IntVector2(3, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '>', out _));
        
        Assert.IsFalse(world.CanPush(new IntVector2(4, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '>', out _));
    }

    [Test]
    public void TestWorldCanPushRight_Obstacle()
    {
        Day17.TetrisWorld world = new(7);
        world.Blocks.Add(new IntVector2(5, 3));
        Assert.IsTrue(world.CanPush(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '>', out _));
        
        world.Blocks.Add(new IntVector2(5, 2));
        Assert.IsFalse(world.CanPush(new IntVector2(2, 3), Day17.PIECES[1], Day17.PIECES_SIZE[1].X, '>', out _));
    }
}
