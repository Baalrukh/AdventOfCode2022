using AdventOfCode2022.Utils;
using Moq;

namespace AdventOfCode2022.Test;

[TestFixture]
public class Day23Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "....#..",
        "..###.#",
        "#...#.#",
        ".#...##",
        "#.###..",
        "##.#.##",
        ".#..#.."
    };

    private static readonly string[] _samellSampleLines = new[]
    {
        "....",
        "..##.",
        "..#..",
        ".....",
        "..##.",
        ".....",
    };
    
    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(110, new Day23().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(20, new Day23().ExecutePart2(_sampleLines));
    }

    [Test]
    public void TestUpdateSmallSample()
    {
        Day23.World world = new Day23.World(Day23.ParsePositions(_samellSampleLines), Day23.CreateRules());
        world.Update();
        string plot = world.Plot();
        Assert.AreEqual("##\n..\n#.\n.#\n#.\n", plot);
    }
    
    [Test]
    public void TestElfDoesntMoveIfAlone()
    {
        HashSet<IntVector2> positions = new();
        Day23.World world = new (positions, new List<Day23.MotionRule>());
        Assert.AreEqual(new IntVector2(0, 0), world.GetDestination(new IntVector2(0, 0)));
    }

    [Test]
    public void TestElfReturnRuleDestinationIfRuleCanBeApplied()
    {
        HashSet<IntVector2> positions = new();
        positions.Add(new IntVector2(1, 0));
        Mock<Day23.MotionRule> ruleMock = new();
        IntVector2 destination = new (1, 2);
        ruleMock.Setup(x => x.TryApply(new IntVector2(0, 0), out destination, positions))
                .Returns(true);
        Day23.World world = new (positions, new List<Day23.MotionRule>() {ruleMock.Object});
        
        Assert.AreEqual(new IntVector2(1, 2), world.GetDestination(new IntVector2(0, 0)));
    }

    [Test]
    public void TestOrthogonalRule()
    {
        Day23.OrthogonalRule rule = new(new[] { new IntVector2(-1, -1), new IntVector2(0, -1), new IntVector2(1, -2) },
                                        new IntVector2(2, 3));

        HashSet<IntVector2> positions = new();
        Assert.IsTrue(rule.TryApply(new IntVector2(5, 6), out var destination, positions));
        Assert.AreEqual(new IntVector2(7, 9), destination);
        
        positions.Add(new IntVector2(6, 4));
        Assert.IsFalse(rule.TryApply(new IntVector2(5, 6), out destination, positions));
    }
}
