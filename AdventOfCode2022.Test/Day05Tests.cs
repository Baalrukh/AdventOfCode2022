namespace AdventOfCode2022.Test;

[TestFixture]
public class Day05Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "    [D]    ",
        "[N] [C]    ",
        "[Z] [M] [P]",
        " 1   2   3 ",
        "",
        "move 1 from 2 to 1",
        "move 3 from 1 to 3",
        "move 2 from 2 to 1",
        "move 1 from 1 to 2",
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual("CMZ", new Day05().ExecutePart1Text(_sampleLines));
    }


    [Test]
    public void TestParseStack()
    {
        var stacks = Day05.ParseStacks(_sampleLines.Take(3).ToArray());
        Assert.AreEqual(3, stacks.Count);
        CollectionAssert.AreEqual(new [] {'N', 'Z'}, stacks[0].Crates);
        CollectionAssert.AreEqual(new [] {'D', 'C', 'M'}, stacks[1].Crates);
        CollectionAssert.AreEqual(new [] {'P'}, stacks[2].Crates);
    }

    [Test]
    public void TestParseCommand()
    {
        Assert.AreEqual(new Day05.StackCommand(1, 1, 2), Day05.ParseCommand("move 1 from 2 to 3"));
    }

    [Test]
    public void TestExecuteCommand()
    {
        var stacks = Day05.ParseStacks(_sampleLines.Take(3).ToArray());

        var stackCommand = new Day05.StackCommand(2, 1, 2);
        stackCommand.Execute(stacks);

        CollectionAssert.AreEqual(new [] {'N', 'Z'}, stacks[0].Crates);
        CollectionAssert.AreEqual(new [] {'M'}, stacks[1].Crates);
        CollectionAssert.AreEqual(new [] {'C', 'D', 'P'}, stacks[2].Crates);
    }

    [Test]
    public void TestExecuteCommandKeepOrder()
    {
        var stacks = Day05.ParseStacks(_sampleLines.Take(3).ToArray());

        var stackCommand = new Day05.StackCommand(2, 1, 2);
        stackCommand.ExecuteKeepOrder(stacks);

        CollectionAssert.AreEqual(new [] {'N', 'Z'}, stacks[0].Crates);
        CollectionAssert.AreEqual(new [] {'M'}, stacks[1].Crates);
        CollectionAssert.AreEqual(new [] {'D', 'C', 'P'}, stacks[2].Crates);
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual("MCD", new Day05().ExecutePart2Text(_sampleLines));
    }
}
