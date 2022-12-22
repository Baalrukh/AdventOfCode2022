namespace AdventOfCode2022.Test;

[TestFixture]
public class Day21Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "root: pppw + sjmn",
        "dbpl: 5",
        "cczh: sllz + lgvd",
        "zczc: 2",
        "ptdq: humn - dvpt",
        "dvpt: 3",
        "lfqf: 4",
        "humn: 5",
        "ljgn: 2",
        "sjmn: drzm * dbpl",
        "sllz: 4",
        "pppw: cczh / lfqf",
        "lgvd: ljgn * ptdq",
        "drzm: hmdt - zczc",
        "hmdt: 32"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(152, new Day21().ExecutePart1(_sampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(301, new Day21().ExecutePart2(_sampleLines));
    }
}
