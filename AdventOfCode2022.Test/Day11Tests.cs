namespace AdventOfCode2022.Test;

[TestFixture]
public class Day11Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "Monkey 0:",
        "  Starting items: 79, 98",
        "  Operation: new = old * 19",
        "  Test: divisible by 23",
        "    If true: throw to monkey 2",
        "    If false: throw to monkey 3",
        "",
        "Monkey 1:",
        "  Starting items: 54, 65, 75, 74",
        "  Operation: new = old + 6",
        "  Test: divisible by 19",
        "    If true: throw to monkey 2",
        "    If false: throw to monkey 0",
        "",
        "Monkey 2:",
        "  Starting items: 79, 60, 97",
        "  Operation: new = old * old",
        "  Test: divisible by 13",
        "    If true: throw to monkey 1",
        "    If false: throw to monkey 3",
        "",
        "Monkey 3:",
        "  Starting items: 74",
        "  Operation: new = old + 3",
        "  Test: divisible by 17",
        "    If true: throw to monkey 0",
        "    If false: throw to monkey 1",
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(10605, new Day11().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestParseMonkey()
    {
        Assert.AreEqual(new Day11.Monkey(new [] {79L, 98}, new Day11.MultiplyIntOperation(19), 23, 2, 3), Day11.MonkeyGroup.ParseMonkey(_sampleLines.Take(6).ToArray()));
    }

    [Test]
    public void TestParseMathOperation_Add()
    {
        Assert.AreEqual(new Day11.AddIntOperation(25), Day11.MonkeyGroup.ParseIntMathOperation("  Operation: new = old + 25"));
    }

    [Test]
    public void TestParseMathOperation_Multiply()
    {
        Assert.AreEqual(new Day11.MultiplyIntOperation(24), Day11.MonkeyGroup.ParseIntMathOperation("  Operation: new = old * 24"));
    }

    [Test]
    public void TestParseMathOperation_Square()
    {
        Assert.AreEqual(Day11.SquareIntOperationInstance, Day11.MonkeyGroup.ParseIntMathOperation("  Operation: new = old * old"));
    }

    [Test]
    public void TestExecuteMonkeyStep()
    {
        Day11.Monkey monkey0 = new Day11.Monkey(new[] { 79L, 98 }, new Day11.MultiplyIntOperation(19), 23, 2, 3);
        Day11.Monkey monkey3 = new Day11.Monkey(new[] { 25L }, new Day11.MultiplyIntOperation(1), 1, 1, 1);

        var monkeys = new[] { monkey0, null, null, monkey3 };
        monkey0.ExecuteStep(monkeys!, Day11.MonkeyGroup.Part1WorryLevelAdjustment);

        CollectionAssert.AreEqual(new [] {98}, monkey0.ItemWorries);
        CollectionAssert.AreEqual(new [] {25, 500}, monkey3.ItemWorries);
        Assert.AreEqual(1, monkey0.InspectionCount);
        Assert.AreEqual(0, monkey3.InspectionCount);
    }

    [Test]
    public void TestExecuteRound()
    {
        Day11.MonkeyGroup monkeys = Day11.MonkeyGroup.ParseMonkeys(_sampleLines, Day11.MonkeyGroup.Part1WorryLevelAdjustment);
        monkeys.ExecuteRound();
        CollectionAssert.AreEqual(new [] {20, 23, 27, 26}, monkeys[0].ItemWorries);
        CollectionAssert.AreEqual(new [] {2080, 25, 167, 207, 401, 1046}, monkeys[1].ItemWorries);
        CollectionAssert.IsEmpty(monkeys[2].ItemWorries);
        CollectionAssert.IsEmpty(monkeys[3].ItemWorries);
    }
    
    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(2713310158, new Day11().ExecutePart2(_sampleLines));
    }
}
