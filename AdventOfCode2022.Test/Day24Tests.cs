using AdventOfCode2022.Utils;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day24Tests
{
    private static readonly string[] _simpleSampleLines = new[]
    {
        "#.#####",
        "#.....#",
        "#>....#",
        "#.....#",
        "#....v#",
        "#.....#",
        "#####.#"
    };
    
    private static readonly string[] _sampleLines = new[]
    {
        "#.######",
        "#>>.<^<#",
        "#.<..<<#",
        "#>v.><>#",
        "#<^v^^>#",
        "######.#"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(18, new Day24().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(54, new Day24().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestParseMap()
    {
        Day24.Blizzard b1 = new Day24.Blizzard(new IntVector2(0, 1), Day24.Direction.Right);
        Day24.Blizzard b2 = new Day24.Blizzard(new IntVector2(4, 3), Day24.Direction.Down);
        Day24.BlizzardsState blizzardsState = new Day24.BlizzardsState(new[] { b1, b2 }, 5, 5);
        Day24.Valley valley = Day24.ParseValley(_simpleSampleLines);
        Assert.AreEqual(new Day24.Valley(5, 5, new IntVector2(0, -1), new IntVector2(4, 5), blizzardsState),
                        valley);
        CollectionAssert.AreEqual(new [] {b1, b2}, valley.BlizzardState.Blizzards);
    }

    [Test]
    public void TestGetWalkabilityMap()
    {
        Day24.Valley valley = Day24.ParseValley(_simpleSampleLines);
        Day24.ValleyWalkability map = valley.BlizzardState.GetWalkabilityMap();
        CollectionAssert.AreEqual(new bool[]
                                  {
                                      true, true, true, true, true,
                                      false, true, true, true, true,
                                      true, true, true, true, true,
                                      true, true, true, true, false,
                                      true, true, true, true, true,
                                  },
                                  map._walkability);
    }

    [Test]
    public void TestBlizzardsUpdate()
    {
        Day24.BlizzardsState blizzardsState = new Day24.BlizzardsState(new[]
        {
            new Day24.Blizzard(new IntVector2(1, 0), Day24.Direction.Right),
            new Day24.Blizzard(new IntVector2(0, 1), Day24.Direction.Down),
            new Day24.Blizzard(new IntVector2(4, 0), Day24.Direction.Left),
            new Day24.Blizzard(new IntVector2(0, 4), Day24.Direction.Up)
        }, 5, 5);
        blizzardsState.UpdateBlizzards();

        CollectionAssert.AreEqual(new[]
        {
            new Day24.Blizzard(new IntVector2(2, 0), Day24.Direction.Right),
            new Day24.Blizzard(new IntVector2(0, 2), Day24.Direction.Down),
            new Day24.Blizzard(new IntVector2(3, 0), Day24.Direction.Left),
            new Day24.Blizzard(new IntVector2(0, 3), Day24.Direction.Up)
        }, blizzardsState.Blizzards);
    }

    [Test]
    public void TestBlizzardsUpdate_Wrapping()
    {
        Day24.BlizzardsState blizzardsState = new Day24.BlizzardsState(new[]
        {
            new Day24.Blizzard(new IntVector2(4, 0), Day24.Direction.Right),
            new Day24.Blizzard(new IntVector2(0, 4), Day24.Direction.Down),
            new Day24.Blizzard(new IntVector2(0, 0), Day24.Direction.Left),
            new Day24.Blizzard(new IntVector2(0, 0), Day24.Direction.Up),
        }, 5, 5);
        blizzardsState.UpdateBlizzards();

        CollectionAssert.AreEqual(new[]
        {
            new Day24.Blizzard(new IntVector2(0, 0), Day24.Direction.Right),
            new Day24.Blizzard(new IntVector2(0, 0), Day24.Direction.Down),
            new Day24.Blizzard(new IntVector2(4, 0), Day24.Direction.Left),
            new Day24.Blizzard(new IntVector2(0, 4), Day24.Direction.Up),
        }, blizzardsState.Blizzards);
    }
    
}
