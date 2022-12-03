namespace AdventOfCode2022.Test;

[TestFixture]
public class Day03Tests
{
    private static readonly string[] _sampleLines = new[]
    {
        "vJrwpWtwJgWrhcsFMMfFFhFp",
        "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
        "PmmdzqPrVvPwwTWBwg",
        "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
        "ttgJtRGJQctTZtZT",
        "CrZsJsPPZsGzwwsLwLmpwMDw"
    };

    [Test]
    public void TestPart1()
    {
        Assert.AreEqual(157, new Day03().ExecutePart1(_sampleLines));
    }
    
    [Test]
    public void TestPart2()
    {
        Assert.AreEqual(70, new Day03().ExecutePart2(_sampleLines));
    }
}
