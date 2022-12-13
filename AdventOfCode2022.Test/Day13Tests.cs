namespace AdventOfCode2022.Test;

[TestFixture]
public class Day13Tests
{
    private static readonly string[] _sampleLines1 = new[]
    {
        "[1,1,3,1,1]",
        "[1,1,5,1,1]",
    };

    private static readonly string[] _sampleLines2 = new[]
    {
        "[[1],[2,3,4]]",
        "[[1],4]",
    };

    private static readonly string[] _sampleLines3 = new[]
    {
        "[9]",
        "[[8,7,6]]",
    };

    private static readonly string[] _sampleLines4 = new[]
    {
        "[[4,4],4,4]",
        "[[4,4],4,4,4]",
    };

    private static readonly string[] _sampleLines5 = new[]
    {
        "[7,7,7,7]",
        "[7,7,7]",
    };

    private static readonly string[] _sampleLines6 = new[]
    {
        "[]",
        "[3]",
    };

    private static readonly string[] _sampleLines7 = new[]
    {
        "[[[]]]",
        "[[]]",
    };

    private static readonly string[] _sampleLines8 = new[]
    {
        "[1,[2,[3,[4,[5,6,7]]]],8,9]",
        "[1,[2,[3,[4,[5,6,0]]]],8,9]"
    };

    private static readonly string[][] _allSamples = new[]
        {_sampleLines1, _sampleLines2, _sampleLines3, _sampleLines4, _sampleLines5, _sampleLines6, _sampleLines7, _sampleLines8};

    private static string[] _allSampleLines => _allSamples.SelectMany(x => x.Concat(new[] {""})).ToArray();


    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(13, new Day13().ExecutePart1(_allSampleLines));
    }

    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(140, new Day13().ExecutePart2(_allSampleLines));
    }

    [Test]
    public void TestParseNode()
    {
        Assert.AreEqual(new Day13.ListNode(
            new Day13.PacketNode[]
            {
                new Day13.ListNode(new Day13.PacketNode[]
                {
                    new Day13.NumberNode(1), new Day13.NumberNode(2)
                }),
                new Day13.NumberNode(3),
                new Day13.NumberNode(4)
            }),
            Day13.ParseNode("[[1,2],3,4]"));
    }


    private static void TestSample(string[] sampleLines, bool expected)
    {
        Assert.AreEqual(expected, Day13.IsInRightOrder(Day13.ParseNode(sampleLines[0]), Day13.ParseNode(sampleLines[1])));
    }

    [Test]
    public void TestIntValue()
    {
        TestSample(_sampleLines1, true);
    }

    [Test]
    public void TestSample2()
    {
        TestSample(_sampleLines2, true);
    }

    [Test]
    public void TestSample3()
    {
        TestSample(_sampleLines3, false);
    }

    [Test]
    public void TestSample4()
    {
        TestSample(_sampleLines4, true);
    }

    [Test]
    public void TestSample5()
    {
        TestSample(_sampleLines5, false);
    }

    [Test]
    public void TestSample6()
    {
        TestSample(_sampleLines6, true);
    }

    [Test]
    public void TestSample7()
    {
        TestSample(_sampleLines7, false);
    }

    [Test]
    public void TestSample8()
    {
        TestSample(_sampleLines8, false);
    }

}
