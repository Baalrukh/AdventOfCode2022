using AdventOfCode2022.Utils;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day09Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "R 4",
        "U 4",
        "L 3",
        "D 1",
        "R 4",
        "D 1",
        "L 5",
        "R 2"
    };
    private static readonly string[] _sampleLines2 = new[]
    {
        "R 5",
        "U 8",
        "L 8",
        "D 3",
        "R 17",
        "D 10",
        "L 25",
        "U 20"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(13, new Day09().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(1, new Day09().ExecutePart2(_sampleLines));
        Assert.AreEqual(36, new Day09().ExecutePart2(_sampleLines2));
    }

    [Test]
    public void TestUpdateTailPositionDoesNothingIfTailIsAdjacentToHead()
    {
        AssertTailAtSamePositionAfterUpdate(new IntVector2(-1, 0));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(-1, 1));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(0, 1));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(1, 1));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(1, 0));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(1, -1));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(0, -1));
        AssertTailAtSamePositionAfterUpdate(new IntVector2(-1, -1));
    }

    private void AssertTailAtSamePositionAfterUpdate(IntVector2 tailPosition)
    {
        Assert.AreEqual(tailPosition, Day09.UpdateTailPosition(new IntVector2(0, 0), tailPosition));
    }

    [Test]
    public void TestUpdateTailPositionMovesTowardHead_Orthogonal()
    {
        Assert.AreEqual(new IntVector2(-1, 0), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-2, 0)));
        Assert.AreEqual(new IntVector2(1, 0), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(2, 0)));
        Assert.AreEqual(new IntVector2(0, -1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(0, -2)));
        Assert.AreEqual(new IntVector2(0, 1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(0, 2)));
    }

    // XX.XX
    // X...X
    // ..H..
    // X...X
    // XX.XX
    [Test]
    public void TestUpdateTailPositionMovesTowardHead_Diagonal()
    {
        Assert.AreEqual(new IntVector2(-1, 0), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-2, -1)));
        Assert.AreEqual(new IntVector2(-1, -1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-2, -2)));
        Assert.AreEqual(new IntVector2(0, -1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-1, -2)));

        Assert.AreEqual(new IntVector2(0, -1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(1, -2)));
        Assert.AreEqual(new IntVector2(1, -1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(2, -2)));
        Assert.AreEqual(new IntVector2(1, 0), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(2, -1)));

        Assert.AreEqual(new IntVector2(1, 0), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(2, 1)));
        Assert.AreEqual(new IntVector2(1, 1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(2, 2)));
        Assert.AreEqual(new IntVector2(0, 1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(1, 2)));

        Assert.AreEqual(new IntVector2(0, 1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-1, 2)));
        Assert.AreEqual(new IntVector2(-1, 1), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-2, 2)));
        Assert.AreEqual(new IntVector2(-1, 0), Day09.UpdateTailPosition(new IntVector2(0, 0), new IntVector2(-2, 1)));
    }
}
